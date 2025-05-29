using UnityEngine;
using UnityEngine.InputSystem;
using XenTek.Core.Commands;

namespace XenTek.Arcade
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private float moveSpeed = 5f;

        private CommandManager commandManager;
        private InputAction moveAction;
        private InputAction undoAction;

        private void Awake()
        {
            commandManager = GetComponent<CommandManager>();
            if (commandManager == null)
            {
                Debug.LogError("CommandManager component missing on PlayerController GameObject.");
            }
        }

        private void OnEnable()
        {
            if (inputActions != null)
            {
                moveAction = inputActions.FindAction("Move");
                undoAction = inputActions.FindAction("Undo");
                moveAction?.Enable();
                undoAction?.Enable();
                moveAction.performed += OnMove;
                undoAction.performed += OnUndo;
            }
        }

        private void OnDisable()
        {
            moveAction?.Disable();
            undoAction?.Disable();
            moveAction.performed -= OnMove;
            undoAction.performed -= OnUndo;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(input.x, 0, input.y).normalized;
            Vector3 newPosition = transform.position + moveDirection * moveSpeed;

            commandManager.ExecuteCommand(new MovePlayerCommand(transform, newPosition));
        }

        private void OnUndo(InputAction.CallbackContext context)
        {
            commandManager.UndoLastCommand();
        }
    }
}