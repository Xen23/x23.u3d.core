using UnityEngine;
using System.Collections.Generic;

namespace Xen23.Core
{
    [System.Serializable]
    public class MenuTranslation
    {
        [SerializeField] private List<TranslationEntry> translations = new List<TranslationEntry>();

        public string GetTranslation(string languageCode)
        {
            var entry = translations.Find(t => t.languageCode == languageCode);
            return entry != null ? entry.translatedText : "";
        }
    }

    [System.Serializable]
    public class TranslationEntry
    {
        [SerializeField] public string languageCode = "en";
        [SerializeField] public string translatedText = "";
    }
}