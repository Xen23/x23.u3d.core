using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEditor;

namespace Xen23.Core
{
    [CreateAssetMenu(fileName = "NewMenuContent", menuName = "Xen23/Menu Content")]
    public class MenuContent : XenScriptableBase
    {
        [Header("Menu Metadata")]
        [SerializeField] private string menuId = "menu_default";
        [SerializeField] private string menuTitle = "Main Menu";
        [SerializeField] private MenuTranslation titleTranslations = new MenuTranslation();

        [Header("Menu Structure")]
        [SerializeField] private List<MenuPanel> panels = new List<MenuPanel>();

        [Header("UI Resources")]
        [SerializeField, TextArea(5, 10)] private string uiXmlLayout = "<!-- UI XML Layout -->";
        [SerializeField] private Sprite backgroundSprite;
        [SerializeField] private Texture2D backgroundTexture;

        [Header("Audio")]
        [SerializeField] private AudioClip menuBgm;
        [SerializeField] private AudioClip selectSfx;

        [System.Serializable]
        private class MenuContentData
        {
            public int version;
            public string menuId;
            public string menuTitle;
            public MenuTranslation titleTranslations;
            public List<MenuPanelData> panels;
            public string uiXmlLayout;
            public string backgroundSpritePath;
            public string backgroundTexturePath;
            public string menuBgmPath;
            public string selectSfxPath;
        }

        public string MenuId => menuId;
        public string MenuTitle => menuTitle;
        public MenuTranslation TitleTranslations => titleTranslations;
        public List<MenuPanel> Panels => panels;
        public string UiXmlLayout => uiXmlLayout;
        public Sprite BackgroundSprite => backgroundSprite;
        public Texture2D BackgroundTexture => backgroundTexture;
        public AudioClip MenuBgm => menuBgm;
        public AudioClip SelectSfx => selectSfx;

        public override void SaveToDisk()
        {
            try
            {
                var data = new MenuContentData
                {
                    version = Version,
                    menuId = menuId,
                    menuTitle = menuTitle,
                    titleTranslations = titleTranslations,
                    panels = panels.ConvertAll(p => new MenuPanelData(p)),
                    uiXmlLayout = uiXmlLayout,
                    backgroundSpritePath = AssetDatabase.GetAssetPath(backgroundSprite),
                    backgroundTexturePath = AssetDatabase.GetAssetPath(backgroundTexture),
                    menuBgmPath = AssetDatabase.GetAssetPath(menuBgm),
                    selectSfxPath = AssetDatabase.GetAssetPath(selectSfx)
                };
                string json = JsonUtility.ToJson(data, true);
                string filePath = Path.Combine(GetSavePath(), $"{name}.json");
                File.WriteAllText(filePath, json);
                if (Xen23ConfigSO.Instance?.enableVerboseLogging ?? false)
                    Debug.Log($"Saved {name} to {filePath}");

                foreach (var panel in panels)
                    panel.SaveToDisk();
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
                    var data = JsonUtility.FromJson<MenuContentData>(json);
                    if (data.version == Version)
                    {
                        menuId = data.menuId;
                        menuTitle = data.menuTitle;
                        titleTranslations = data.titleTranslations;
                        uiXmlLayout = data.uiXmlLayout;
                        backgroundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(data.backgroundSpritePath);
                        backgroundTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(data.backgroundTexturePath);
                        menuBgm = AssetDatabase.LoadAssetAtPath<AudioClip>(data.menuBgmPath);
                        selectSfx = AssetDatabase.LoadAssetAtPath<AudioClip>(data.selectSfxPath);
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