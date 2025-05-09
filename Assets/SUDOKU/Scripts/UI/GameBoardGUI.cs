// File: STUIBoard.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

namespace SudokuToolkit
{
    public class GameBoardGUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private VisualTreeAsset tileTemplate;
        private SudokuGame gameManager;
        private VisualElement gridElement;
        private Button[,] tileElements;
        private VisualElement numberPicker;
        private SudokuInputActions inputActions;
        private readonly List<InputAction> numberActions = new();
        private const float TileSize = 50f;

        private void Awake()
        {
            gameManager = GetComponentInParent<SudokuGame>();
            inputActions = new SudokuInputActions();
            if (!InitializeComponents()) return;
            SetupUI();
            SetupInput();
        }

        private bool InitializeComponents()
        {
            if (gameManager == null || uiDocument == null || tileTemplate == null)
            {
                Debug.LogError("[GameBoardGUI] Missing required components.");
                return false;
            }
            tileElements = new Button[STGrid.Size, STGrid.Size];
            return true;
        }

        private void SetupUI()
        {
            var root = uiDocument.rootVisualElement;
            gridElement = root.Q<VisualElement>("grid");
            numberPicker = root.Q<VisualElement>("number-picker");
            if (gridElement == null || numberPicker == null)
            {
                Debug.LogError("[GameBoardGUI] Grid or number picker not found in UXML.");
                return;
            }
            Debug.Log("[GameBoardGUI] Grid and number picker found.");

            // Setup number picker buttons
            numberPicker.style.display = DisplayStyle.None;
            for (int i = 1; i <= 9; i++)
            {
                int num = i;
                var button = new Button(() => OnNumberPicked(num)) { text = i.ToString() };
                button.AddToClassList("number-button");
                numberPicker.Add(button);
            }
            var clearButton = new Button(() => OnNumberPicked(0)) { text = "Clear" };
            clearButton.AddToClassList("number-button");
            numberPicker.Add(clearButton);

            // Setup game control buttons
            var hintButton = root.Q<Button>("hint-button");
            if (hintButton != null)
            {
                hintButton.RegisterCallback<ClickEvent>(_ => { gameManager.RequestHint(); Debug.Log("[GameBoardGUI] Hint button clicked."); });
            }
            else
            {
                Debug.LogError("[GameBoardGUI] Hint button not found.");
            }

            var validateButton = root.Q<Button>("validate-button");
            if (validateButton != null)
            {
                validateButton.RegisterCallback<ClickEvent>(_ => { gameManager.Validate(); Debug.Log("[GameBoardGUI] Validate button clicked."); });
            }
            else
            {
                Debug.LogError("[GameBoardGUI] Validate button not found.");
            }

            var undoButton = root.Q<Button>("undo-button");
            if (undoButton != null)
            {
                undoButton.RegisterCallback<ClickEvent>(_ => { gameManager.Undo(); Debug.Log("[GameBoardGUI] Undo button clicked."); });
            }
            else
            {
                Debug.LogError("[GameBoardGUI] Undo button not found.");
            }

            var menuButton = root.Q<Button>("menu-button");
            if (menuButton != null)
            {
                menuButton.RegisterCallback<ClickEvent>(_ =>
                {
                    GetComponentInParent<MainMenuScreen>().ShowMenu(true);
                    Debug.Log("[GameBoardGUI] Menu button clicked.");
                });
            }
            else
            {
                Debug.LogError("[GameBoardGUI] Menu button not found.");
            }
        }

        private void SetupInput()
        {
            inputActions.Game.Hint.performed += _ =>
            {
                gameManager.RequestHint();
                Debug.Log("[GameBoardGUI] Hint input action triggered.");
            };
            inputActions.Game.Validate.performed += _ =>
            {
                gameManager.Validate();
                Debug.Log("[GameBoardGUI] Validate input action triggered.");
            };
            inputActions.Game.Undo.performed += _ =>
            {
                gameManager.Undo();
                Debug.Log("[GameBoardGUI] Undo input action triggered.");
            };
            inputActions.Game.Enable();
            Debug.Log("[GameBoardGUI] Input actions enabled.");

            for (int i = 1; i <= 9; i++)
            {
                int num = i;
                var action = new InputAction($"Number{num}", InputActionType.Button, $"<Keyboard>/{num}");
                action.performed += _ =>
                {
                    OnNumberInput(num);
                    Debug.Log($"[GameBoardGUI] Number {num} input action triggered.");
                };
                action.Enable();
                numberActions.Add(action);
            }
        }

        private void OnDestroy()
        {
            inputActions.Dispose();
            foreach (var action in numberActions)
                action.Dispose();
        }

        public void DisplayGrid(STGrid grid)
        {
            if (grid?.Tiles == null)
            {
                Debug.LogError("[GameBoardGUI] Invalid grid.");
                return;
            }
            if (gridElement == null)
            {
                Debug.LogError("[GameBoardGUI] Grid element is null.");
                return;
            }
            gridElement.Clear();
            Debug.Log("[GameBoardGUI] Displaying grid.");
            for (int row = 0; row < STGrid.Size; row++)
            {
                for (int col = 0; col < STGrid.Size; col++)
                {
                    var tile = grid.Tiles[row, col];
                    var tileRoot = tileTemplate?.Instantiate();
                    if (tileRoot == null)
                    {
                        Debug.LogError($"[GameBoardGUI] Failed to instantiate tile at ({row},{col}).");
                        continue;
                    }
                    var tileElement = tileRoot.Q<Button>("tile");
                    if (tileElement == null)
                    {
                        Debug.LogError($"[GameBoardGUI] Tile Button not found at ({row},{col}).");
                        continue;
                    }
                    tileElement.text = tile.Value == 0 ? "" : tile.Value.ToString();
                    tileElement.AddToClassList(tile.IsFixed ? "tile-fixed" : "tile");
                    tileElement.style.left = new StyleLength(col * TileSize);
                    tileElement.style.top = new StyleLength(row * TileSize);
                    int r = row, c = col;
                    tileElement.RegisterCallback<PointerDownEvent>(_ =>
                    {
                        ShowNumberPicker(r, c);
                        Debug.Log($"[GameBoardGUI] Tile clicked at ({r},{c}).");
                    });
                    gridElement.Add(tileElement);
                    tileElements[row, col] = tileElement;
                    Debug.Log($"[GameBoardGUI] Added tile at ({row},{col}) with value {tile.Value}.");
                }
            }
        }

        public void UpdateTile(int row, int col, int value, bool isFixed, bool isHint = false)
        {
            var tileElement = tileElements[row, col];
            if (tileElement == null) return;
            tileElement.text = value == 0 ? "" : value.ToString();
            tileElement.RemoveFromClassList("tile");
            tileElement.RemoveFromClassList("tile-fixed");
            tileElement.RemoveFromClassList("tile-hint");
            tileElement.AddToClassList(isFixed ? "tile-fixed" : "tile");
            if (isHint)
            {
                tileElement.AddToClassList("tile-hint");
                tileElement.schedule.Execute(() => tileElement.RemoveFromClassList("tile-hint")).ExecuteLater(1000);
            }
        }

        private void ShowNumberPicker(int row, int col)
        {
            if (gridElement == null || numberPicker == null)
            {
                Debug.LogError("[GameBoardGUI] Grid or number picker is null.");
                return;
            }
            var tile = gameManager.GetGrid()?.Tiles[row, col];
            if (tile == null || tile.IsFixed)
            {
                Debug.Log($"[GameBoardGUI] Tile at ({row},{col}) is null or fixed.");
                return;
            }
            numberPicker.style.left = new StyleLength(col * TileSize);
            numberPicker.style.top = new StyleLength(row * TileSize + TileSize);
            numberPicker.style.display = DisplayStyle.Flex;
            numberPicker.userData = (row, col);
            Debug.Log($"[GameBoardGUI] Number picker shown at ({row},{col}).");
        }

        private void OnNumberPicked(int value)
        {
            if (numberPicker.userData is (int row, int col))
            {
                gameManager.UpdateTile(row, col, value);
                numberPicker.style.display = DisplayStyle.None;
                UpdateConflictStates();
                Debug.Log($"[GameBoardGUI] Number {value} picked for tile ({row},{col}).");
            }
            else
            {
                Debug.LogError("[GameBoardGUI] Number picker userData is invalid.");
            }
        }

        private void OnNumberInput(int value)
        {
            if (numberPicker.style.display == DisplayStyle.Flex && numberPicker.userData is (int row, int col))
            {
                OnNumberPicked(value);
            }
            else
            {
                Debug.Log($"[GameBoardGUI] Number input {value} ignored; number picker not visible.");
            }
        }

        public void UpdateConflictStates()
        {
            var grid = gameManager.GetGrid();
            if (grid == null) return;
            var conflicts = gameManager.GetGenerator().FindConflicts(grid);
            for (int row = 0; row < STGrid.Size; row++)
            {
                for (int col = 0; col < STGrid.Size; col++)
                {
                    var tileElement = tileElements[row, col];
                    if (tileElement != null)
                    {
                        tileElement.RemoveFromClassList("tile-conflict");
                        if (conflicts.Contains((row, col)))
                            tileElement.AddToClassList("tile-conflict");
                    }
                }
            }
        }
    }
}