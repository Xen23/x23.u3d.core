using UnityEngine;
using System.Collections;

[System.Serializable]
public class BattleSpherePhysics
{
	[Header("Vehicle Physics")]

	[Tooltip("The force added to the ball to move it.")]
	public float m_fMovePower = 100;

	[Tooltip("The mass of the ball.")]
	public float m_fMass = 10f;
	//[Header ("")]

	[Tooltip("The drag coefficient.")]
	public float m_fDrag = 10f;
	//[Header ("")]

	[Tooltip("The angular drag coefficient.")]
	public float m_fAngularDrag = 10f;
	//[Header ("")]

	[Tooltip("Whether or not to use torque to move the ball.")]
	public bool m_bUseTorque = true;

	[Tooltip("Whether or not to ignore the mass whilst moving the ball.")]
	public bool m_bIgnoreMass = true;

	[Tooltip("The maximum velocity the ball can rotate at.")]
	public float m_fMaxAngularVelocity = 50;

	[Tooltip("The force added to the ball when it jumps.")]
	public float m_fJumpPower = 50;
}