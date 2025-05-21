using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace X23
{
	
public class Pickup : MonoBehaviour
	{

	public enum PickupType {Bullet, Bounce, Pierce, Health}
	public PickupType pickupType = PickupType.Bullet;
	public float rotateSpeed = 90f;
	
	public Text label;

	private Renderer[] quadRenderers;
	// Reference to the player GameObject.
	private GameObject player;  
	GameObject canvas;
	Light pickupLight;
	bool used = false;

	void Awake() {
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag("Player");
		quadRenderers = GetComponentsInChildren<Renderer>();
		canvas = GameObject.Find("PickupLabelCanvas");
		pickupLight = GetComponent<Light>();
	}

	void Start () {
		//label.gameObject.transform.SetParent(canvas.transform, false);
		//label.color = pickupLight.color;
		//label.transform.localScale = Vector3.one;
		//label.transform.rotation = Quaternion.identity;
	}

	void Update() {
		if (used) {
			return;
		}

		transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
		
		//Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		//label.transform.position = screenPos + new Vector3(0, 40, 0);
	}

	void OnTriggerEnter (Collider other) {
		if (used) {
			return;
		}
		if (player == null) player = GameObject.FindGameObjectWithTag("Player");
		if (other.gameObject != player && other.gameObject.tag != "Player") {
			return;
		}

		switch (pickupType) {
			case PickupType.Bullet:
				//if (other.GetComponentInChildren<PlayerShooting>().numberOfBullets <= 36) {
					//other.GetComponentInChildren<PlayerShooting>().numberOfBullets++;
				//}
				break;
				
			case PickupType.Bounce:
				//other.GetComponentInChildren<PlayerShooting>().BounceTimer = 0;
				break;
				
			case PickupType.Pierce:
				//other.GetComponentInChildren<PlayerShooting>().PierceTimer = 0;
				break;
				
			case PickupType.Health:
				other.GetComponentInChildren<PlayerHealth>().AddHealth(50);
				break;
		}

		if (GetComponent<AudioSource>()) GetComponent<AudioSource>().enabled = true;//.Play();

		foreach (Renderer quadRenderer in quadRenderers) {
			quadRenderer.enabled = false;
		}
		
		if (GetComponent<Collider>()) GetComponent<Collider>().enabled = false;

		if (pickupLight) pickupLight.enabled = false;
		//Destroy(label);

		used = true;

		Destroy(gameObject, 1);
	}
}

}