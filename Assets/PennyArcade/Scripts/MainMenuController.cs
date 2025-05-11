using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using XenTek.Core;
using XenTek.Core.Commands;

namespace PennyArcade.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private VisualTreeAsset uxmlAsset;
        [SerializeField] private ThemeStyleSheet themeAsset;
        [SerializeField] private InputActionAsset inputActions;

        private CommandManager commandManager;
        private UIDocument uiDocument;
        private InputAction navigateAction;
        private InputAction selectAction;
        private VisualElement focusedButton;
        private int focusedIndex;

        private void Awake()
        {
            commandManager = GetComponent<CommandManager>();
            if (commandManager == null)
            {
                Debug.LogError("CommandManager component missing on MainMenuController GameObject.");
            }

            uiDocument = GetComponent<UIDocument>();
            if (uiDocument == null)
            {
                uiDocument = gameObject.AddComponent<UIDocument>();
            }

            if (uxmlAsset != null)
            {
                uiDocument.visualTreeAsset = uxmlAsset;
            }
            if (themeAsset != null)
            {
                uiDocument.panelSettings.themeStyleSheet = themeAsset;
            }

            var root = uiDocument.rootVisualElement;
            if (Resources.Load<StyleSheet>("Penny Arcade Styles") is StyleSheet ussAsset)
            {
                root.styleSheets.Add(ussAsset);
            }
        }

        private void Start()
        {
            var backend = GetComponent<BackendService>();
            if (backend != null)
            {
                backend.CheckForUpdates("PennyArcade", result => Debug.Log($"Update check: {result}"));
            }
        }

        private void OnEnable()
        {
            if (inputActions != null)
            {
                navigateAction = inputActions.FindAction("Navigate");
                selectAction = inputActions.FindAction("Select");
                navigateAction?.Enable();
                selectAction?.Enable();
                navigateAction.performed += OnNavigate;
                selectAction.performed += OnSelect;
            }

            var root = uiDocument.rootVisualElement;
            root.Q<Button>("play-button")?.RegisterCallback<ClickEvent>(evt =>
            {
                commandManager.ExecuteCommand(new UIMenuCommand("Play", () => Debug.Log("Starting Game")));
            });

            root.Q<Button>("settings-button")?.RegisterCallback<ClickEvent>(evt =>
            {
                commandManager.ExecuteCommand(new UIMenuCommand("Settings", () => Debug.Log("Opening Settings")));
            });

            root.Q<Button>("exit-button")?.RegisterCallback<ClickEvent>(evt =>
            {
                commandManager.ExecuteCommand(new UIMenuCommand("Exit", Application.Quit));
            });

            var buttons = root.Query<Button>().ToList();
            if (buttons.Count > 0)
            {
                focusedIndex = 0;
                focusedButton = buttons[0];
                focusedButton.Focus();
            }
        }

        private void OnDisable()
        {
            navigateAction?.Disable();
            selectAction?.Disable();
            navigateAction.performed -= OnNavigate;
            selectAction.performed -= OnSelect;
        }

        private void OnNavigate(InputAction.CallbackContext context)
        {
            var buttons = uiDocument.rootVisualElement.Query<Button>().ToList();
            if (buttons.Count == 0) return;

            Vector2 input = context.ReadValue<Vector2>();
            if (input.y > 0.5f)
            {
                focusedIndex = (focusedIndex - 1 + buttons.Count) % buttons.Count;
            }
            else if (input.y < -0.5f)
            {
                focusedIndex = (focusedIndex + 1) % buttons.Count;
            }

            focusedButton = buttons[focusedIndex];
            focusedButton.Focus();
            Debug.Log($"Navigated to: {focusedButton.name}");
        }

        private void OnSelect(InputAction.CallbackContext context)
        {
            if (focusedButton != null)
            {
                var button = focusedButton.Q<Button>();
                if (button != null)
                {
                    using (var clickEvent = ClickEvent.GetPooled())
                    {
                        clickEvent.target = button;
                        button.SendEvent(clickEvent);
                    }
                    Debug.Log($"Selected: {focusedButton.name}");
                }
            }
        }
    }
}