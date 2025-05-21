using UnityEngine; using System.Collections; using System.Collections.Generic;
using UnityEngine.UI; using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.CrossPlatformInput;

namespace X23
{
	public class VehicleBattleSphereController : MonoBehaviour
	{
		public PlayerController PlayerController;
		public BattleSphereStats SphereStats = new BattleSphereStats();
		public BattleSpherePhysics SpherePhysics = new BattleSpherePhysics();
		public BattleSphereStyles SphereCustomisables = new BattleSphereStyles();
		public BallStyles Styles;
		public BallCamera Ballcam;
		public BallControls Controls;
		public BallActions Actions;

		void OnEnable()
		{
			Debug.Log("BATTLESPHERE!!");
			if (PlayerController == null) PlayerController = gameObject.GetComponentInParent<PlayerController>();
			if (Controls == null) Controls = GetComponentInParent<BallControls>();
			if (Styles == null) Styles = GetComponentInChildren<BallStyles>();
			if (Ballcam == null) Ballcam = GetComponentInChildren<BallCamera>();
			if (Actions == null) Actions = GetComponentInChildren<BallActions>();
		}

		void LateUpdate()
		{
			if (PlayerController.localPlayerControl)
			{
				if (Styles) Styles.gameObject.SetActive(true);
				if (Ballcam) Ballcam.gameObject.SetActive(true);
				if (Controls) Controls.gameObject.SetActive(true);
				if (Actions) Actions.gameObject.SetActive(true);
			}
			else
			{
				if (Styles) Styles.enabled = false;
				if (Ballcam) Ballcam.gameObject.SetActive(false);
				if (Controls) Controls.enabled = false;
				if (Actions) Actions.enabled = false;
			}
			if (Controls && PlayerController) PlayerController.v3PlayerPosition = Actions.transform.position;
		}
    
		public void Move(Vector3 moveDirection)
		{
			Actions.DoAddSpin(moveDirection);
		}

		void _OnTriggerEnter(Collider other)
		{
			Debug.Log("Collided:" + other.name);
		}
	}
}
