using UnityEngine;
using UnityEditor;

namespace XenTek.Editor
{
    /// <summary>
    /// ScriptableObject to store editor configuration data for the XenTek framework.
    /// Used to manage project settings, build targets, and backend server details.
    /// </summary>
    [CreateAssetMenu(fileName = "XenTekUnityConfigData", menuName = "XenTek/Unity Config Data", order = 1)]
    public class XenTekUnityEditorWindowMainDataSO : ScriptableObject
    {
        // Singleton-like access to the settings instance
        private static XenTekUnityEditorWindowMainDataSO instance;
        public static XenTekUnityEditorWindowMainDataSO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<XenTekUnityEditorWindowMainDataSO>("XenTekUnityConfigData");
                    if (instance == null)
                    {
                        Debug.LogError("XenTekUnityConfigData not found in Resources. Create one via Assets > Create > XenTek > Unity Config Data.");
                    }
                }
                return instance;
            }
        }

        [Header("Project Settings")]
        [Tooltip("Name of the active project (e.g., Arcade)")]
        public string projectName = "Arcade";

        [Tooltip("Version of the project")]
        public string projectVersion = "0.1.0";

        [Header("Build Settings")]
        [Tooltip("Primary target platform for the project")]
        public BuildTarget primaryTargetPlatform = BuildTarget.Android;

        [Tooltip("Output directory for builds, relative to project root")]
        public string buildOutputPath = "Builds/";

        [Header("Backend Settings")]
        [Tooltip("URL of the XenTek master server (e.g., http://yourserver.com:8080)")]
        public string masterServerUrl = "http://localhost:8080";

        [Tooltip("API key for authenticating with the XenTek master server")]
        public string masterServerApiKey = "";

        [Header("Editor Preferences")]
        [Tooltip("Enable verbose logging in the editor for debugging XenTek framework")]
        public bool enableVerboseLogging = false;

        [Tooltip("Default scene to load in the editor")]
        public string defaultEditorScene = "Assets/Arcade/Editor/Scenes/ArcadeEditor.unity";

        /// <summary>
        /// Resets all settings to their default values.
        /// </summary>
        public void ResetToDefaults()
        {
            projectName = "Arcade";
            projectVersion = "0.1.0";
            primaryTargetPlatform = BuildTarget.Android;
            buildOutputPath = "Builds/";
            masterServerUrl = "http://localhost:8080";
            masterServerApiKey = "";
            enableVerboseLogging = false;
            defaultEditorScene = "Assets/Arcade/Editor/Scenes/ArcadeEditor.unity";
        }

        // Validation for settings
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(projectName))
            {
                Debug.LogWarning("Project name is empty in XenTekUnityEditorWindowMainDataSO.");
            }
            if (string.IsNullOrEmpty(masterServerUrl))
            {
                Debug.LogWarning("Master server URL is empty in XenTekUnityEditorWindowMainDataSO.");
            }
        }
    }
}