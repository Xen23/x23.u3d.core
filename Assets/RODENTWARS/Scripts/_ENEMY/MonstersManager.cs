using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace X23
{

public class MonstersManager : MonoBehaviour
	{

	// Reference to the player's heatlh.
	public PlayerHealth playerHealth;   
	// The distance from our Camera View Frustrum we want to spawn enemies
	// to make sure they are not visisble when they spawn. I'm too lazy to
	// do any proper checks.
	public float bufferDistance = 200;
	// The time in seconds between each wave.
	public float timeBetweenWaves = 5f;
    // The time in seconds between each spawn in a wave.
    public float spawnTime = 3f;
    // The wave to start on.
	public int startingWave = 1;
    // The difficulty to start on.
	public int startingDifficulty = 1;
	// Reference to the Text component.
	public Text number;
	//public Text cheese;
	public Text time;
	//public Text wave;
	public Text enemies;
	//public Text kills;

	// The number of enemies left alive for the current wave.
	//[HideInInspector]
	public int enemiesAlive = 0;

    // A class depicting one wave with x number of entries.
    [System.Serializable]
    public class Wave {
        public Entry[] entries;

        // A class depicting one wave entry.
        [System.Serializable]
        public class Entry {
            // The enemy type to spawn.
            public GameObject enemy;
            // The number of enemies to spawn.
            public int count;
            // A counter telling us how many have been spawned so far.
            [System.NonSerialized]
            public int spawned;
        }
    }

    // All our waves.
    public Wave[] waves;

    // Misc private variables needed to make everything work.
    [SerializeField] Vector3 spawnPosition = Vector3.zero;
	[SerializeField] int waveNumber;
	[SerializeField] float timer;
	[SerializeField] Wave currentWave;
	[SerializeField] int spawnedThisWave = 0;
	[SerializeField] int totalToSpawnForWave;
	[SerializeField] bool shouldSpawn = false;
	[SerializeField] int difficulty;

	[SerializeField] float aliveTimer;
	[SerializeField] string aliveTimerString;

	void Start() {
	if (!playerHealth)
	{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			if (player) playerHealth = GetComponent<PlayerHealth>();
		}
		// Let us start on a higher wave and difficulty if we wish.
		waveNumber = startingWave > 0 ? startingWave - 1 : 0;
		difficulty = startingDifficulty;

		// Start the next, ie. the first wave.
		StartCoroutine("StartNextWave");
	}
	
	void Update()
	{
		// This is false while we're setting up the next wave.
		if (!shouldSpawn) {
			return;
        }

		// Start the next wave when we've spawned all our enemies and the player
		// has killed them all.
		if ((spawnedThisWave >= totalToSpawnForWave) && enemiesAlive <= 0)
		{
			StartCoroutine("StartNextWave");
			return;
		}

		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;
		aliveTimer += Time.deltaTime;
		float timeInSecond = aliveTimer;// 65.0f;
		//int iTime = Mathf.FloorToInt(timeInSecond);
		int iMinutes = Mathf.FloorToInt(timeInSecond / 60f);
		int iSeconds = Mathf.FloorToInt(timeInSecond - iMinutes * 60);
		string niceTime = string.Format("{0:0}:{1:00}", iMinutes, iSeconds);
		aliveTimerString = niceTime;
		number.text = "" + (waveNumber).ToString();
		enemies.text = "" + enemiesAlive.ToString();
		time.text = "" + aliveTimerString;
		// This will give you times in the 0:00 format.If you'd rather have 00:00, simply do
		// string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

		// If the timer exceeds the time between attacks, the player is in range and this enemy is alive attack.
		if (timer >= spawnTime) {
		// Spawn one enemy from each of the entries in this wave.
		// The difficulty multiplies the number of spawned enemies for each
		// "loop", that is each full run through all the waves.
			foreach (Wave.Entry entry in currentWave.entries) {
				if (entry.spawned < (entry.count * difficulty)) {
					Spawn(entry);
				}
			}
		}
	}
	/**
	 * 
	 */
	IEnumerator StartNextWave()
	{
		shouldSpawn = false;

		yield return new WaitForSeconds(timeBetweenWaves);

		if (waveNumber == waves.Length) {
			waveNumber = 0;
			difficulty++;
		}

		currentWave = waves[waveNumber];

        // The difficulty multiplies the number of spawned enemies for each
        // "loop", that is each full run through all the waves.
        totalToSpawnForWave = 0;
		foreach (Wave.Entry entry in currentWave.entries) {
			totalToSpawnForWave += (entry.count * difficulty);
		}

		spawnedThisWave = 0;
		shouldSpawn = true;

		waveNumber++;

		//if (number != null) number.text = (waveNumber + ((difficulty - 1) * waves.Length)).ToString();
		//if (number != null) number.GetComponent<Animation>().Play();
	}

	/**
	 * Spawn enemies.
 	 * 
	 * This method is called at regular intervals, but all the ways this function 
	 * can end up not spawning an enemy means it could be many intervals between each 
	 * actual spawn and our enemies will spawn very irregularly. I guess that just 
	 * makes it seem more random though. And I'm lazy. :p
	 */
	void Spawn(Wave.Entry entry) {
		// Reset the timer.
		timer = 0f;
		
		// If the player has no health left, stop spawning.
		if (!(playerHealth == null))
		{
			if (playerHealth.currentHealth <= 0f)
			{
				//return;
			}
		}
		// Find a random position roughly on the level.
		Vector3 randomPosition = Random.insideUnitSphere * 1500;
		if (randomPosition.y < 0) randomPosition.y = 100;
			/*		
					// Find the closest position on the nav mesh to our random position.
					// If we can't find a valid position return and try again.
					UnityEngine.AI.NavMeshHit hit;
					if (!UnityEngine.AI.NavMesh.SamplePosition(randomPosition, out hit, 5, 1)) {
						return;
					}

					// We have a valid spawn position on the nav mesh.
					spawnPosition = hit.position;

					// Check if this position is visible on the screen, if it is we
					// return and try again.
					Vector3 screenPos = Camera.main.WorldToScreenPoint(spawnPosition);
					if ((screenPos.x > -bufferDistance && screenPos.x < (Screen.width + bufferDistance)) && 
						(screenPos.y > -bufferDistance && screenPos.y < (Screen.height + bufferDistance))) 
					{
						return;
					}
			*/
			spawnPosition = randomPosition;
			GameObject enemy;
			// We passed all the checks, spawn our enemy.
//			NetworkManager NWM = NetworkLobbyManager.singleton;
			bool Active = false;
/*			if (NWM != null && NWM.isActiveAndEnabled)
			{
				Active = NWM.isNetworkActive;
			}
			if (Active)
            {
				enemy = Instantiate(entry.enemy, spawnPosition, Quaternion.identity) as GameObject;
//				NetworkServer.Spawn(enemy);
				//enemy = Network.Instantiate(entry.enemy, spawnPosition, Quaternion.identity, 0) as GameObject;
            }
            else
            {
                enemy = Instantiate(entry.enemy, spawnPosition, Quaternion.identity) as GameObject;
			}
*/
		GameObject MNPC = GameObject.Find("_MONSTERNPCS");
//		if (MNPC) enemy.gameObject.transform.SetParent(MNPC.transform);
		// Multiply health and score value by the current difficulty.
//		enemy.GetComponent<EnemyHealth>().startingHealth *= difficulty;
//		enemy.GetComponent<EnemyHealth>().scoreValue *= difficulty;
//		enemy.GetComponent<EnemyHealth>().waveManager = this;
		//enemy.GetComponent<EnemyHealth>().scoreManager = ;
		//enemy.GetComponent<EnemyHealth>().waveManager = ;
		entry.spawned++;
		spawnedThisWave++;
		enemiesAlive++;
	}

}

}
