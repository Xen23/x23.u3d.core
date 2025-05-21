using UnityEngine;
using System.Collections;
using UnityEngine.AI;
namespace X23
{
	
public class EnemyMovement : MonoBehaviour
    {

	// A layer mask so the raycast only hits things on the shootable layer.
	public LayerMask shootableMask;  
	public float roamSpeed = 1.5f;
	public float attackSpeed = 104;

	// Reference to the player's position.
	public Transform player;       
	// Reference to the player's health.
	public PlayerHealth playerHealth;
        // Reference to this enemy's health.
        public EnemyHealth enemyHealth;     
	// Reference to the nav mesh agent.
	UnityEngine.AI.NavMeshAgent nav;   
	// Reference to the animator.
	Animator anim;
	// Reference the renderer.
	SkinnedMeshRenderer myRenderer;
	// A ray from the gun end forwards.
	Ray shootRay;          
	// A raycast hit to get information about what was hit.
	RaycastHit shootHit;    
	Vector3 position;
    public bool hasValidTarget = false;
    public bool foundPlayer = false;

	void Awake() {
//        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
//        enemyHealth = GetComponent<EnemyHealth>();
    }
    void Update() {
        if (nav == null) nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (enemyHealth == null) enemyHealth = GetComponent<EnemyHealth>();

        //if (player == null)
        //{
		
        GameObject goPlayer = GameObject.FindGameObjectWithTag("Player");
        if (!(goPlayer == null))
        {
            player = goPlayer.transform;
        }
        //}
        if ((player != null))// && (playerHealth == null))
        {
            playerHealth = player.GetComponentInChildren<PlayerHealth>();
        }

        //if (playerHealth != null) SetRandomNavTarget();
        //		playerHealth = player.GetComponent<PlayerHealth> ();
        //		if (enemyHealth.currentHealth > 0) {
        // Control the speed of the walk animation with the velocity we're moving at.
        //print ("Speed: " + nav.speed + " Velocity: " + nav.velocity.magnitude);
        float currentSpeed = nav.velocity.magnitude;


        if ((player != null) && (playerHealth != null))
        {
			position = GameObject.Find("#PLAYERWORLDPOS").transform.position;
			//position = player.position;
            hasValidTarget = true;
            foundPlayer = true;
            //myRenderer.materials[1].color = Color.Lerp(myRenderer.materials[1].color, new Color(1, 0, 0, 0), 2 * Time.deltaTime);
            //myRenderer.materials[1].SetColor("_RimColor", Color.Lerp(myRenderer.materials[1].GetColor("_RimColor"), new Color(1, 0, 0, 1), 2 * Time.deltaTime));
            nav.speed = attackSpeed;
            //Debug.Log("Seaching..");
/*
 *          Vector3 distanceFromTarget = position - transform.position;
			Vector3 direction = (player.position + new Vector3(0, 1, 0)) - (transform.position + new Vector3(0, 1, 0));
			shootRay.origin = transform.position + new Vector3(0, 1, 0);
			shootRay.direction = direction;
			if (Physics.Raycast (shootRay, out shootHit, 500, shootableMask)) {
                    //Debug.Log("Aiming.." + shootHit.transform.tag);
               if (shootHit.transform.tag == "Player") {
					position = player.position;
					hasValidTarget = true;
					foundPlayer = true;
					//myRenderer.materials[1].color = Color.Lerp(myRenderer.materials[1].color, new Color(1, 0, 0, 0), 2 * Time.deltaTime);
					//myRenderer.materials[1].SetColor("_RimColor", Color.Lerp(myRenderer.materials[1].GetColor("_RimColor"), new Color(1, 0, 0, 1), 2 * Time.deltaTime));
					nav.speed = attackSpeed;
				}
				else {
					//myRenderer.materials[1].color = Color.Lerp(myRenderer.materials[1].color, new Color(0, 0, 0, 0), 2 * Time.deltaTime);
					//myRenderer.materials[1].SetColor("_RimColor", Color.Lerp(myRenderer.materials[1].GetColor("_RimColor"), new Color(0, 0, 0, 1), 2 * Time.deltaTime));
				}
			}
*/
		}

        if (hasValidTarget) {
            float distanceFromNavMesh = GetAgentDistanceFromNavMesh(transform.gameObject);
            bool isWithin5OfNavMesh = IsAgentOnNavMesh(transform.gameObject);
            if (isWithin5OfNavMesh)
            {
                nav.enabled = true;
				if (nav.isOnNavMesh) nav.SetDestination(position);
				Vector3 direction = Vector3.Scale(transform.position, position).normalized;
				GetComponent<Rigidbody>().AddTorque(direction * 500f);
            }
           // Debug.Log(distanceFromNavMesh);
        }
        else
        {
            SetRandomNavTarget();
        }
    }
	// Don't set this too high, or NavMesh.SamplePosition() may slow down
    public bool IsAgentOnNavMesh(GameObject agentObject, float onMeshThreshold = 10f)
    {
        Vector3 agentPosition = agentObject.transform.position;
        NavMeshHit hit;

        // Check for nearest point on navmesh to agent, within onMeshThreshold
        if (NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, NavMesh.AllAreas))
        {
            // Check if the positions are vertically aligned
            if (Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z))
            {
                // Lastly, check if object is below navmesh
                return agentPosition.y >= hit.position.y;
            }
        }

        return false;
    }
    public float GetAgentDistanceFromNavMesh(GameObject agentObject)
    {
        Vector3 agentPosition = agentObject.transform.position;
        NavMeshHit hit;
        float distance;
        if (NavMesh.SamplePosition(agentPosition, out hit, 10f, NavMesh.AllAreas))
        {
            distance = agentPosition.y - hit.position.y;
        }
        else
        {
            distance = 0f;
        }
        return distance;
    }
	void SetRandomNavTarget() {
		Vector3 randomPosition = Random.insideUnitSphere * 30;
		randomPosition.y = 0;
		randomPosition += transform.position;
		UnityEngine.AI.NavMeshHit hit;
		hasValidTarget = UnityEngine.AI.NavMesh.SamplePosition(randomPosition, out hit, 5, 1);
		Vector3 finalPosition = hit.position;
		position = finalPosition;
    }
}

}