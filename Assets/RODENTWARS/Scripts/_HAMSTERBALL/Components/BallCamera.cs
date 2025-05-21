using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.CrossPlatformInput;

namespace X23
{
    public class BallCamera : MonoBehaviour
	{
		public VehicleBattleSphereController BallController;
		public GameObject PlayerView;

		public string LookXAxisName = "Mouse X";
		public string LookYAxisName = "Mouse Y";
		public string LookZAxisName = "Mouse Scrollwheel";
		[SerializeField] public Vector3 m_v3LookDirection;
		[SerializeField] public Vector3 m_v3CameraZAxis;
		[SerializeField] private float m_fMoveInputAxisMouseX;
		[SerializeField] private float m_fMoveInputAxisMouseY;
		[SerializeField] private float m_fMoveInputAxisMouseZ;
		bool AllowPlayerInput = false;

		void OnEnable()
		{
			BallController = GetComponentInParent<VehicleBattleSphereController>();
			BallController.Ballcam = this;
			if (!PlayerView) PlayerView = GameObject.FindGameObjectWithTag("MainCamera");
			AllowPlayerInput = true;
		}

		void Update()
		{
			if (AllowPlayerInput)
			{
				m_fMoveInputAxisMouseX = Input.GetAxisRaw(LookXAxisName);
				m_fMoveInputAxisMouseY = Input.GetAxisRaw(LookYAxisName);
				//m_fMoveInputAxisMouseZ = Input.GetAxisRaw(LookZAxisName);
			}
            if (!PlayerView) PlayerView = GameObject.FindGameObjectWithTag("MainCamera");
            m_v3LookDirection = (PlayerView.transform.up + PlayerView.transform.right + PlayerView.transform.forward).normalized;
			m_v3CameraZAxis = Vector3.Scale(PlayerView.transform.forward, new Vector3(1f, 1f, 1f)).normalized;
            BallController.PlayerController.v3CameraLookAngle = m_v3LookDirection;
        }

		void FixedUpdate()
		{
			FreeLookCam FreeLook = GetComponent<FreeLookCam>();
			AutoCam AutoCam = GetComponent<AutoCam>();
			UnityStandardAssets.Utility.SmoothFollow FollowTarget = GetComponent<UnityStandardAssets.Utility.SmoothFollow>();
			//ProtectCameraFromWallClip ClipProtect = GetComponent<ProtectCameraFromWallClip>();
			if (AllowPlayerInput)
			{
				//FreeLook.enabled = true;
				//AutoCam.enabled = false;
				//FollowTarget.alwaysLookAtTarget = BallController.transform;

			}
			else
			{
				//FreeLook.enabled = false;
				//AutoCam.enabled = true;
			}
		}
}
}