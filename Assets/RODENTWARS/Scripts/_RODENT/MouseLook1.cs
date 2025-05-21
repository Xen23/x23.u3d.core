using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook1 : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	public GameObject SmoothFollowGameObject;
	public UnityStandardAssets.Utility.SmoothFollow SmoothFollowComponent;
	[Range (0F, 360F)] public float testFOVX = 180F;
	[Range (0F, 360F)] public float testFOVY = 120F;
	[SerializeField] float minimumX;
	[SerializeField] float maximumX;
	[SerializeField] float minimumY;
	[SerializeField] float maximumY;

	float rotationX = 0F;
	float rotationY = 0F;
	float distance = 0.0f;
	float startDistance = 0.0f;
	[SerializeField] float zoomDistance = 0.0f;
	public float distanceMin = -10f;
	public float distanceMax = 10f;
	public float zoomSensitivity = 5f;
    CursorLockMode LockState;// = Cursor.lockState;
    bool LockStateVisible;// = Cursor.visible;

    void doZoomAdjust()
	{

	}
	void doFieldOfView()
	{
		minimumX = (testFOVX / 2) - testFOVX ;
		maximumX = testFOVX / 2 ;
		minimumY = (testFOVY / 2) - testFOVY ;
		maximumY = testFOVY / 2;
	}

	float upOffSet = 0f;
	float startUpOffSet = 0f;
	void Update ()
	{
		if (axes == RotationAxes.MouseXAndY)
		{
			if (Input.GetMouseButton(2)) 
			{
				zoomDistance += Input.GetAxis("Mouse Y") * zoomSensitivity;
			}else{
				zoomDistance += Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
			}
			if (zoomDistance < distanceMin) zoomDistance = distanceMin;
			if (zoomDistance > distanceMax) zoomDistance = distanceMax;

			if (zoomDistance > 0f)
			{
				doFieldOfView();
				minimumX = minimumX - zoomDistance/3.3333f;
				maximumX = maximumX + zoomDistance/3.3333f;
				if (minimumX < -180) minimumX = -180;
				if (maximumX > 180) maximumX = 180;
				if (minimumX >= 0) minimumX = 0;
				if (maximumX <= 0) maximumX = 0;
				upOffSet = startUpOffSet;
			}

			if (zoomDistance == 0f)
			{
				doFieldOfView();
				upOffSet = startUpOffSet;
			}

			if (zoomDistance < 0f)
			{
				doFieldOfView();
				minimumX = minimumX - zoomDistance/3.3333f;
				maximumX = maximumX + zoomDistance/3.3333f;
				if (minimumX < -180) minimumX = -180;
				if (maximumX > 180) maximumX = 180;
				if (minimumX >= 0) minimumX = 0;
				if (maximumX <= 0) maximumX = 0;
				upOffSet = startUpOffSet - zoomDistance / 5;
			}


			//minimumX = minimumX - zoomDistance / 10;

			distance = Mathf.Clamp(zoomDistance, distanceMin, distanceMax);
			//distance = startDistance + distance;
			Vector3 position = new Vector3(transform.localPosition.x, upOffSet, distance);
          if (SmoothFollowGameObject)
          {
							SmoothFollowComponent = SmoothFollowGameObject.GetComponent<UnityStandardAssets.Utility.SmoothFollow>();
							SmoothFollowComponent.distance = -distance;
							//if (distance < 0) SmoothFollowComponent.distance = -distance; 
              //if (distance > 0) SmoothFollowComponent.distance = distance;
          }
          //float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
          //rotationX = Mathf.Clamp (rotationX, minimumX, maximumX);
          Vector3 euAng = transform.localEulerAngles;
          if (Input.GetMouseButtonDown(1))
          {
              LockState = Cursor.lockState;
              LockStateVisible = Cursor.visible;
          }
          if (Input.GetMouseButton(1)) 
			{
                GetComponent<UnityStandardAssets.Utility.SmoothFollow>().distance = 15f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

				if (!Input.GetMouseButton(2)) 
			    {
			        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
	    		    rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
    		    }

			    rotationX -= Input.GetAxis("Mouse X") * sensitivityX;
			    rotationX = Mathf.Clamp (rotationX, minimumX, maximumX);

			    transform.localEulerAngles = new Vector3(-rotationY, -rotationX, 0);

			}else{
                //LockState = Cursor.lockState;
                //LockStateVisible = Cursor.visible;
//                rotationX = 0;
//				rotationY = 0;
//				transform.localEulerAngles = euAng;
				//Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
				//    Cursor.lockState = LockState;
				//    Cursor.visible = LockStateVisible;
				rotationX = 0;
				rotationY = 0;
				transform.localEulerAngles = euAng;

				//Cursor.lockState = CursorLockMode.None;
				//Cursor.visible = true;
			}
			transform.localPosition = position;
		}
		else if (axes == RotationAxes.MouseX)
		{
			rotationX = Input.GetAxis("Mouse X") * sensitivityX;
			rotationX = Mathf.Clamp (rotationX, minimumX, maximumX);

			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
		//startDistance = transform.position.z;
	}
	
	void Start ()
	{
	    // set min/max'es to half the field-of-view
		//minimumX = (testFOVX / 2) - testFOVX ;
		//maximumX = testFOVX / 2 ;
		//minimumY = (testFOVY / 2) - testFOVY ;
		//maximumY = testFOVY / 2;
		doFieldOfView();

		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().freezeRotation = true;

		startUpOffSet = transform.localPosition.y;
		startDistance = transform.localPosition.z;
	    ////
	    //Vector3 angles = transform.eulerAngles;
        //x = angles.y;
        //y = angles.x;
		
	}

	//float xSpeed = 100.0f;
	//float ySpeed = 100.0f;
	//float yMinLimit = 2f;
	//float yMaxLimit = 80f;
	//private float x = 0.0f;
	//private float y = 0.0f;
	//public Transform target;
    void LateUpdate ()
    {
       // if (target) {
            //x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            //y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
		    //y = ClampAngle(y, yMinLimit, yMaxLimit);
			//Quaternion rotation = Quaternion.Euler(y, x, 0);
		    //zoomDistance += Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
		    //distance = Mathf.Clamp(zoomDistance, distanceMin, distanceMax);
		    //distance = startDistance + distance;
    		//Vector3 position = new Vector3(transform.position.x, transform.position.y, distance);
            //transform.rotation = rotation;
//            transform.position = position;
        //}
    }  

    //static float ClampAngle (float angle, float min, float max)
    //{
    //    if (angle < -360) angle += 360;
    //    if (angle > 360) angle -= 360;
    //    return Mathf.Clamp (angle, min, max);
    //}

}
