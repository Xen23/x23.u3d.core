using UnityEngine;

namespace XenTek.Core
{
    /// <summary>
    /// Runtime ScriptableObject for XenTek framework configuration.
    /// Stores settings accessible at runtime, such as logging and server details.
    /// </summary>
    [CreateAssetMenu(fileName = "XenTekConfig", menuName = "XenTek/Config", order = 2)]
    public class XenTekConfigSO : ScriptableObject
    {
        private static XenTekConfigSO instance;
        public static XenTekConfigSO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<XenTekConfigSO>("XenTekConfig");
                    if (instance == null)
                    {
                        Debug.LogError("XenTekConfig not found in Resources. Create one via Assets > Create > XenTek > Config.");
                    }
                }
                return instance;
            }
        }

        [Header("Runtime Settings")]
        [Tooltip("Enable verbose logging for debugging XenTek framework at runtime")]
        public bool enableVerboseLogging = false;

        [Header("Backend Settings")]
        [Tooltip("URL of the XenTek master server (e.g., http://yourserver.com:8080)")]
        public string masterServerUrl = "http://localhost:8080";

        [Tooltip("API key for authenticating with the XenTek master server")]
        public string masterServerApiKey = "";
    }
}