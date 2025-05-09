// File: STUIMenu.cs
using UnityEngine;
using UnityEngine.UIElements;

namespace SudokuToolkit
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        private SudokuGame gameManager;
        private GameBoardTimerUI timer;
        private VisualElement menuScreen;
        private VisualElement gameScreen;

        private void Awake()
        {
            gameManager = GetComponent<SudokuGame>();
            timer = GetComponentInChildren<GameBoardTimerUI>();
            if (!InitializeComponents()) return;
            SetupUI();
        }

        private bool InitializeComponents()
        {
            if (gameManager == null || timer == null || uiDocument?.rootVisualElement == null)
            {
                Debug.LogError("[MainMenuScreen] Missing required components.");
                return false;
            }
            return true;
        }

        private void SetupUI()
        {
            var root = uiDocument.rootVisualElement;
            menuScreen = root.Q<VisualElement>("menu-screen");
            gameScreen = root.Q<VisualElement>("game-screen");
            if (menuScreen == null || gameScreen == null)
            {
                Debug.LogError("[MainMenuScreen] Menu or game screen not found in UXML.");
                return;
            }
            var newGameButton = root.Q<Button>("menu-new-game");
            if (newGameButton != null)
                newGameButton.RegisterCallback<ClickEvent>(_ => StartNewGame());
            var continueButton = root.Q<Button>("menu-continue");
            if (continueButton != null)
                continueButton.RegisterCallback<ClickEvent>(_ => gameManager.ContinueGame());
            var difficultyDropdown = root.Q<DropdownField>("menu-difficulty");
            if (difficultyDropdown != null)
            {
                difficultyDropdown.choices = new() { "Easy", "Medium", "Hard" };
                difficultyDropdown.value = "Medium";
            }
            var symmetryDropdown = root.Q<DropdownField>("menu-symmetry");
            if (symmetryDropdown != null)
            {
                symmetryDropdown.choices = new() { "Central", "Horizontal", "Vertical", "None" };
                symmetryDropdown.value = "Central";
            }
        }

        public void ShowMenu(bool hasSavedGame)
        {
            menuScreen.style.display = DisplayStyle.Flex;
            gameScreen.style.display = DisplayStyle.None;
            var continueButton = menuScreen.Q<Button>("menu-continue");
            if (continueButton != null)
                continueButton.style.display = hasSavedGame ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public void ShowGame()
        {
            menuScreen.style.display = DisplayStyle.None;
            gameScreen.style.display = DisplayStyle.Flex;
            timer.StartTimer();
        }

        private void StartNewGame()
        {
            var difficultyDropdown = menuScreen.Q<DropdownField>("menu-difficulty");
            var symmetryDropdown = menuScreen.Q<DropdownField>("menu-symmetry");
            var difficulty = difficultyDropdown?.value switch
            {
                "Easy" => STDifficulty.Easy,
                "Medium" => STDifficulty.Medium,
                "Hard" => STDifficulty.Hard,
                _ => STDifficulty.Medium
            };
            var symmetry = symmetryDropdown?.value switch
            {
                "Central" => STSymmetry.Central,
                "Horizontal" => STSymmetry.Horizontal,
                "Vertical" => STSymmetry.Vertical,
                "None" => STSymmetry.None,
                _ => STSymmetry.Central
            };
            gameManager.StartNewGame(difficulty, symmetry);
        }
    }
}