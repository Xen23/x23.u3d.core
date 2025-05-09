// File: STGameManager.cs
using UnityEngine;
using System.Collections.Generic;

namespace SudokuToolkit
{
    public class SudokuGame : MonoBehaviour
    {
        [SerializeField] private SudokuGameDataSO gameData;
        [SerializeField] private GameBoardGUI uiBoard;
        private SudokuGeneratorClass generator;
        private STGrid grid;
        private MainMenuScreen menu;
        private GameWonScreen winScreen;
        private readonly Stack<(int row, int col, int value)> undoStack = new();

        private void Awake()
        {
            generator = new SudokuGeneratorClass();
            menu = GetComponent<MainMenuScreen>();
            winScreen = GetComponentInChildren<GameWonScreen>();
            if (!InitializeComponents()) return;
            bool hasSavedGame = !string.IsNullOrEmpty(PlayerPrefs.GetString("STLastGame", ""));
            menu.ShowMenu(hasSavedGame);
        }

        private bool InitializeComponents()
        {
            if (menu == null || winScreen == null || uiBoard == null || gameData == null)
            {
                Debug.LogError("[SudokuGame] Missing required components.");
                return false;
            }
            return true;
        }

        public void StartNewGame(STDifficulty difficulty, STSymmetry symmetry)
        {
            gameData.Reset();
            gameData.SetDifficulty(difficulty);
            gameData.SetSymmetry(symmetry);
            grid = generator.Generate(difficulty, symmetry);
            gameData.SetPuzzleState(generator.Serialize(grid));
            uiBoard.DisplayGrid(grid);
            menu.ShowGame();
            undoStack.Clear();
            SaveGame();
        }

        public void ContinueGame()
        {
            string savedState = PlayerPrefs.GetString("STLastGame", "");
            if (!string.IsNullOrEmpty(savedState))
            {
                grid = generator.Deserialize(savedState);
                gameData.SetPuzzleState(savedState);
                uiBoard.DisplayGrid(grid);
                menu.ShowGame();
                undoStack.Clear();
            }
        }

        public void RequestHint()
        {
            if (generator.GetHint(grid, out int row, out int col, out int value))
            {
                grid.Tiles[row, col].Value = value;
                grid.Tiles[row, col].IsFixed = true;
                gameData.IncrementHintsUsed();
                gameData.AdjustScore(-10);
                uiBoard.UpdateTile(row, col, value, true, isHint: true);
                SaveGame();
            }
        }

        public void Undo()
        {
            if (undoStack.Count == 0) return;
            var (row, col, value) = undoStack.Pop();
            grid.Tiles[row, col].Value = value;
            uiBoard.UpdateTile(row, col, value, false);
            SaveGame();
        }

        public void Validate()
        {
            bool isValid = generator.Validate(grid, true);
            Debug.Log($"[SudokuGame] Grid is {(isValid ? "valid and complete" : "invalid or incomplete")}.");
            uiBoard.UpdateConflictStates();
            if (isValid)
            {
                gameData.AdjustScore(CalculateScore());
                SaveGame();
                winScreen.Show();
            }
        }

        public void UpdateTile(int row, int col, int value)
        {
            if (grid == null || grid.Tiles[row, col].IsFixed) return;
            undoStack.Push((row, col, grid.Tiles[row, col].Value));
            grid.Tiles[row, col].Value = value;
            uiBoard.UpdateTile(row, col, value, false);
            SaveGame();
        }

        private int CalculateScore()
        {
            int baseScore = gameData.GetDifficulty() switch
            {
                STDifficulty.Easy => 100,
                STDifficulty.Medium => 200,
                STDifficulty.Hard => 300,
                _ => 200
            };
            int timePenalty = Mathf.FloorToInt(gameData.GetTimeElapsed() / 60) * 2;
            return Mathf.Max(0, baseScore - gameData.GetHintsUsed() * 10 - timePenalty);
        }

        private void SaveGame()
        {
            gameData.SetPuzzleState(generator.Serialize(grid));
            PlayerPrefs.SetString("STLastGame", gameData.GetPuzzleState());
            PlayerPrefs.Save();
        }

        public STGrid GetGrid() => grid;
        public SudokuGeneratorClass GetGenerator() => generator;
    }
}