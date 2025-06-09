using UnityEngine;

namespace XenTek.Core
{
    public abstract class BaseManager : MonoBehaviour
    {
        protected XenTekConfigSO Config => XenTekConfigSO.Instance;

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