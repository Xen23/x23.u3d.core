using UnityEngine;
using System.Collections;
namespace X23
{

    public class EnemyAttack : MonoBehaviour
	{

	// The time in seconds between each attack.
	public float timeBetweenAttacks = 0.5f;  
	// The amount of health taken away per attack.
	public int attackDamage = 10;               
	   
	// Reference to the player GameObject.
	GameObject player;       
	// Reference to the player's health.
	PlayerHealth playerHealth;     
	// Reference to this enemy's health.
	EnemyHealth enemyHealth; 
	// Whether player is within the trigger collider and can be attacked.
	[SerializeField] bool playerInRange;   
	// Timer for counting up to the next attack.
	float timer;                               
	
	void Awake() {
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag("Player");
		if (!(player == null)) playerHealth = player.GetComponent<PlayerHealth>();
		enemyHealth = GetComponent<EnemyHealth>();
	}

	void OnTriggerEnter(Collider other) {
		// If the entering collider is the player the player is in range.
		if (other.gameObject.tag == "Player") {
			playerInRange = true;
			// Add a slight "reaction time".
			timer = 0.2f;
		}
	}
	
	
	void OnTriggerExit(Collider other) {
		// If the exiting collider is the player the player is no longer in range.
		if (other.gameObject.tag == "Player") { 
			playerInRange = false;
		}
	}
	
	
	void Update() {
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;
		
		// If the timer exceeds the time between attacks, the player is in range,
		// we are alive and the player is alive then attack.
		if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0) {
			Attack();
		}

        player = GameObject.FindGameObjectWithTag("Player");
		if (!(player == null)) playerHealth = player.GetComponentInChildren<PlayerHealth>();
		
	}
	
	
	void Attack() {
		// Reset the timer.
		timer = 0f;
		
		// Damage the player.
		playerHealth.TakeDamage(attackDamage);
	}
}

}