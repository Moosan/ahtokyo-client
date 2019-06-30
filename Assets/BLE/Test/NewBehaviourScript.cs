using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public void OnCentral()
    {
        BluetoothLEHardwareInterface.Initialize(true,false,()=> {
            Debug.Log("成功したよ");
        },_=> {
            Debug.Log("失敗したよ");
        });
    }
    public void OnPeripheral()
    {
        BluetoothLEHardwareInterface.Initialize(false, true, () => {
            Debug.Log("成功したよ");
        }, _ => {
            Debug.Log("失敗したよ");
        });
    }
    public void OnScan()
    {
        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null,(address,deviceName) =>
        {
            Debug.Log(address);
            Debug.Log(deviceName);
        });
    }
    public void OnStop()
    {
        BluetoothLEHardwareInterface.DeInitialize(()=> { Debug.Log("でぃいにじゃらいず"); });
    }
}
