using System;
using UnityEngine;

public class MouseDragRotate : MonoBehaviour {
	public float rotationSpeed = 1f;

	public Action<Quaternion> OnMouseEvent = null;

	void OnMouseDrag()
	{
		float XaxisRotation = Input.GetAxis("Mouse X")*rotationSpeed;
		float YaxisRotation = Input.GetAxis("Mouse Y")*rotationSpeed;
		// select the axis by which you want to rotate the GameObject
		transform.Rotate (Vector3.down, XaxisRotation);
		transform.Rotate (Vector3.right, YaxisRotation);

		if (OnMouseEvent != null)
			OnMouseEvent (transform.rotation);
	}
}
