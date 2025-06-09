using UnityEngine;

namespace XenTek.Core
{
    /// <summary>
    /// Runtime ScriptableObject for XenTek framework configuration.
    /// Stores settings accessible at runtime, such as logging and server details.
    /// </summary>
    [CreateAssetMenu(fileName = "XenTekConfig", menuName = "XenTek/Game Core Config", order = 2)]
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
        public bool enableVerboseLogging = false;

        [Header("Backend Settings")]
        public string masterServerUrl = "http://localhost:8080";
        public string masterServerApiKey = "";
    }

}

