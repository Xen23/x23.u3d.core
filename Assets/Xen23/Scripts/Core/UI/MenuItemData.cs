using UnityEditor;
using UnityEngine;

namespace Xen23.Core
{
    [System.Serializable]
    public class MenuItemData
    {
        public int version;
        public string itemId;
        public string itemText;
        public MenuTranslation itemTextTranslations;
        public string itemIconPath;
        public string accessibilityIconOverridePath;
        public ItemActionType actionType;
        public string actionParameter;
        public Vector2 itemPosition;
        public Vector2 itemSize;
        public bool isInteractable;
        public AccessibilityOptions accessibilityOptions;

        public MenuItemData(MenuItem item)
        {
            version = item.Version;
            itemId = item.ItemId;
            itemText = item.ItemText;
            itemTextTranslations = item.ItemTextTranslations;
            itemIconPath = AssetDatabase.GetAssetPath(item.ItemIcon);
            accessibilityIconOverridePath = AssetDatabase.GetAssetPath(item.AccessibilityIconOverride);
            actionType = item.ActionType;
            actionParameter = item.ActionParameter;
            itemPosition = item.ItemPosition;
            itemSize = item.ItemSize;
            isInteractable = item.IsInteractable;
            accessibilityOptions = item.AccessibilityOptions;
        }
    }
}