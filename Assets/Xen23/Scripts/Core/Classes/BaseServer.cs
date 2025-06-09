using UnityEngine;

namespace Xen23.Core
{
    public abstract class BaseServer : BaseManager
    {
        public abstract void StartServer();
        public abstract void StopServer();
    }
}