using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Shatalmic
{
	public class Networking : MonoBehaviour
	{
		private float _timeout = 0f;
		private States _state = States.None;
		private NetworkDevice _deviceToConnect = null;
		private NetworkDevice _deviceToDisconnect = null;

		public class Characteristic
		{
			public string ServiceUUID;
			public string CharacteristicUUID;
			public bool Found;
		}

		public static List<Characteristic> Characteristics = new List<Characteristic>
		{
			new Characteristic { ServiceUUID = "37200001-7638-4216-B629-96AD40F79AA1", CharacteristicUUID = "37200002-7638-4216-B629-96AD40F79AA1", Found = false },
		};

		public Characteristic SampleCharacteristic = Characteristics[0];

		public bool AllCharacteristicsFound { get { return !(Characteristics.Where (c => c.Found == false).Any ()); } }
		public Characteristic GetCharacteristic (string serviceUUID, string characteristicsUUID)
		{
			return Characteristics.Where (c => IsEqual (serviceUUID, c.ServiceUUID) && IsEqual (characteristicsUUID, c.CharacteristicUUID)).FirstOrDefault ();
		}

		public class NetworkDevice
		{
			public string Name;
			public string Address;
			public bool Connected;
			public bool DoDisconnect;
		}

		public List<NetworkDevice> NetworkDeviceList;

		enum States
		{
			None,
			StartScan,
			Connect,
			Subscribe,
			Disconnect,
			Disconnecting,
		}

		void SetState (States newState, float timeout)
		{
			_state = newState;
			_timeout = timeout;
		}

		void Reset ()
		{
			_timeout = 0f;
			_state = States.None;
			_networkName = null;
			_deviceToConnect = null;
			_deviceToDisconnect = null;
			NetworkDeviceList = new List<NetworkDevice> ();
		}

		public Action<string> OnError = (error) => {
			BluetoothLEHardwareInterface.Log ("Error: " + error);
		};

		public Action<string> OnStatusMessage = (message) => {
			BluetoothLEHardwareInterface.Log (message);
		};

		public Action<NetworkDevice> OnDeviceReady;
		public Action<NetworkDevice> OnDeviceDisconnected;
		public Action<NetworkDevice, string, byte[]> OnDeviceData;

		public void Initialize (Action<string> onError, Action<string> onStatusMessage)
		{
			BluetoothLEHardwareInterface.Log ("BLNE: Bluetooth Low Energy Networking Initialize");

			if (onError != null)
				OnError = onError;
			if (onStatusMessage != null)
				OnStatusMessage = onStatusMessage;

			BluetoothLEHardwareInterface.Initialize (true, true, () => {

			}, (error1) => {
				if (error1.Contains("Peripheral mode is Not Available"))
				{
					if (OnError != null)
						OnError ("Client mode not available");
					
					// try central only mode
					BluetoothLEHardwareInterface.Initialize (true, false, () => {

					}, (error2) => {
						if (OnError != null)
							OnError (error2);
					});
				}
				else
				{
					if (OnError != null)
						OnError (error1);
				}
			});
		}

		bool _serverStarted = false;
		string _networkName;
		public void StartServer (string networkName, Action<NetworkDevice> onDeviceReady, Action<NetworkDevice> onDeviceDisconnected, Action<NetworkDevice, string, byte[]> onDeviceData)
		{
			if (!_serverStarted)
			{
				Reset ();

				_serverStarted = true;
				_networkName = networkName;

				OnDeviceReady = onDeviceReady;
				OnDeviceDisconnected = onDeviceDisconnected;
				OnDeviceData = onDeviceData;

				SetState (States.StartScan, 0.1f);
			}
		}

		public Action OnFinishedStoppingServer = null;
		public void StopServer (Action onFinishedStoppingServer)
		{
			if (_serverStarted)
			{
				BluetoothLEHardwareInterface.StopScan ();

				if (NetworkDeviceList != null && NetworkDeviceList.Count > 0)
				{
					OnFinishedStoppingServer = onFinishedStoppingServer;
					foreach (var device in NetworkDeviceList)
						device.DoDisconnect = true;
					SetState (States.Disconnect, 0.1f);
				}
				else if (onFinishedStoppingServer != null)
				{
					onFinishedStoppingServer ();
					_serverStarted = false;
				}
			}
			else if (onFinishedStoppingServer != null)
			{
				onFinishedStoppingServer ();
				_serverStarted = false;
			}
		}

		public void WriteDevice (NetworkDevice device, byte[] bytes, Action onWritten)
		{
			BluetoothLEHardwareInterface.WriteCharacteristic (device.Address, SampleCharacteristic.ServiceUUID, SampleCharacteristic.CharacteristicUUID, bytes, bytes.Length, true, (Characteristic) => {
				if (onWritten != null)
					onWritten ();
			});
		}

		public void StartClient (string networkName, string clientName, Action onStartedAdvertising, Action<string, string, byte[]> onCharacteristicWritten)
		{
			Reset ();

			BluetoothLEHardwareInterface.PeripheralName (networkName + ":" + clientName);

			BluetoothLEHardwareInterface.RemoveServices ();
			BluetoothLEHardwareInterface.RemoveCharacteristics ();

			BluetoothLEHardwareInterface.CBCharacteristicProperties properties =
				BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyRead |
				BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyWrite |
				BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyNotify;

			BluetoothLEHardwareInterface.CBAttributePermissions permissions =
				BluetoothLEHardwareInterface.CBAttributePermissions.CBAttributePermissionsReadable |
				BluetoothLEHardwareInterface.CBAttributePermissions.CBAttributePermissionsWriteable;

			BluetoothLEHardwareInterface.CreateCharacteristic (SampleCharacteristic.CharacteristicUUID, properties, permissions, null, 5, (characteristic, bytes) => {
				if (onCharacteristicWritten != null)
					onCharacteristicWritten (clientName, characteristic, bytes);
			});

			BluetoothLEHardwareInterface.CreateService (SampleCharacteristic.ServiceUUID, true, (characteristic) => {
				StatusMessage = "Created service";
			});

			BluetoothLEHardwareInterface.StartAdvertising (() => {
				StatusMessage = "8";
				if (onStartedAdvertising != null)
					onStartedAdvertising ();
			});
		}

		public void SendFromClient (byte[] bytes) {
			StatusMessage = "SendFromClient";
			BluetoothLEHardwareInterface.UpdateCharacteristicValue (SampleCharacteristic.CharacteristicUUID, bytes, bytes.Length);
		}

		public void StopClient (Action onStoppedAdvertising)
		{
			BluetoothLEHardwareInterface.StopAdvertising (() => {
				if (onStoppedAdvertising != null)
					onStoppedAdvertising ();
			});
		}

		public string StatusMessage
		{
			set
			{
				if (OnStatusMessage != null)
					OnStatusMessage (value);
			}
		}

		void Start ()
		{
			Reset ();
		}

		// Update is called once per frame
		void Update ()
		{
			if (_timeout > 0f)
			{
				_timeout -= Time.deltaTime;
				if (_timeout <= 0f)
				{
					_timeout = 0f;

					switch (_state)
					{
					case States.None:
						if (_deviceToConnect == null)
						{
							StatusMessage = "Can connected";
							_deviceToConnect = NetworkDeviceList.Where (d => !d.Connected).Select (d => d).FirstOrDefault ();
							if (_deviceToConnect != null)
							{
								StatusMessage = string.Format ("Need connect: {0}", _deviceToConnect.Name);
								SetState (States.Connect, 0.1f);
							}
						}
						break;

					case States.StartScan:
						_state = States.None;
						BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, deviceName) => {
							if (deviceName.StartsWith (_networkName))
							{
								StatusMessage = "Found " + address;

								if (NetworkDeviceList == null)
									NetworkDeviceList = new List<NetworkDevice> ();

								NetworkDeviceList.Add (new NetworkDevice { Name = deviceName, Address = address, Connected = false });

								if (_deviceToConnect == null)
									SetState (States.None, 0.01f);
							}

						}, null, true);
						break;

					case States.Connect:
						if (_deviceToConnect != null)
						{
							StatusMessage = string.Format ("Connecting to {0}...", _deviceToConnect.Name);

							BluetoothLEHardwareInterface.ConnectToPeripheral (_deviceToConnect.Address, null, null, (address, serviceUUID, characteristicUUID) => {
								var characteristic = GetCharacteristic (serviceUUID, characteristicUUID);
								if (characteristic != null)
								{
									characteristic.Found = true;

									if (AllCharacteristicsFound)
									{
										_deviceToConnect.Connected = true;
										SetState (States.Subscribe, 3f);
									}
								}
							}, (disconnectAddress) => {
								var networkDevice = NetworkDeviceList.Where (d => d.Address.Equals (disconnectAddress)).Select (d => d).FirstOrDefault ();
								if (networkDevice != null)
								{
									StatusMessage = "Disconnected from " + networkDevice.Name;
									if (OnDeviceDisconnected != null && OnDeviceDisconnected != null)
										OnDeviceDisconnected (networkDevice);

									NetworkDeviceList.Remove (networkDevice);
									StatusMessage = string.Format ("1 device count: {0}", NetworkDeviceList.Count);
									if (networkDevice == _deviceToDisconnect)
									{
										_deviceToDisconnect = null;
										SetState (States.Disconnect, 0.1f);
									}
								}
							});
						}
						break;

					case States.Subscribe:
						_state = States.None;
						StatusMessage = "Subscribe to device";
						BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (_deviceToConnect.Address, SampleCharacteristic.ServiceUUID, SampleCharacteristic.CharacteristicUUID, (deviceAddressNotify, characteristicNotify) => {
							var networkDevice = NetworkDeviceList.Where (d => d.Address.Equals (deviceAddressNotify)).Select (d => d).FirstOrDefault ();
							if (networkDevice != null && OnDeviceData != null)
							{
								OnDeviceReady (networkDevice);
								_deviceToConnect = null;
								StatusMessage = string.Format ("Device completely connected to {0}", networkDevice.Name);
								SetState (States.None, 0.1f);
							}
						}, (deviceAddressData, characteristricData, bytes) => {
							var networkDevice = NetworkDeviceList.Where (d => d.Address.Equals (deviceAddressData)).Select (d => d).FirstOrDefault ();
							if (networkDevice != null && OnDeviceData != null)
								OnDeviceData (networkDevice, characteristricData, bytes);
						});
						break;

					case States.Disconnect:
						_deviceToDisconnect = NetworkDeviceList.Where (d => d.DoDisconnect).Select (d => d).FirstOrDefault ();
						if (_deviceToDisconnect != null)
						{
							SetState (States.Disconnecting, 5f);
							if (_deviceToDisconnect.Connected)
							{
								BluetoothLEHardwareInterface.DisconnectPeripheral (_deviceToDisconnect.Address, (address) => {
									// since we have a callback for disconnect in the connect method above, we don't
									// need to process the callback here.
								});
							}
							else
							{
								NetworkDeviceList.Remove (_deviceToDisconnect);
								StatusMessage = string.Format ("2 device count: {0}", NetworkDeviceList.Count);

								_deviceToDisconnect = null;
								_state = States.None;
							}
						}
						else
						{
							_state = States.None;
							if (OnFinishedStoppingServer != null)
							{
								OnFinishedStoppingServer ();
								OnFinishedStoppingServer = null;
							}

							_serverStarted = false;
						}
						break;

					case States.Disconnecting:
						if (_deviceToDisconnect != null && NetworkDeviceList != null && NetworkDeviceList.Contains (_deviceToDisconnect))
						{
							StatusMessage = string.Format ("3 device count: {0}", NetworkDeviceList.Count);

							NetworkDeviceList.Remove (_deviceToDisconnect);
							_deviceToDisconnect = null;
						}

						// if we got here we timed out disconnecting, so just go to disconnected state
						SetState (States.Disconnect, 0.1f);
						break;
					}
				}
			}
		}

		bool IsEqual (string uuid1, string uuid2)
		{
			return (uuid1.ToUpper ().CompareTo (uuid2.ToUpper ()) == 0);
		}
	}
}
