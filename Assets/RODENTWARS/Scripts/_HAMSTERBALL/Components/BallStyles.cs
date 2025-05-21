using UnityEngine; using System.Collections; using System.Collections.Generic;
using UnityEngine.UI; using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.CrossPlatformInput;

namespace X23
{
    public class BallStyles : MonoBehaviour
	{
		public VehicleBattleSphereController BallController;
		public Renderer MainRenderer;
		public Renderer[] ChildRendererList;

		void OnEnable()
		{
			BallController = GetComponentInParent<VehicleBattleSphereController>();
			BallController.Styles = this;

			MainRenderer = GetComponent<Renderer>();
			ChildRendererList = GetComponentsInChildren<Renderer>();

			foreach (Renderer renderer in ChildRendererList)
			{
				renderer.material.color = BallController.SphereCustomisables.m_coBallColor;
			}
			MainRenderer.material.color = BallController.SphereCustomisables.m_coBallColor;
		}


		void Update()
		{

		}
	}
}