// File: STUIScore.cs
using UnityEngine;
using UnityEngine.UIElements;

namespace SudokuToolkit
{
    public class GameBoardScoreUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private SudokuGameDataSO gameData;
        private Label scoreLabel;

        private void Awake()
        {
            if (uiDocument?.rootVisualElement == null)
            {
                Debug.LogError("[GameBoardScoreUI] UIDocument or rootVisualElement is null.");
                return;
            }
            scoreLabel = uiDocument.rootVisualElement.Q<Label>("score");
            if (scoreLabel == null)
                Debug.LogError("[GameBoardScoreUI] Score label not found.");
            UpdateScore();
        }

        public void UpdateScore()
        {
            if (scoreLabel != null)
                scoreLabel.text = $"Score: {gameData.GetScore()}";
        }
    }
}