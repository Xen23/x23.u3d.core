using UnityEngine;
using System.Collections;
namespace X23
{
    public class DynamicItemsManager : MonoBehaviour
	{
	
	    public int scoreNeededForExtraBullet = 1500;
	    public int extraScoreNeededAfterEachPickup = 1500;

	    public Pickup healthPickup;
	    public Pickup bouncePickup;
	    public Pickup piercePickup;
	    public Pickup bulletPickup;

    }
}
