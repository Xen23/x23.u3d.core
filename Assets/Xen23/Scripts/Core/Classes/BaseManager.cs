using UnityEngine;

namespace Xen23.Core
{
    public abstract class BaseManager : MonoBehaviour
    {
        protected Xen23ConfigSO Config => Xen23ConfigSO.Instance;

        // Abstract method to initialize the manager
        public abstract void Initialize();
        public bool IsInitialized;

        // Common method for all managers
        protected void Log(string message)
        {
            if (Config != null && Config.enableVerboseLogging)
            {
                Debug.Log($"[XenTek] {message}");
            }
        }
    }
    
}