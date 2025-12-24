using UnityEngine;
using UnityEngine.UI;
using UIFramework;

namespace UIFramework.Examples
{
    /// <summary>
    /// MessagePopup使用示例
    /// </summary>
    public class MessagePopupExample : MonoBehaviour
    {
        [Header("Example Buttons")]
        [SerializeField] private Button showMessageButton;
        [SerializeField] private Button showConfirmButton;
        [SerializeField] private Button showCustomButton;

        private void Start()
        {
            // 绑定按钮事件
            if (showMessageButton != null)
                showMessageButton.onClick.AddListener(ShowSimpleMessage);

            if (showConfirmButton != null)
                showConfirmButton.onClick.AddListener(ShowConfirmDialog);

            if (showCustomButton != null)
                showCustomButton.onClick.AddListener(ShowCustomDialog);
        }

        /// <summary>
        /// 显示简单消息
        /// </summary>
        private void ShowSimpleMessage()
        {
            MessagePopup.ShowMessage("这是一个简单的消息提示！");
        }

        /// <summary>
        /// 显示确认对话框
        /// </summary>
        private void ShowConfirmDialog()
        {
            MessagePopup.ShowConfirm(
                "确认操作",
                "您确定要执行此操作吗？",
                () => {
                    Debug.Log("用户点击了确认");
                    MessagePopup.ShowMessage("操作已确认！");
                },
                () => {
                    Debug.Log("用户点击了取消");
                    MessagePopup.ShowMessage("操作已取消！");
                }
            );
        }

        /// <summary>
        /// 显示自定义对话框
        /// </summary>
        private void ShowCustomDialog()
        {
            var data = new MessagePopupData("您想要保存当前进度吗？")
            {
                title = "保存进度",
                confirmButtonText = "保存",
                cancelButtonText = "不保存",
                showCancelButton = true,
                onConfirm = () => {
                    Debug.Log("保存进度");
                    MessagePopup.ShowMessage("进度已保存！");
                },
                onCancel = () => {
                    Debug.Log("不保存进度");
                    MessagePopup.ShowMessage("进度未保存！");
                },
                onClose = () => {
                    Debug.Log("对话框被关闭");
                }
            };

            MessagePopup.ShowCustom(data);
        }

        /// <summary>
        /// 显示带标题的确认对话框
        /// </summary>
        public void ShowTitledConfirm()
        {
            MessagePopup.ShowConfirm(
                "删除文件",
                "此操作将永久删除选中的文件，无法恢复。您确定要继续吗？",
                () => {
                    Debug.Log("文件已删除");
                    MessagePopup.ShowMessage("文件删除成功！");
                },
                () => {
                    Debug.Log("取消删除");
                }
            );
        }

        /// <summary>
        /// 显示警告消息
        /// </summary>
        public void ShowWarningMessage()
        {
            var data = new MessagePopupData("网络连接不稳定，请检查您的网络设置。")
            {
                title = "网络警告",
                confirmButtonText = "重试",
                cancelButtonText = "取消",
                showCancelButton = true,
                onConfirm = () => {
                    Debug.Log("重试连接");
                    MessagePopup.ShowMessage("正在重新连接...");
                },
                onCancel = () => {
                    Debug.Log("取消连接");
                }
            };

            MessagePopup.ShowCustom(data);
        }

        /// <summary>
        /// 显示成功消息
        /// </summary>
        public void ShowSuccessMessage()
        {
            var data = new MessagePopupData("数据同步完成！")
            {
                title = "同步成功",
                confirmButtonText = "好的",
                showCancelButton = false,
                onConfirm = () => {
                    Debug.Log("确认成功消息");
                }
            };

            MessagePopup.ShowCustom(data);
        }
    }
}
