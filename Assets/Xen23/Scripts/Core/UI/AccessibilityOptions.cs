using UnityEngine;

namespace Xen23.Core
{
    [System.Serializable]
    public class AccessibilityOptions
    {
        [SerializeField] private bool highContrastMode = false;
        [SerializeField] private float textSizeMultiplier = 1f;
        [SerializeField] private bool screenReaderSupport = true;
        [SerializeField] private AudioClip screenReaderAudio;

        public bool HighContrastMode => highContrastMode;
        public float TextSizeMultiplier => textSizeMultiplier;
        public bool ScreenReaderSupport => screenReaderSupport;
        public AudioClip ScreenReaderAudio => screenReaderAudio;
    }
}