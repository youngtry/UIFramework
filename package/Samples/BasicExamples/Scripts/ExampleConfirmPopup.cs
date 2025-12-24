using UnityEngine;
using UnityEngine.UI;
using System;
using UIFramework;

namespace UIFramework.Examples
{
    /// <summary>
    /// 示例确认弹窗
    /// </summary>
    public class ExampleConfirmPopup : Popup
    {
        [Header("Confirm Popup Elements")]
        [SerializeField] private Text titleText;
        [SerializeField] private Text messageText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button closeButton;

        private Action onConfirm;
        private Action onCancel;

        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Start()
        {
            // 绑定按钮事件
            if (confirmButton != null)
                confirmButton.onClick.AddListener(OnConfirmClick);

            if (cancelButton != null)
                cancelButton.onClick.AddListener(OnCancelClick);

            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClick);
        }

        protected override void OnAfterShow(params object[] data)
        {
            base.OnAfterShow(data);

            // 处理传入的数据
            if (data != null && data.Length > 0)
            {
                // 第一个参数：标题
                if (data.Length > 0 && data[0] is string title)
                {
                    SetTitle(title);
                }

                // 第二个参数：消息内容
                if (data.Length > 1 && data[1] is string message)
                {
                    SetMessage(message);
                }

                // 第三个参数：确认回调
                if (data.Length > 2 && data[2] is Action confirmCallback)
                {
                    onConfirm = confirmCallback;
                }

                // 第四个参数：取消回调
                if (data.Length > 3 && data[3] is Action cancelCallback)
                {
                    onCancel = cancelCallback;
                }
            }

            Debug.Log($"ConfirmPopup shown with {data?.Length ?? 0} parameters");
        }

        /// <summary>
        /// 设置标题
        /// </summary>
        public void SetTitle(string title)
        {
            if (titleText != null)
            {
                titleText.text = title;
            }
        }

        /// <summary>
        /// 设置消息内容
        /// </summary>
        public void SetMessage(string message)
        {
            if (messageText != null)
            {
                messageText.text = message;
            }
        }

        /// <summary>
        /// 设置确认回调
        /// </summary>
        public void SetConfirmCallback(Action callback)
        {
            onConfirm = callback;
        }

        /// <summary>
        /// 设置取消回调
        /// </summary>
        public void SetCancelCallback(Action callback)
        {
            onCancel = callback;
        }

        private void OnConfirmClick()
        {
            onConfirm?.Invoke();
            Close();
        }

        private void OnCancelClick()
        {
            onCancel?.Invoke();
            Close();
        }

        private void OnCloseClick()
        {
            Close();
        }

        protected override void OnBackgroundClick()
        {
            // 重写背景点击行为，确认弹窗不应该通过点击背景关闭
            // 可以根据需要修改这个行为
            if (closeOnBackgroundClick)
            {
                OnCancelClick();
            }
        }

        /// <summary>
        /// 静态方法：显示确认弹窗
        /// </summary>
        public static void Show(string title, string message, Action onConfirm = null, Action onCancel = null)
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup<ExampleConfirmPopup>(title, message, onConfirm, onCancel);
            }
        }
    }
}
