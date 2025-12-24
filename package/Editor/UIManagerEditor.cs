using UnityEngine;
using UnityEditor;

namespace UIFramework.Editor
{
    [CustomEditor(typeof(UIManager))]
    public class UIManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            EditorGUILayout.Space();
            
            UIManager uiManager = (UIManager)target;
            
            // 验证按钮
            if (GUILayout.Button("Validate Setup"))
            {
                ValidateSetup(uiManager);
            }
            
            EditorGUILayout.Space();
            
            // 显示帮助信息
            EditorGUILayout.HelpBox(
                "设置说明：\n" +
                "1. 分配UI Canvas\n" +
                "2. 将Page类型的页面拖拽到Pages数组\n" +
                "3. 将Popup类型的弹窗拖拽到Popups数组\n" +
                "4. 点击'Validate Setup'验证设置",
                MessageType.Info
            );
        }
        
        private void ValidateSetup(UIManager uiManager)
        {
            bool hasErrors = false;
            
            // 验证UI Canvas
            if (uiManager.GetComponent<UIManager>().GetType().GetField("uiCanvas", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(uiManager) == null)
            {
                Debug.LogError("UI Canvas is not assigned!");
                hasErrors = true;
            }
            
            // 验证Pages数组
            var pagesField = uiManager.GetType().GetField("pages", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var pages = pagesField?.GetValue(uiManager) as BasePage[];
            
            if (pages != null)
            {
                for (int i = 0; i < pages.Length; i++)
                {
                    if (pages[i] != null && !(pages[i] is Page))
                    {
                        Debug.LogError($"Pages[{i}] ({pages[i].name}) is not a Page type!");
                        hasErrors = true;
                    }
                }
            }
            
            // 验证Popups数组
            var popupsField = uiManager.GetType().GetField("popups", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var popups = popupsField?.GetValue(uiManager) as BasePage[];
            
            if (popups != null)
            {
                for (int i = 0; i < popups.Length; i++)
                {
                    if (popups[i] != null && !(popups[i] is Popup))
                    {
                        Debug.LogError($"Popups[{i}] ({popups[i].name}) is not a Popup type!");
                        hasErrors = true;
                    }
                }
            }
            
            if (!hasErrors)
            {
                Debug.Log("UIManager setup validation passed!");
            }
        }
    }
}
