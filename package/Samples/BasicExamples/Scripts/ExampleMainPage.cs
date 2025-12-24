using UnityEngine;
using UnityEngine.UI;
using UIFramework;

namespace UIFramework.Examples
{
    /// <summary>
    /// 示例主页面
    /// </summary>
    public class ExampleMainPage : Page
    {
        [Header("Main Page Elements")]
        [SerializeField] private Text titleText;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button inventoryButton;
        [SerializeField] private Button showPopupButton;

        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Start()
        {
            // 绑定按钮事件
            if (settingsButton != null)
                settingsButton.onClick.AddListener(() => UIManager.Instance.ShowPage<ExampleSettingsPage>());

            if (inventoryButton != null)
                inventoryButton.onClick.AddListener(() => UIManager.Instance.ShowPage<ExampleInventoryPage>());

            if (showPopupButton != null)
                showPopupButton.onClick.AddListener(() => UIManager.Instance.ShowPopup<ExampleConfirmPopup>("确认", "这是一个示例弹窗"));
        }

        protected override void OnAfterShow(params object[] data)
        {
            base.OnAfterShow(data);

            // 更新标题
            if (titleText != null)
            {
                string title = "主页面";
                if (data.Length > 0 && data[0] is string customTitle)
                {
                    title = customTitle;
                }
                titleText.text = title;
            }

            Debug.Log($"MainPage shown with {data?.Length ?? 0} parameters");
        }
    }
}
