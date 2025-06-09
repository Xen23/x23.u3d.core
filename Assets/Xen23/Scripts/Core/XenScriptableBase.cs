using UnityEngine;
using System;
using System.IO;

namespace Xen23.Core
{
    public abstract class XenScriptableBase : ScriptableObject
    {
        [Header("File Settings")]
        [SerializeField] private int version = 1;
        [SerializeField] private bool useCustomSavePath = false;
        [SerializeField] private string customSavePath = "Assets/Resources/MenuData";
        [SerializeField] private bool autoSaveOnChange = true;

        public int Version => version;
        private string DefaultSavePath => Path.Combine(Application.persistentDataPath, "MenuData");

        protected string GetSavePath()
        {
            string path = useCustomSavePath ? customSavePath : Xen23ConfigSO.Instance?.customSavePath ?? DefaultSavePath;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        public virtual void SaveToDisk()
        {
            try
            {
                string json = JsonUtility.ToJson(this, true);
                string filePath = Path.Combine(GetSavePath(), $"{name}.json");
                File.WriteAllText(filePath, json);
                if (Xen23ConfigSO.Instance?.enableVerboseLogging ?? false)
                    Debug.Log($"Saved {name} to {filePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save {name}: {ex.Message}");
            }
        }

        public virtual void LoadFromDisk()
        {
            string filePath = Path.Combine(GetSavePath(), $"{name}.json");
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    JsonUtility.FromJsonOverwrite(json, this);
                    if (Xen23ConfigSO.Instance?.enableVerboseLogging ?? false)
                        Debug.Log($"Loaded {name} from {filePath}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to load {name}: {ex.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"No save file found for {name} at {filePath}");
            }
        }

        public string FindFileLocation()
        {
            string path = Path.Combine(GetSavePath(), $"{name}.json");
            return File.Exists(path) ? path : "File not found";
        }

        private void OnValidate()
        {
            if (autoSaveOnChange && Application.isEditor)
            {
                SaveToDisk();
            }
        }
    }
}