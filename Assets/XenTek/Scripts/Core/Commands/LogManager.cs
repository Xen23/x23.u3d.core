using UnityEngine;

namespace XenTek.Core
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
                Debug.LogError("LogManager failed to initialize: XenTekConfigSO not found.");
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