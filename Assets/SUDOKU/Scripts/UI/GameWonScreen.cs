// File: STUIWinScreen.cs
using UnityEngine;
using UnityEngine.UIElements;

namespace SudokuToolkit
{
    public class GameWonScreen : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private SudokuGameDataSO gameData;
        private VisualElement winScreen;
        private SudokuGame gameManager;

        private void Awake()
        {
            gameManager = GetComponentInParent<SudokuGame>();
            if (!InitializeComponents()) return;
            SetupUI();
        }

        private bool InitializeComponents()
        {
            if (gameManager == null || uiDocument?.rootVisualElement == null)
            {
                Debug.LogError("[GameWonScreen] Missing required components.");
                return false;
            }
            return true;
        }

        private void SetupUI()
        {
            var root = uiDocument.rootVisualElement;
            winScreen = root.Q<VisualElement>("win-screen");
            if (winScreen == null)
            {
                Debug.LogError("[GameWonScreen] Win screen not found in UXML.");
                return;
            }
            var backToMenuButton = winScreen.Q<Button>("win-back-to-menu");
            if (backToMenuButton != null)
                backToMenuButton.RegisterCallback<ClickEvent>(_ => BackToMenu());
        }

        public void Show()
        {
            var statsLabel = winScreen.Q<Label>("win-stats");
            if (statsLabel != null)
            {
                int minutes = Mathf.FloorToInt(gameData.GetTimeElapsed() / 60);
                int seconds = Mathf.FloorToInt(gameData.GetTimeElapsed() % 60);
                statsLabel.text = $"Score: {gameData.GetScore()} | Time: {minutes:00}:{seconds:00}";
            }
            winScreen.style.display = DisplayStyle.Flex;
            winScreen.AddToClassList("visible");
        }

        private void BackToMenu()
        {
            winScreen.style.display = DisplayStyle.None;
            winScreen.RemoveFromClassList("visible");
            gameManager.GetComponent<MainMenuScreen>().ShowMenu(true);
        }
    }
}