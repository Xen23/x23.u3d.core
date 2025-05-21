using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.CrossPlatformInput;

namespace X23
{
	public class BallActions : MonoBehaviour
	{
		public VehicleBattleSphereController BallController;
		public Rigidbody RigidBody;
		public Vector3 MoveDirection;
		public Vector3 AimDirection;

		public KeyCode Key_ActionPrimary;
		public KeyCode Key_ActionSecondary;
		public KeyCode Key_SpeedModifier;
		public KeyCode Key_ReloadUseItem;

		protected bool m_bJump;
		protected bool m_bBoost;
		protected bool m_bThrust;
		protected bool m_bSubmit, m_bCancel, m_bFire1, m_bFire2, m_bFire3;
		protected bool m_bModifyer1, m_bModifyer2, m_bAction1, m_bAction2;//, m_fFire3, m_fJump;

		void OnEnable()
		{
			if (BallController == null) BallController = GetComponentInParent<VehicleBattleSphereController>();
			if (BallController != null) BallController.Actions = this;
			if (BallController != null) RigidBody = BallController.Controls.RigidBody;
			MoveDirection = Vector3.zero;
		}

		void Update()
		{
				MoveDirection = BallController.Controls.m_v3MovementInput;
				AimDirection = BallController.Ballcam.m_v3LookDirection;
				m_bSubmit = Input.GetButtonUp("Submit");
				m_bCancel = Input.GetButtonUp("Cancel");
				m_bFire1 = Input.GetButtonUp("Fire1");
				m_bFire2 = Input.GetButtonUp("Fire2");
				m_bFire3 = Input.GetButtonUp("Fire3");
				m_bJump = Input.GetButtonUp("Jump");
				m_bBoost = false;
				m_bThrust = false;
				if (Input.GetButtonDown("Jump")) m_bJump = true;
				if (Input.GetButton("Fire1")) m_bBoost = true;
				if (Input.GetButton("Fire3")) m_bThrust = true;
		}

		void FixedUpdate()
		{
			Actions(m_bJump, m_bThrust, m_bBoost);
			m_bJump = false;
			m_bThrust = false;
			m_bBoost = false;
			//if (isLocalPlayer || parentXPlayerController.localPlayerControl)
			//{
				//if (thrust >= 0) thrust = thrust - 0.01f;
				//			float fAxisRawHorizontal;
				//			float fAxisRawVertical;
				//			float fAxisRawFire1;
				//			fAxisRawHorizontal = Input.GetAxisRaw("Horizontal");
				//			fAxisRawVertical = Input.GetAxisRaw("Vertical");
				//			fAxisRawFire1 = Input.GetAxisRaw("Fire1");
				//			Debug.Log("Axis: Raw: H:" + fAxisRawHorizontal  + " V:" + fAxisRawVertical + " Fire1:" + fAxisRawFire1);
				//moveHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime * 100.0f;
				//moveVertical = Input.GetAxis("Vertical") * Time.deltaTime * 100.0f;
				//moveHorizontal = Input.GetAxis("Horizontal");
				//moveVertical = Input.GetAxis("Vertical");
				// Adjust the look angle by an amount proportional to the turn speed and horizontal input.
				//moveMouseX = Input.GetAxis("Mouse X");
				//moveMouseY = Input.GetAxis("Mouse Y");
				//			if ((moveVertical == 0) && (moveMouseY != 0)) {
				//				if (moveMouseY > 0) moveVertical = 1;// * Time.deltaTime * 100.0f;
				//				if (moveMouseY < 0) moveVertical = -1;// * Time.deltaTime * 100.0f;
				//			}
				//transform.Rotate(moveHorizontal, 0, 0);
				//transform.Translate(moveHorizontal, 0, moveVertical);
				//Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
				//GetComponent<Rigidbody>().AddForce(movement * speed);
				//GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * (speed * thrust));
				//RodentStats.iRodentStatExperiencePoints += 100;
			//}
		}

		public void Actions(bool bJump, bool bThrust, bool bBoost)
		{
			DoJump(bJump);
			DoBoost(bBoost, MoveDirection);
			DoThrust(bThrust, MoveDirection);
			if (Input.GetKeyUp(KeyCode.B)) RigidBody.velocity = Vector3.zero;
			if (Input.GetKeyUp(KeyCode.G) && RigidBody.mass == 0) RigidBody.mass = 500;
			if (Input.GetKeyUp(KeyCode.G) && RigidBody.mass != 0) RigidBody.mass = 0;
			if (Input.GetKeyUp(KeyCode.H)) transform.position = new Vector3(0f, 500f, 0f);
		}

		public void DoJump(bool blJump)
		{
			if (blJump)
				if (Physics.Raycast(transform.position, -Vector3.up, BallController.SphereCustomisables.m_fGroundRayLength))
				{
					RigidBody.AddForce(Vector3.up * BallController.SpherePhysics.m_fJumpPower * 10, ForceMode.Acceleration);
				}
		}

		public void DoBoost(bool blBoost, Vector3 moveDirection)
		{
			if (blBoost) RigidBody.AddTorque(new Vector3(moveDirection.z, 0f, -moveDirection.x) * BallController.SpherePhysics.m_fMovePower * 100, ForceMode.VelocityChange);
		}

		public void DoThrust(bool blThrust, Vector3 moveDirection)
		{
			if (blThrust)
			{
				RigidBody.AddForce(Vector3.up * BallController.SpherePhysics.m_fJumpPower * .001f, ForceMode.Impulse);
			}
		}

		public void DoAddSpin(Vector3 moveDirection)
		{
			if (BallController.SpherePhysics.m_bUseTorque)
			{
				if (BallController.SpherePhysics.m_bIgnoreMass)
				{
					RigidBody.AddTorque(new Vector3(moveDirection.z, moveDirection.y, -moveDirection.x) * BallController.SpherePhysics.m_fMovePower, ForceMode.Acceleration);
				}
				else
				{
					RigidBody.AddTorque(new Vector3(moveDirection.z, moveDirection.y, -moveDirection.x) * BallController.SpherePhysics.m_fMovePower, ForceMode.Force);
				}
			}
			else
			{
				if (BallController.SpherePhysics.m_bIgnoreMass)
				{
					RigidBody.AddForce(moveDirection * BallController.SpherePhysics.m_fMovePower, ForceMode.Acceleration);
				}
				else
				{
					RigidBody.AddForce(moveDirection * BallController.SpherePhysics.m_fMovePower, ForceMode.Force);
				}
			}
		}

	}
}