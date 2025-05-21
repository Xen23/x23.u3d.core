using UnityEngine;
using System.Collections;

[System.Serializable]
public class BattleSphereStyles
{
	[Tooltip("Default colour of the ball.")]
	public Color m_coBallColor;

	[Tooltip("The length of the ray to check if the ball is grounded.")]
	[Range(0f, 100f)] public float m_fGroundRayLength = 1f;
}