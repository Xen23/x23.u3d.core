using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace Xen23.Core
{
    [CreateAssetMenu(fileName = "NewMenuItem", menuName = "Xen23/Menu Item")]
    public class MenuItem : XenScriptableBase
    {
        [SerializeField] private string itemId = "item_default";
        [SerializeField] private string itemText = "Item";
        [SerializeField] private MenuTranslation itemTextTranslations = new MenuTranslation();
        [SerializeField] private Sprite itemIcon;
        [SerializeField] private Sprite accessibilityIconOverride;
        [SerializeField] private ItemActionType actionType = ItemActionType.None;
        [SerializeField] private string actionParameter = "";
        [SerializeField] private Vector2 itemPosition = Vector2.zero;
        [SerializeField] private Vector2 itemSize = new Vector2(200, 50);
        [SerializeField] private bool isInteractable = true;
        [SerializeField] private AccessibilityOptions accessibilityOptions = new AccessibilityOptions();

        public string ItemId => itemId;
        public string ItemText => itemText;
        public MenuTranslation ItemTextTranslations => itemTextTranslations;
        public Sprite ItemIcon => itemIcon;
        public Sprite AccessibilityIconOverride => accessibilityIconOverride;
        public ItemActionType ActionType => actionType;
        public string ActionParameter => actionParameter;
        public Vector2 ItemPosition => itemPosition;
        public Vector2 ItemSize => itemSize;
        public bool IsInteractable => isInteractable;
        public AccessibilityOptions AccessibilityOptions => accessibilityOptions;

        public override void SaveToDisk()
        {
            try
            {
                var data = new MenuItemData(this);
                string json = JsonUtility.ToJson(data, true);
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

        public override void LoadFromDisk()
        {
            string filePath = Path.Combine(GetSavePath(), $"{name}.json");
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    var data = JsonUtility.FromJson<MenuItemData>(json);
                    if (data.version == Version)
                    {
                        itemId = data.itemId;
                        itemText = data.itemText;
                        itemTextTranslations = data.itemTextTranslations;
                        itemIcon = AssetDatabase.LoadAssetAtPath<Sprite>(data.itemIconPath);
                        accessibilityIconOverride = AssetDatabase.LoadAssetAtPath<Sprite>(data.accessibilityIconOverridePath);
                        actionType = data.actionType;
                        actionParameter = data.actionParameter;
                        itemPosition = data.itemPosition;
                        itemSize = data.itemSize;
                        isInteractable = data.isInteractable;
                        accessibilityOptions = data.accessibilityOptions;
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