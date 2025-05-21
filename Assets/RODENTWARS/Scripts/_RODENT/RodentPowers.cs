using UnityEngine; using System.Collections; using System.Collections.Generic; // Unity v5 Defaults.
																																						 //using UnityEngine.SceneManagement; using UnityEngine.Networking; using UnityEngine.UI;
namespace X23.RodentWars
{

[System.Serializable]
public class RodentPowers
{
		/* RodentPowers Fields Public */
		#region Public Fields
		[Header("Rodent Information")]
		[SerializeField] protected string stAccountName;
		[SerializeField] protected string stRodentName;
		[SerializeField] protected float flOnlineTimeTotal;
		[SerializeField] protected bool blIsAlive;

		[Header("Class, Level and Race")]
		//[SerializeField] protected string stRodentName;
		[SerializeField] [Range(1, 10)] public int iRace;
		[SerializeField] [Range(1, 10)] public int iClass;
		[SerializeField] [Range(1, 100)] public int iLevel;

		[Header("Health, Armour, XP, Mana Points..")]
		[SerializeField] public int HealthPoints;
		[SerializeField] public int ArmourPoints;
		[SerializeField] public int ManaPoints;
		// _dep
		[SerializeField] public int ExperiencePoints;
		// _dep

		[Header("Scrabble, Life, Toughs Power..")]
		[SerializeField] public int ScrabblePower;
		[SerializeField] public int LiftPower;
		[SerializeField] public int ToughsPower;

		[Header("Gnaw Scratch, Swish Power..")]
		[SerializeField] public int GnawPower;
		[SerializeField] public int ScratchPower;
		[SerializeField] public int SwishPower;

		[Header("Smarts, Wits, Quickness Power..")]
		[SerializeField] public int SmartsPower;
		[SerializeField] public int WitsPower;
		[SerializeField] public int QuicknessPower;

		[Header("Sway, Streets, Income Power..")]
		[SerializeField] public int SwayPower;
		[SerializeField] public int StreetsPower;
		[SerializeField] public int IncomePower;

		[Header("Choppas, Shootas, Sneeka Power..")]
		[SerializeField] public int ChoppasPower;
		[SerializeField] public int ShootasPower;
		[SerializeField] public int SneekaPower;

		[Header("Retro :p")]
		public float lv;
		public float hp;
		public float ap;
		public float mp;
		public float xp;
		#endregion
		/* RodentPowers Fields Protected/Private */
		#region Protected/Private Fields

		#endregion

		#region Unity Methods
		void Start()
    {
		
    }

    void Awake()
    {
		
    }

    void Update()
    {
		
    }

    void OnGUI()
    {
		
    }

    void FixedUpdate()
    {
		
    }

    void LateUpdate()
    {
		
    }

    void OnEnable()
    {
		
    }

    void OnDisable()
    {
		
    }
    #endregion
 
    #region Private Methods
    #endregion

}


}