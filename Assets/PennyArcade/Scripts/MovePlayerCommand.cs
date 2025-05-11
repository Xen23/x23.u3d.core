using UnityEngine;
using XenTek.Core.Commands;

namespace PennyArcade
{
    public class MovePlayerCommand : ICommand
    {
        private readonly Transform player;
        private readonly Vector3 fromPosition;
        private readonly Vector3 toPosition;

        public MovePlayerCommand(Transform player, Vector3 toPosition)
        {
            this.player = player;
            this.fromPosition = player.position;
            this.toPosition = toPosition;
        }

        public void Execute() => player.position = toPosition;
        public void Undo() => player.position = fromPosition;
    }
}