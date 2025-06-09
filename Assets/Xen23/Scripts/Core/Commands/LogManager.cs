using UnityEngine;

namespace Xen23.Core
{
    public class LogManager : BaseManager, ILoggable
    {
        private void Awake()
        {
            Initialize();
        }

        public override void Initialize()
        {
            if (Config == null)
            {
                Debug.LogError("LogManager failed to initialize: Xen23ConfigSO not found.");
                return;
            }
            Log("LogManager initialized.");
        }

        public void LogMessage(string message)
        {
            Log(message); // Uses the base class's Log method
        }

        public void LogError(string error)
        {
            if (Config != null)
            {
                Debug.LogError($"[XenTek] Error: {error}");
            }
        }
    }
}