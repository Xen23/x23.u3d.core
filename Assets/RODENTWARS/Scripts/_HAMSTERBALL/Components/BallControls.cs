using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.CrossPlatformInput;

namespace X23
{
	public class BallControls: MonoBehaviour
	{
		public VehicleBattleSphereController HamsterBall;
		public Rigidbody RigidBody;
		public Camera PlayerView;
		public string ThrottleAxisName = "Vertical";
		public string StrafeAxisName = "Horizontal";
		[SerializeField] private KeyCode IgnoreMass = KeyCode.LeftShift; 
		[SerializeField] private KeyCode NoTorque = KeyCode.LeftAlt;
		[SerializeField] public Vector3 m_v3MovementInput;
		[SerializeField] public Vector3 m_v3MovementDirection;
        [SerializeField] private float m_fMoveInputAxisHorizontal;
        [SerializeField] private float m_fMoveInputAxisVertical;
        [SerializeField] private float _Mass;
        [SerializeField] private Vector3 _Velocity;
        [SerializeField] private Vector3 _CentreOfMass;
		float m_fIabc = 0f;
		
		void OnEnable()
		{
			m_v3MovementDirection = Vector3.zero;
			if (HamsterBall == null) HamsterBall = GetComponent<VehicleBattleSphereController>();
			if (HamsterBall != null) HamsterBall.Controls = this;
			if (RigidBody == null) RigidBody = GetComponentInChildren<Rigidbody>();
			if (PlayerView == null && HamsterBall != null) PlayerView = HamsterBall.Ballcam.PlayerView.GetComponentInChildren<Camera>();
			if (RigidBody)
			{
				RigidBody.mass = HamsterBall.SpherePhysics.m_fMass;
				RigidBody.drag = HamsterBall.SpherePhysics.m_fDrag;
				RigidBody.angularDrag= HamsterBall.SpherePhysics.m_fAngularDrag;
				RigidBody.maxAngularVelocity = HamsterBall.SpherePhysics.m_fMaxAngularVelocity;
			}
		}
		void Update()
		{
			// Read inputs
			m_fMoveInputAxisHorizontal = Input.GetAxisRaw(StrafeAxisName);
			m_fMoveInputAxisVertical = Input.GetAxisRaw(ThrottleAxisName);
			HamsterBall.SpherePhysics.m_bIgnoreMass = Input.GetKey(IgnoreMass); 
			HamsterBall.SpherePhysics.m_bUseTorque = !Input.GetKey(NoTorque);
			if (Input.GetKey(KeyCode.Q)) m_fIabc = -0.5f;
			if (Input.GetKey(KeyCode.E)) m_fIabc = 0.5f;
			// Monitor Rigidbody from here..
			if (RigidBody)
			{
				_Mass = RigidBody.mass;
				_Velocity = RigidBody.velocity;
				_CentreOfMass = RigidBody.worldCenterOfMass;
			}
            // Set movement based on camera view (or not).
			if (PlayerView != null)
			{
				m_v3MovementInput = (m_fMoveInputAxisVertical * HamsterBall.Ballcam.m_v3CameraZAxis + m_fMoveInputAxisHorizontal * PlayerView.transform.right).normalized;
			}
			else
			{
				m_v3MovementInput = (m_fMoveInputAxisVertical * Vector3.forward + m_fMoveInputAxisHorizontal * Vector3.right).normalized;
			}
			m_v3MovementInput = new Vector3(m_v3MovementInput.x, m_v3MovementInput.y + m_fIabc, m_v3MovementInput.z);
			m_fIabc = 0f;
		}
		void FixedUpdate()
		{
			HamsterBall.Move(m_v3MovementInput);
            //m_v3MovementDirection = Vector3.Scale(LastFrameWorldPos.normalized, WorldPos.normalized).normalized;
            //HamsterBall.PlayerController.SetPlayerPosition(transform);
			//transform.localPosition = Vector3.zero;
            //BallController.transform.position = transform.position;
			//transform.position = WorldPos;
			//UIMoving.rectTransform.ra.z = m_v3MovementDirection;
			//LastFrameWorldPos = transform.position;
		}
/*
		public void DoAddSpin(Vector3 moveDirection)
		{
			if (HamsterBall.SpherePhysics.m_bUseTorque)
			{
				if (HamsterBall.SpherePhysics.m_bIgnoreMass)
				{
					RigidBody.AddTorque(new Vector3(moveDirection.z, m_fIabc, -moveDirection.x) * HamsterBall.SpherePhysics.m_fMovePower, ForceMode.Acceleration);
				}
				else
				{
					RigidBody.AddTorque(new Vector3(moveDirection.z, m_fIabc, -moveDirection.x) * HamsterBall.SpherePhysics.m_fMovePower, ForceMode.Force);
				}
			}
			else
			{
				if (HamsterBall.SpherePhysics.m_bIgnoreMass)
				{
					RigidBody.AddForce(moveDirection * HamsterBall.SpherePhysics.m_fMovePower, ForceMode.Acceleration);
				}
				else
				{
					RigidBody.AddForce(moveDirection * HamsterBall.SpherePhysics.m_fMovePower, ForceMode.Force);
				}
			}
			m_fIabc = 0f;
		}
*/
	}
}