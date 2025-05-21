using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace X23

{
    public class WorldplayerGUIManager : MonoBehaviour
	{

	// The player's score.
	int score;
	int cheese;
	int kills;

	// Reference to the Text component.
	public Text scoreText; 
	public Text cheeseText;
	public Text killsText;

	void Awake()
	{
		score = 0;
		cheese = 0;
		kills = 0;
	}

	void Update()
	{
		if (scoreText != null) scoreText.text = score.ToString();
		if (cheeseText != null) cheeseText.text = cheese.ToString();
		if (killsText != null) killsText.text = kills.ToString();
	}

	public void AddScore(int toAdd)
	{
		score += toAdd;
	}

	public void AddCheese(int toAdd)
	{
		cheese += toAdd;
	}

	public void AddKills(int toAdd)
	{
		kills += toAdd;
	}

	public int GetScore()
	{
		return score;
	}

	public int GetCheese()
	{
		return cheese;
	}

	public int GetKills()
	{
		return kills;
	}
}

}
