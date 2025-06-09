using UnityEngine;

namespace XenTek.Core
{
    public abstract class BaseServer : BaseManager
    {
        public abstract void StartServer();
        public abstract void StopServer();
    }
}