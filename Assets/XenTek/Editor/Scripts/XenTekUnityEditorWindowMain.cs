using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace XenTek.Editor
{
    /// <summary>
    /// Custom Editor Window for managing XenTek framework settings using UXML/USS.
    /// Allows editing of XenTekUnityEditorWindowMainDataSO via a UIToolkit interface.
    /// </summary>
    public class XenTekUnityEditorWindowMain : EditorWindow
    {
        [SerializeField] private VisualTreeAsset uxmlAsset; // Assign XenTekUnityEditorWindowMain.uxml
        [SerializeField] private StyleSheet ussAsset; // Assign XenTekUnityEditorWindowMain.uss

        private XenTekUnityEditorWindowMainDataSO settings;
        private SerializedObject serializedObject;

        [MenuItem("XenTek/Main Window", false, 10)]
        public static void ShowWindow()
        {
            var window = GetWindow<XenTekUnityEditorWindowMain>("XenTek Main Window");
            window.minSize = new Vector2(400, 300);
        }

        private void OnEnable()
        {
            // Initialize settings
            settings = XenTekUnityEditorWindowMainDataSO.Instance;
            if (settings != null)
            {
                serializedObject = new SerializedObject(settings);
            }
        }

        private void CreateGUI()
        {
            if (settings == null)
            {
                rootVisualElement.Add(new Label("Error: XenTekUnityConfigData not found in Assets/XenTek/Editor/Resources/. Create one via Assets > Create > XenTek > Unity Config Data."));
                Button createButton = new Button(() =>
                {
                    var asset = ScriptableObject.CreateInstance<XenTekUnityEditorWindowMainDataSO>();
                    AssetDatabase.CreateAsset(asset, "Assets/XenTek/Editor/Resources/XenTekUnityConfigData.asset");
                    AssetDatabase.SaveAssets();
                    Debug.Log("Created XenTekUnityConfigData.asset in Assets/XenTek/Resources/");
                    settings = asset;
                    serializedObject = new SerializedObject(settings);
                    rootVisualElement.Clear();
                    CreateGUI(); // Reload UI
                })
                {
                    text = "Create Config Data"
                };
                rootVisualElement.Add(createButton);
                return;
            }

            // Load UXML and USS
            if (uxmlAsset != null)
            {
                uxmlAsset.CloneTree(rootVisualElement);
            }
            else
            {
                Debug.LogError("XenTekUnityEditorWindowMain.uxml not assigned in XenTekUnityEditorWindowMain.cs.");
                rootVisualElement.Add(new Label("Error: XenTekUnityEditorWindowMain.uxml not assigned."));
                return;
            }

            if (ussAsset != null)
            {
                rootVisualElement.styleSheets.Add(ussAsset);
            }

            // Bind fields
            var root = rootVisualElement;
            root.Q<TextField>("project-name")?.BindProperty(serializedObject.FindProperty("projectName"));
            root.Q<TextField>("project-version")?.BindProperty(serializedObject.FindProperty("projectVersion"));
            root.Q<EnumField>("primary-target-platform")?.BindProperty(serializedObject.FindProperty("primaryTargetPlatform"));
            root.Q<TextField>("build-output-path")?.BindProperty(serializedObject.FindProperty("buildOutputPath"));
            root.Q<TextField>("master-server-url")?.BindProperty(serializedObject.FindProperty("masterServerUrl"));
            root.Q<TextField>("master-server-api-key")?.BindProperty(serializedObject.FindProperty("masterServerApiKey"));
            root.Q<Toggle>("enable-verbose-logging")?.BindProperty(serializedObject.FindProperty("enableVerboseLogging"));
            root.Q<TextField>("default-editor-scene")?.BindProperty(serializedObject.FindProperty("defaultEditorScene"));

            // Setup buttons
            root.Q<Button>("reset-button")?.RegisterCallback<ClickEvent>(evt =>
            {
                settings.ResetToDefaults();
                serializedObject.Update();
            });

            root.Q<Button>("save-button")?.RegisterCallback<ClickEvent>(evt =>
            {
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
                Debug.Log("XenTekUnityConfigData saved.");
            });
        }
    }
}