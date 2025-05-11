// File: STGameData.cs
using UnityEngine;

namespace SudokuToolkit
{
    public enum STDifficulty { Easy, Medium, Hard }
    public enum STSymmetry { Central, Horizontal, Vertical, None }

    [CreateAssetMenu(fileName = "STGameData", menuName = "SudokuToolkit/GameData")]
    public class SudokuGameDataSO : ScriptableObject
    {
        [SerializeField] private STDifficulty difficulty = STDifficulty.Medium;
        [SerializeField] private STSymmetry symmetry = STSymmetry.Central;
        [SerializeField] private float timeElapsed;
        [SerializeField] private int score;
        [SerializeField] private int hintsUsed;
        [SerializeField] private string puzzleState;

        public void Reset()
        {
            difficulty = STDifficulty.Medium;
            symmetry = STSymmetry.Central;
            timeElapsed = 0;
            score = 0;
            hintsUsed = 0;
            puzzleState = "";
        }

        public STDifficulty GetDifficulty() => difficulty;
        public void SetDifficulty(STDifficulty value) => difficulty = value;
        public STSymmetry GetSymmetry() => symmetry;
        public void SetSymmetry(STSymmetry value) => symmetry = value;
        public float GetTimeElapsed() => timeElapsed;
        public void SetTimeElapsed(float value) => timeElapsed = value;
        public int GetScore() => score;
        public void SetScore(int value) => score = value;
        public void AdjustScore(int delta) => score = Mathf.Max(0, score + delta);
        public int GetHintsUsed() => hintsUsed;
        public void IncrementHintsUsed() => hintsUsed++;
        public string GetPuzzleState() => puzzleState;
        public void SetPuzzleState(string value) => puzzleState = value;
    }
}