using UnityEngine;
using System.Collections;

[System.Serializable]
public class BattleSphereStats
{
	[Header("Vehicle Stat Powers")]
	// Dodge, Stealth, Grapple Power..
	[SerializeField] public int iRodentStatDodgePower;
	[SerializeField] public int iRodentStatStealthPower;
	[SerializeField] public int iRodentStatGrapplePower;
	// Grip, Bounce, Swerve Power..
	[SerializeField] public int iRodentStatGripPower;
	[SerializeField] public int iRodentStatBouncePower;
	[SerializeField] public int iRodentStatSwervePower;
	// Core, Skin, Gears Power..
	[SerializeField] public int iRodentStatCorePower;
	[SerializeField] public int iRodentStatSkinPower;
	[SerializeField] public int iRodentStatGearsPower;
	// Cannons, Turrets, Specials (Hardpoints)..
	[SerializeField] public int iRodentStatCanonPower;
	[SerializeField] public int iRodentStatTurretsPower;
	[SerializeField] public int iRodentStatSpecialsPower;
}