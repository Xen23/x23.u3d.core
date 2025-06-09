using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
namespace Xen23.Core
{
    [System.Serializable]
    public class MenuPanelData
    {
        public int version;
        public string panelId;
        public string panelTitle;
        public MenuTranslation panelTitleTranslations;
        public Vector2 panelPosition;
        public Vector2 panelSize;
        public List<MenuItemData> items;
        public string panelBackgroundPath;

        public MenuPanelData(MenuPanel panel)
        {
            version = panel.Version;
            panelId = panel.PanelId;
            panelTitle = panel.PanelTitle;
            panelTitleTranslations = panel.PanelTitleTranslations;
            panelPosition = panel.PanelPosition;
            panelSize = panel.PanelSize;
            items = panel.Items.ConvertAll(i => new MenuItemData(i));
            panelBackgroundPath = AssetDatabase.GetAssetPath(panel.PanelBackground);
        }
    }
}