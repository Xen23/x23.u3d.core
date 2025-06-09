using UnityEngine;

namespace Xen23.Arcade.UI
{
    public class UIMenuCommand : Xen23.Core.Commands.ICommand
    {
        private readonly string actionName;
        private readonly System.Action onExecute;
        private readonly System.Action onUndo;

        public UIMenuCommand(string actionName, System.Action onExecute, System.Action onUndo = null)
        {
            this.actionName = actionName;
            this.onExecute = onExecute;
            this.onUndo = onUndo;
        }

        public void Execute()
        {
            onExecute?.Invoke();
            Debug.Log($"UI Action: {actionName}");
        }

        public void Undo()
        {
            onUndo?.Invoke();
            Debug.Log($"Undid UI Action: {actionName}");
        }
    }
}