/**********************************************************
* Made with Unity. Baked with XenTek. Crafted by Discordia.
* Origin Date: 04/09/2017
* Author: Xen23; Discordia Design and Technology.
* Package Type: Unity C# behaviour script
*
* Package Name:
* Purpose:
* Options:
* Called by:
* Version:
* Last Updated:
**********************************************************/using UnityEngine; using System.Collections; using System.Collections.Generic;
public class contPowerUps : MonoBehaviour {

	// EDITOR
	void Reset() {} // Called in Editor (only!) when this script is attached or reset.
	// INITIALISATION
	void Awake() {}
	void OnEnable() {} // Called after re-enabling script (if script was disabled during the frame).

	// START
	void Start() { // Called once per script.

	}

	// PHYSICS
	void FixedUpdate() { // Fixed physics cycle. May occur more than once per frame if fixed timestep is quicker than frame updates.

	}
	// FixedUpdate is immediately followed by internal Unity physics calculations..
	void OnTriggerXXX() {} void OnCollisionXXX() {} // void yield WaitForFixedUpdate() {}

	// INPUT EVENTS
	void OnMouseXXX() {}

	// GAME LOGIC
	void Update() {
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}
	// Update is followed by optional yield functions and callbacks for WWW requests, etc...
	// void yield WaitForSeconds() {} void yield WWW() {} void yield StartCoroutine() {} // .. Which are followed by internal physics calculations.

	void LateUpdate() {

	}

	//SCENE RENDERING
	void OnWillRenderObject() {}
	void OnPreCull() {}
	void OnBecameVisible() {}
	void OnBecameInvisible() {}
	void OnPreRenderObject() {}
	void OnRenderObject() {}
	void OnPostRender() {}
	//void OnRenderImage() {}

	//GIZMO RENDERING
	void OnDrawGizmos() {} // OnDrawGizmos in only called in the Editor.

	//GUI RENDERING
	void OnGUI() { // OnGUI is called multiple times per frame update.

	}

	//END OF FRAME
	// void yield WaitForEndOfFrame() {} // End Of Frame.
	//PAUSING
	void OnApplicationPause() {} // Pausing adds a frame after the input frame of the pause input. Returns to FixedUpdate on UnPause.
	//DISABLE / ENABLING
	void OnDisable() {} // ONLY called if the script is diabled during the frame. Returns to OnEnable after re-enabling.
	//DECOMMISIONING
	void OnApplicationQuit() {}
	void OnDestroy() {}
}