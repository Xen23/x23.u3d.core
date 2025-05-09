using System.Collections.Generic;
using UnityEngine;

namespace XenTek.Core.Commands
{
    /// <summary>
    /// Manages execution and history of commands in the XenTek framework.
    /// Supports undo/redo and saving action history for replay or server sync.
    /// </summary>
    public class CommandManager : MonoBehaviour
    {
        private readonly Stack<ICommand> undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> redoStack = new Stack<ICommand>();

        /// <summary>
        /// Executes a command and adds it to the undo stack.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear(); // Clear redo stack on new action
            if (XenTekConfigSO.Instance != null && XenTekConfigSO.Instance.enableVerboseLogging)
            {
                Debug.Log($"Executed command: {command.GetType().Name}");
            }
        }

        /// <summary>
        /// Undoes the last command and moves it to the redo stack.
        /// </summary>
        public void UndoLastCommand()
        {
            if (undoStack.Count > 0)
            {
                ICommand command = undoStack.Pop();
                command.Undo();
                redoStack.Push(command);
                if (XenTekConfigSO.Instance != null && XenTekConfigSO.Instance.enableVerboseLogging)
                {
                    Debug.Log($"Undid command: {command.GetType().Name}");
                }
            }
        }

        /// <summary>
        /// Redoes the last undone command and moves it back to the undo stack.
        /// </summary>
        public void RedoLastCommand()
        {
            if (redoStack.Count > 0)
            {
                ICommand command = redoStack.Pop();
                command.Execute();
                undoStack.Push(command);
                if (XenTekConfigSO.Instance != null && XenTekConfigSO.Instance.enableVerboseLogging)
                {
                    Debug.Log($"Redid command: {command.GetType().Name}");
                }
            }
        }

        /// <summary>
        /// Clears all command history (e.g., on game reset).
        /// </summary>
        public void ClearHistory()
        {
            undoStack.Clear();
            redoStack.Clear();
        }
    }
}