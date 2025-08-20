using UnityEngine;
using UnityEditor;

namespace UIFramework.Editor
{
    [CustomEditor(typeof(UIResourceManager))]
    public class UIResourceManagerEditor : UnityEditor.Editor
    {
        private UIResourceManager resourceManager;
        private bool showCollectionIcons = true;
        private bool showUIBackgrounds = true;
        private bool showUIIcons = true;
        private bool showAnimationPrefabs = true;
        private bool showAudioClips = true;

        private void OnEnable()
        {
            resourceManager = (UIResourceManager)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("UI Resource Manager", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Configure UI resources and icons that can be customized at runtime.", MessageType.Info);
            EditorGUILayout.Space();

            // 工具按钮
            DrawToolButtons();
            EditorGUILayout.Space();

            // 收集动画图标
            showCollectionIcons = EditorGUILayout.Foldout(showCollectionIcons, "Collection Animation Icons", true);
            if (showCollectionIcons)
            {
                EditorGUI.indentLevel++;
                DrawCollectionIcons();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            // UI背景图片
            showUIBackgrounds = EditorGUILayout.Foldout(showUIBackgrounds, "UI Background Images", true);
            if (showUIBackgrounds)
            {
                EditorGUI.indentLevel++;
                DrawUIBackgrounds();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            // UI图标
            showUIIcons = EditorGUILayout.Foldout(showUIIcons, "UI Icons", true);
            if (showUIIcons)
            {
                EditorGUI.indentLevel++;
                DrawUIIcons();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            // 动画预制体
            showAnimationPrefabs = EditorGUILayout.Foldout(showAnimationPrefabs, "Animation Prefabs", true);
            if (showAnimationPrefabs)
            {
                EditorGUI.indentLevel++;
                DrawAnimationPrefabs();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            // 音频剪辑
            showAudioClips = EditorGUILayout.Foldout(showAudioClips, "Audio Clips", true);
            if (showAudioClips)
            {
                EditorGUI.indentLevel++;
                DrawAudioClips();
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawToolButtons()
        {
            EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Set as Current Instance"))
            {
                UIResourceManager.Instance = resourceManager;
                Debug.Log("UIResourceManager instance updated.");
            }
            if (GUILayout.Button("Create in Resources"))
            {
                CreateInResourcesFolder();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load Default Icons"))
            {
                LoadDefaultIcons();
            }
            if (GUILayout.Button("Clear All Resources"))
            {
                ClearAllResources();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawCollectionIcons()
        {
            resourceManager.goldIcon = (Sprite)EditorGUILayout.ObjectField("Gold Icon", resourceManager.goldIcon, typeof(Sprite), false);
            resourceManager.diamondIcon = (Sprite)EditorGUILayout.ObjectField("Diamond Icon", resourceManager.diamondIcon, typeof(Sprite), false);
            resourceManager.moneyIcon = (Sprite)EditorGUILayout.ObjectField("Money Icon", resourceManager.moneyIcon, typeof(Sprite), false);
        }

        private void DrawUIBackgrounds()
        {
            resourceManager.defaultBackground = (Sprite)EditorGUILayout.ObjectField("Default Background", resourceManager.defaultBackground, typeof(Sprite), false);
            resourceManager.popupBackground = (Sprite)EditorGUILayout.ObjectField("Popup Background", resourceManager.popupBackground, typeof(Sprite), false);
            resourceManager.buttonBackground = (Sprite)EditorGUILayout.ObjectField("Button Background", resourceManager.buttonBackground, typeof(Sprite), false);
        }

        private void DrawUIIcons()
        {
            resourceManager.confirmIcon = (Sprite)EditorGUILayout.ObjectField("Confirm Icon", resourceManager.confirmIcon, typeof(Sprite), false);
            resourceManager.cancelIcon = (Sprite)EditorGUILayout.ObjectField("Cancel Icon", resourceManager.cancelIcon, typeof(Sprite), false);
            resourceManager.closeIcon = (Sprite)EditorGUILayout.ObjectField("Close Icon", resourceManager.closeIcon, typeof(Sprite), false);
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Status Icons", EditorStyles.miniBoldLabel);
            resourceManager.warningIcon = (Sprite)EditorGUILayout.ObjectField("Warning Icon", resourceManager.warningIcon, typeof(Sprite), false);
            resourceManager.errorIcon = (Sprite)EditorGUILayout.ObjectField("Error Icon", resourceManager.errorIcon, typeof(Sprite), false);
            resourceManager.successIcon = (Sprite)EditorGUILayout.ObjectField("Success Icon", resourceManager.successIcon, typeof(Sprite), false);
            resourceManager.infoIcon = (Sprite)EditorGUILayout.ObjectField("Info Icon", resourceManager.infoIcon, typeof(Sprite), false);
        }

        private void DrawAnimationPrefabs()
        {
            resourceManager.addTextAnimationPrefab = (GameObject)EditorGUILayout.ObjectField("Add Text Animation", resourceManager.addTextAnimationPrefab, typeof(GameObject), false);
            resourceManager.tipPrefab = (GameObject)EditorGUILayout.ObjectField("Tip Prefab", resourceManager.tipPrefab, typeof(GameObject), false);
            resourceManager.goldCollectionPrefab = (GameObject)EditorGUILayout.ObjectField("Gold Collection", resourceManager.goldCollectionPrefab, typeof(GameObject), false);
            resourceManager.diamondCollectionPrefab = (GameObject)EditorGUILayout.ObjectField("Diamond Collection", resourceManager.diamondCollectionPrefab, typeof(GameObject), false);
            resourceManager.moneyCollectionPrefab = (GameObject)EditorGUILayout.ObjectField("Money Collection", resourceManager.moneyCollectionPrefab, typeof(GameObject), false);
        }

        private void DrawAudioClips()
        {
            resourceManager.collectSound = (AudioClip)EditorGUILayout.ObjectField("Collect Sound", resourceManager.collectSound, typeof(AudioClip), false);
            resourceManager.buttonClickSound = (AudioClip)EditorGUILayout.ObjectField("Button Click", resourceManager.buttonClickSound, typeof(AudioClip), false);
            resourceManager.successSound = (AudioClip)EditorGUILayout.ObjectField("Success Sound", resourceManager.successSound, typeof(AudioClip), false);
            resourceManager.errorSound = (AudioClip)EditorGUILayout.ObjectField("Error Sound", resourceManager.errorSound, typeof(AudioClip), false);
        }

        private void CreateInResourcesFolder()
        {
            string resourcesPath = "Assets/Resources";
            if (!AssetDatabase.IsValidFolder(resourcesPath))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            string assetPath = resourcesPath + "/UIResourceManager.asset";
            if (AssetDatabase.LoadAssetAtPath<UIResourceManager>(assetPath) != null)
            {
                if (!EditorUtility.DisplayDialog("File Exists", "UIResourceManager.asset already exists in Resources folder. Overwrite?", "Yes", "No"))
                {
                    return;
                }
            }

            AssetDatabase.CreateAsset(Instantiate(resourceManager), assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"UIResourceManager created at: {assetPath}");
        }

        private void LoadDefaultIcons()
        {
            // 尝试从项目中查找常用的图标
            string[] guids = AssetDatabase.FindAssets("t:Sprite");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(path).ToLower();
                
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                if (sprite == null) continue;

                // 根据文件名自动分配图标
                if (fileName.Contains("gold") && resourceManager.goldIcon == null)
                    resourceManager.goldIcon = sprite;
                else if (fileName.Contains("diamond") && resourceManager.diamondIcon == null)
                    resourceManager.diamondIcon = sprite;
                else if (fileName.Contains("money") && resourceManager.moneyIcon == null)
                    resourceManager.moneyIcon = sprite;
                else if (fileName.Contains("confirm") && resourceManager.confirmIcon == null)
                    resourceManager.confirmIcon = sprite;
                else if (fileName.Contains("cancel") && resourceManager.cancelIcon == null)
                    resourceManager.cancelIcon = sprite;
                else if (fileName.Contains("close") && resourceManager.closeIcon == null)
                    resourceManager.closeIcon = sprite;
            }
            
            EditorUtility.SetDirty(resourceManager);
            Debug.Log("Attempted to load default icons based on file names.");
        }

        private void ClearAllResources()
        {
            if (EditorUtility.DisplayDialog("Clear Resources", "Are you sure you want to clear all resource references?", "Yes", "No"))
            {
                // 清空所有引用
                var fields = typeof(UIResourceManager).GetFields();
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(Sprite) || field.FieldType == typeof(GameObject) || field.FieldType == typeof(AudioClip))
                    {
                        field.SetValue(resourceManager, null);
                    }
                }
                
                EditorUtility.SetDirty(resourceManager);
                Debug.Log("All resource references cleared.");
            }
        }
    }
}
