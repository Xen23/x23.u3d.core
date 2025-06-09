using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;

namespace Xen23.Core
{
    [CreateAssetMenu(fileName = "NewMenuPanel", menuName = "Xen23/Menu Panel")]
    public class MenuPanel : XenScriptableBase
    {
        [SerializeField] private string panelId = "panel_default";
        [SerializeField] private string panelTitle = "Panel";
        [SerializeField] private MenuTranslation panelTitleTranslations = new MenuTranslation();
        [SerializeField] private Vector2 panelPosition = Vector2.zero;
        [SerializeField] private Vector2 panelSize = new Vector2(400, 300);
        [SerializeField] private List<MenuItem> items = new List<MenuItem>();
        [SerializeField] private Sprite panelBackground;

        public string PanelId => panelId;
        public string PanelTitle => panelTitle;
        public MenuTranslation PanelTitleTranslations => panelTitleTranslations;
        public Vector2 PanelPosition => panelPosition;
        public Vector2 PanelSize => panelSize;
        public List<MenuItem> Items => items;
        public Sprite PanelBackground => panelBackground;

        public override void SaveToDisk()
        {
            try
            {
                var data = new MenuPanelData(this);
                string json = JsonUtility.ToJson(data, true);
                string filePath = Path.Combine(GetSavePath(), $"{name}.json");
                File.WriteAllText(filePath, json);
                if (Xen23ConfigSO.Instance?.enableVerboseLogging ?? false)
                    Debug.Log($"Saved {name} to {filePath}");

                foreach (var item in items)
                    item.SaveToDisk();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save {name}: {ex.Message}");
            }
        }

        public override void LoadFromDisk()
        {
            string filePath = Path.Combine(GetSavePath(), $"{name}.json");
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    var data = JsonUtility.FromJson<MenuPanelData>(json);
                    if (data.version == Version)
                    {
                        panelId = data.panelId;
                        panelTitle = data.panelTitle;
                        panelTitleTranslations = data.panelTitleTranslations;
                        panelPosition = data.panelPosition;
                        panelSize = data.panelSize;
                        panelBackground = AssetDatabase.LoadAssetAtPath<Sprite>(data.panelBackgroundPath);
                        if (Xen23ConfigSO.Instance?.enableVerboseLogging ?? false)
                            Debug.Log($"Loaded {name} from {filePath}");
                    }
                    else
                    {
                        Debug.LogWarning($"Version mismatch for {name}. Expected {Version}, got {data.version}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to load {name}: {ex.Message}");
                }
            }
        }
    }
}