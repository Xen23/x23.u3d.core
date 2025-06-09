using UnityEngine;

namespace Xen23.Core
{
    [CreateAssetMenu(fileName = "CoreConfig", menuName = "Xen23/Core Config", order = 2)]
    public class Xen23ConfigSO : ScriptableObject
    {
        private static Xen23ConfigSO instance;
        public static Xen23ConfigSO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<Xen23ConfigSO>("CoreConfig");
                    if (instance == null)
                    {
                        Debug.LogError("Xen23Config not found in Resources. Create one via Assets > Create > Xen23 > Game Core Config.");
                    }
                }
                return instance;
            }
        }

        [Header("Runtime Settings")]
        public bool enableVerboseLogging = false;
        public string customSavePath = "Assets/Resources/MenuData";

        [Header("Backend Settings")]
        public string masterServerUrl = "http://localhost:8080";
        public string masterServerApiKey = "";
    }
}