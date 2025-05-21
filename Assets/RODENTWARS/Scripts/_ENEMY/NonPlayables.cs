	using UnityEngine; using System.Collections; using System.Collections.Generic; // Unity v5 Defaults.
	using UnityEngine.Networking;
	/// +New xt_ class controller for xtClassNPCs.</summary>
	public class NonPlayables : MonoBehaviour
{
		// START
		public GameObject myPrefab;

		public void Start() {
			StartCoroutine(BallSpawner(5));
		}

		public void menuInit(int spawns) {
		    StartCoroutine(BallSpawner(spawns));
		}

		public IEnumerator BallSpawner(int spawns = 10) {
			//if (myPrefab == null);
			GameObject go = myPrefab;
			for(int i = 0 ; i < spawns ; i++)
			{
				int rand_num = Random.Range(-500,500);
				int rand_num2 = Random.Range(100,150);
				int rand_num3 = Random.Range(-500,500);
				//Vector3 position = Vector3.zero + Random.insideUnitSphere * 100;
			    //position.x = position.x + rand_num;
			    //position.y = position.y + rand_num2;
			    //position.z = position.z + rand_num3;
				Vector3 position = new Vector3(0f, 0f, 0f);
			    //go.transform.localScale.Set((go.transform.localScale.x + rand_num), (go.transform.localScale.y + rand_num2), (go.transform.localScale.z + rand_num3));
                //RaycastHit hitInfo;
                //if(Physics.Raycast(position + new Vector3(0, 1, 0), Vector3.down, out hitInfo, 10) && 
                //	hitInfo.collider.tag == "Spawnable")
                //Network.Instantiate(go, position, Random.rotation, 0);
                go.transform.SetParent(GameObject.Find("_MONSTERNPCS").transform);
                go.transform.SetParent(null);

				//AudioSource.PlayClipAtPoint("AWOOGA.wav", position);
				/*
				  if (NetworkManager.singleton)
				 
				{
					if (NetworkManager.singleton.isActiveAndEnabled)
					{
						NetworkIdentity.Instantiate(go, position, Quaternion.identity);
					}
					else
					{
						Instantiate(go, position, Quaternion.identity);
					}
				}
				*/
                //WaitForSeconds(69);
                Debug.Log("Spawner.");
				//BallSpawner(50);
			}
			yield return true;//BallSpawner(50); // BallSpawner(50);
				
		}

		void OnGUI() 
		{
			if (GUI.Button(new Rect(10, 170, 80, 20), "Spawner")) {
				Debug.Log("Ball Spawner.");
				//NonPlayables npGameObject = NonPlayables;
			menuInit(3);
			}
		}
}