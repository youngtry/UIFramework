using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFramework
{
    /// <summary>
    /// UI弹窗基类
    /// </summary>  
    public class Popup : BasePage
    {
        protected bool isInitialized = false;
        protected CanvasGroup canvasGroup;
        protected RectTransform rectTransform;

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void Start()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            if (isInitialized) return;
            isInitialized = true;
        }

        public override void Show(params object[] args)
        {
            gameObject.SetActive(true);
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public override void Hide(params object[] args)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            gameObject.SetActive(false);

            // 通知UIManager弹窗已关闭
            if (UIManager.Instance != null)
            {
                UIManager.Instance.OnPopupClosed(this);
            }
        }

        public override void Refresh()
        {
            if (!isInitialized)
            {
                Initialize();
            }
        }

        public override bool IsVisible()
        {
            return gameObject.activeSelf && (canvasGroup == null || canvasGroup.alpha > 0);
        }

        protected virtual void OnDestroy()
        {
            // 子类可以重写此方法进行清理工作
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
