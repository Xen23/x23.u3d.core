using UnityEngine;
namespace Xen23.Core.Commands
{
    /// <summary>
    /// Interface for all commands in the XenTek framework.
    /// Supports execution and undoing of actions for save/replay functionality.
    /// </summary>
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}