using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFramework
{
    /// <summary>
    /// 提示框数据类
    /// </summary>
    [System.Serializable]
    public class MessagePopupData
    {
        public string title = "提示";
        public string message = "";
        public string confirmButtonText = "确认";
        public string cancelButtonText = "取消";
        public bool showCancelButton = true;
        public Action onConfirm;
        public Action onCancel;
        public Action onClose;

        public MessagePopupData(string message, Action onConfirm = null, Action onCancel = null)
        {
            this.message = message;
            this.onConfirm = onConfirm;
            this.onCancel = onCancel;
        }

        public MessagePopupData(string title, string message, Action onConfirm = null, Action onCancel = null)
        {
            this.title = title;
            this.message = message;
            this.onConfirm = onConfirm;
            this.onCancel = onCancel;
        }
    }
    public class MessagePopup : Popup
    {
        [Header("UI组件")]
        [SerializeField] private Text titleText;
        [SerializeField] private Text messageText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Text confirmButtonText;
        // [SerializeField] private Text cancelButtonText;
        [SerializeField] private GameObject cancelButtonObject; // 用于控制取消按钮的显示/隐藏

        // 回调函数
        private Action onConfirmCallback;
        private Action onCancelCallback;
        private Action onCloseCallback;

        protected override void Start()
        {
            base.Start();

            // 绑定按钮事件
            if (confirmButton != null)
            {
                confirmButton.onClick.AddListener(OnConfirmClick);
            }

            if (cancelButton != null)
            {
                cancelButton.onClick.AddListener(OnCancelClick);
            }
        }

        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="args">参数数组</param>
        public override void Show(params object[] args)
        {
            base.Show(args);

            if (args.Length > 0)
            {
                ParseArguments(args);
            }
        }

        /// <summary>
        /// 解析传入的参数
        /// </summary>
        /// <param name="args">参数数组</param>
        private void ParseArguments(object[] args)
        {
            // 重置回调
            onConfirmCallback = null;
            onCancelCallback = null;
            onCloseCallback = null;

            foreach (var arg in args)
            {
                if (arg is MessagePopupData data)
                {
                    SetupPopup(data);
                    Utils.ResizeLayout(messageText.gameObject.transform.parent.gameObject);
                    return;
                }
            }


            Utils.ResizeLayout(messageText.gameObject.transform.parent.gameObject);

        }

        /// <summary>
        /// 使用MessagePopupData设置弹窗
        /// </summary>
        /// <param name="data">弹窗数据</param>
        private void SetupPopup(MessagePopupData data)
        {
            // 设置文本
            SetMessage(data.message);
            // SetConfirmButtonText(data.needTranslation ? GetTranslatedText(data.confirmButtonText) : data.confirmButtonText);
            // SetCancelButtonText(data.needTranslation ? GetTranslatedText(data.cancelButtonText) : data.cancelButtonText);

            // 设置回调
            onConfirmCallback = data.onConfirm;
            onCancelCallback = data.onCancel;
            onCloseCallback = data.onClose;

            // 设置按钮显示
            SetButtonsVisibility(data.showCancelButton);
        }



        /// <summary>
        /// 设置标题文本
        /// </summary>
        /// <param name="title">标题</param>
        public void SetTitle(string title)
        {
            if (titleText != null)
            {
                titleText.text = title;
            }
        }

        /// <summary>
        /// 设置消息文本
        /// </summary>
        /// <param name="message">消息内容</param>
        public void SetMessage(string message)
        {
            if (messageText != null)
            {
                messageText.text = message;
            }
        }

        /// <summary>
        /// 设置确认按钮文本
        /// </summary>
        /// <param name="text">按钮文本</param>
        public void SetConfirmButtonText(string text)
        {
            if (confirmButtonText != null && !string.IsNullOrEmpty(text))
            {
                confirmButtonText.text = text;
            }
        }

        /// <summary>
        /// 设置取消按钮文本
        /// </summary>
        /// <param name="text">按钮文本</param>
        public void SetCancelButtonText(string text)
        {
            // if (cancelButtonText != null && !string.IsNullOrEmpty(text))
            // {
            //     cancelButtonText.text = text;
            // }
        }

        /// <summary>
        /// 设置按钮显示状态
        /// </summary>
        /// <param name="showCancel">是否显示取消按钮</param>
        public void SetButtonsVisibility(bool showCancel)
        {
            if (cancelButtonObject != null)
            {
                cancelButtonObject.SetActive(showCancel);
            }
            else if (cancelButton != null)
            {
                cancelButton.gameObject.SetActive(showCancel);
            }
        }

        /// <summary>
        /// 确认按钮点击事件
        /// </summary>
        private void OnConfirmClick()
        {
            onConfirmCallback?.Invoke();
            Hide();

        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void OnCancelClick()
        {
            onCancelCallback?.Invoke();
            Hide();
        }

        /// <summary>
        /// 重写Hide方法，添加关闭回调
        /// </summary>
        public override void Hide(params object[] args)
        {
            onCloseCallback?.Invoke();
            base.Hide(args);
        }

        #region 静态便捷方法

        /// <summary>
        /// 快速显示简单消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="onConfirm">确认回调</param>
        public static void ShowMessage(string message, Action onConfirm = null)
        {
            var data = new MessagePopupData(message, onConfirm);
            data.showCancelButton = true;
            UIManager.Instance.ShowPopup<MessagePopup>(data);
        }


        /// <summary>
        /// 快速显示确认对话框
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="onConfirm">确认回调</param>
        /// <param name="onCancel">取消回调</param>
        public static void ShowConfirm(string message, Action onConfirm = null, Action onCancel = null)
        {
            var data = new MessagePopupData(message, onConfirm, onCancel);
            UIManager.Instance.ShowPopup<MessagePopup>(data);
        }

    

        /// <summary>
        /// 快速显示带标题的确认对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">消息内容</param>
        /// <param name="onConfirm">确认回调</param>
        /// <param name="onCancel">取消回调</param>
        public static void ShowConfirm(string title, string message, Action onConfirm = null, Action onCancel = null)
        {
            var data = new MessagePopupData(title, message, onConfirm, onCancel);
            UIManager.Instance.ShowPopup<MessagePopup>(data);
        }

    

        /// <summary>
        /// 显示自定义提示框
        /// </summary>
        /// <param name="data">提示框数据</param>
        public static void ShowCustom(MessagePopupData data)
        {
            UIManager.Instance.ShowPopup<MessagePopup>(data);
        }

        #endregion
    }
}
