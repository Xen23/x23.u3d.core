using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace X23
{
	public class PlayerController : MonoBehaviour
	{
        [SerializeField] PlayersManager playersManager; // Reference to PM that spawned this controller
        [SerializeField] private int playerIndex = 0; // Index in PlayersList in the PM

        [SerializeField] private UnityEvent onConnect;
        [SerializeField] private UnityEvent onDisconnect;
        [SerializeField] private UnityEvent onSpawn;
        [SerializeField] private UnityEvent onDeath;
        [SerializeField] private UnityEvent onScore;
        [SerializeField] private UnityEvent onDamage;

        [SerializeField] private bool isInitialised = false;
        [SerializeField] private bool isLocalPlayer = true;
        [SerializeField] private bool isPlayingNow = false;
        [SerializeField] private bool isPausedNow = false;
        [SerializeField] private bool isLoadingNow = true;
        
        public HUD_Controller HUDUI;
        public BallControls Input;
        public GameObject goMainPlayerCameraRig;
        public Transform trPlayerRotationY;
        public Transform trPlayerRotationXZ;
        
        public Vector3 v3PlayerPosition;
        public Vector3 v3PlayerRotation;
        public Vector3 v3PlayerRotationXZ;
		public Vector3 v3CameraLookAngle;
		
		[SerializeField] public bool localPlayerControl = true;
        
		public int score;
		public Color color;
		public string playerName;
		public int lifeCount;

		protected Text _scoreText;
		protected float _shootingTimer = 0;

		//hard to control WHEN Init is called (networking make order between object spawning non deterministic)
		//so we call init from multiple location (depending on what between spaceship & manager is created first).
        public Transform trCameraAngle;

        private void Start()
        {
            
        }
        private void OnEnable()
        {
            
        }

        private void Awake()
        {
	        RegisterWithPlayersManager();
        }

	    public void SetPlayerPosition(Transform player)
	    {
	        transform.position = player.position;
	        if (!player || !trPlayerRotationY || !trPlayerRotationXZ) return;
	        Quaternion rot = player.rotation;
	        //Vector3 rotEU = player.rotation.eulerAngles;
	        //trPlayerRotationY.rotation = rot;
	        trPlayerRotationY.eulerAngles = new Vector3(0f, rot.eulerAngles.y, 0f);
	        //trPlayerRotationXZ.rotation = rot;
	        trPlayerRotationXZ.eulerAngles = new Vector3(rot.eulerAngles.x, 0f, rot.eulerAngles.z);
	        
	        v3PlayerPosition = transform.position;
	        v3PlayerRotation = player.rotation.eulerAngles;
	        v3PlayerRotationXZ = trPlayerRotationXZ.eulerAngles;

			UnityStandardAssets.Cameras.FreeLookCam FreeLook = FindObjectOfType<UnityStandardAssets.Cameras.FreeLookCam>();
	        trCameraAngle.rotation = FreeLook.m_Cam.transform.rotation;
	    }
	    public void SetupReferences()
		{
			if (isInitialised)
				return;
			
			if (PlayersManager.Instance == null)
			{
				Debug.Log("PlayersManager was not set when instantiated.");
			}
			else
			{
				return;
			}

			//playersManager.AddPlayer();//._allPlayers.Add(this);}
			
			isInitialised = true;
		}

		void OnDestroy()
		{
			DeRegisterWithPlayersManager();
            if (PlayersManager.Instance != null) PlayersManager.PlayersList.Remove(this);
		}

		public void RegisterPlayerWithManger(PlayersManager pm, int index)
		{
			playerIndex = index;
			playersManager = pm;
			onSpawn.Invoke();
		}
		
		private void RegisterWithPlayersManager()
		{
			if (PlayersManager.Instance != null) PlayersManager.Instance.RegisterPlayerRequest(this);
		}

		private void DeRegisterWithPlayersManager()
		{
			if (PlayersManager.Instance != null) PlayersManager.Instance.UnRegisterPlayerRequest(this);
		}

		
/*
		// --- Score & Life management & display
		void OnScoreChanged(int newValue)
		{
			score = newValue;
			UpdateScoreLifeText();
		}

		void OnLifeChanged(int newValue)
		{
			lifeCount = newValue;
			UpdateScoreLifeText();
		}

		void UpdateScoreLifeText()
		{
			if (_scoreText != null)
			{
				_scoreText.text = playerName + "\nSCORE : " + score + "\nLIFE : ";
				for (int i = 1; i <= lifeCount; ++i)
					_scoreText.text += "X";
			}
		}

		[Client]
		public void LocalDestroy()
		{
			//killParticle.transform.SetParent(null);
			//killParticle.transform.position = transform.position;
			//killParticle.gameObject.SetActive(true);
			//killParticle.time = 0;
			//killParticle.Play();

			if (!_canControl)
				return;//already destroyed, happen if destroyed Locally, Rpc will call that later

			//EnableSpaceShip(false);
		}

		//this tell the game this should ONLY be called on server, will ignore call on client & produce a warning
		[Server]
		public void Kill()
		{
			lifeCount -= 1;

			RpcDestroyed();
			if (lifeCount > 0)
			{
				//we start the coroutine on the manager, as disabling a gameobject stop ALL coroutine started by it
				//NetworkGameManager.sInstance.StartCoroutine(NetworkGameManager.sInstance.WaitForRespawn(this));
			}
		}

		[Server]
		public void Respawn()
		{
			RpcRespawn();
		}

		public void CreateBullets()
		{
		}

		// =========== NETWORK FUNCTIONS
		[Command]
		public void CmdFire(Vector3 position, Vector3 forward, Vector3 startingVelocity)
		{
			if (!isClient) //avoid to create bullet twice (here & in Rpc call) on hosting client
				CreateBullets();
			RpcFire();
		}

		//
		[Command]
		public void CmdCollideAsteroid()
		{
			Kill();
		}

		[ClientRpc]
		public void RpcFire()
		{
			CreateBullets();
		}

		//called on client when the player die, spawn the particle (this is only cosmetic, no need to do it on server)
		[ClientRpc]
		void RpcDestroyed()
		{
			LocalDestroy();
		}

		[ClientRpc]
		void RpcRespawn()
		{
		}
*/
	}
}