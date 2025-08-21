using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UIFramework
{
    public class Notice : MonoBehaviour
    {
        [Header("UI组件")]
        [SerializeField] private Text noticeContext;
        [SerializeField] private RectTransform noticeContainer;
        [SerializeField] private Button clickButton;

        [Header("滑动设置")]
        [SerializeField] private float scrollSpeed = 100f; // 像素/秒
        [SerializeField] private float startDelay = 0.5f; // 开始滑动前的延迟
        [SerializeField] private float endDelay = 0.5f; // 滑动完成后的延迟

        private RectTransform textRectTransform;
        private float containerWidth;
        private float textWidth;
        private Coroutine scrollCoroutine;
        private bool isScrolling = false;

        void Awake()
        {
            if (noticeContext != null)
            {
                textRectTransform = noticeContext.GetComponent<RectTransform>();
            }

            if (noticeContainer == null)
            {
                noticeContainer = GetComponent<RectTransform>();
            }

        }

        void Start()
        {
            // 初始时隐藏notice
            // // gameObject.SetActive(false);
        }

        ////
        /// 设置notice的点击回调，如无则无需设置
        /// 
        public void SetNoticeCallback(Action callback)
        {
            if (clickButton != null)
            {
                clickButton.onClick.AddListener(() => { callback?.Invoke(); });
            }
        }

        /// <summary>
        /// 显示notice并开始滑动
        /// </summary>
        /// <param name="text">要显示的文本</param>
        public void ShowNotice(string text)
        {
            if (text == null || noticeContext == null)
            {
                return;
            }

            // 停止之前的滑动
            StopScrolling();

            // 设置文本
            noticeContext.text = text;

            // 显示notice
            gameObject.SetActive(true);

            // 开始滑动
            StartScrolling();
        }

        /// <summary>
        /// 开始滑动动画
        /// </summary>
        private void StartScrolling()
        {
            if (scrollCoroutine != null)
            {
                StopCoroutine(scrollCoroutine);
            }

            scrollCoroutine = StartCoroutine(ScrollCoroutine());
        }

        /// <summary>
        /// 停止滑动动画
        /// </summary>
        public void StopScrolling()
        {
            isScrolling = false;

            if (scrollCoroutine != null)
            {
                StopCoroutine(scrollCoroutine);
                scrollCoroutine = null;
            }

            // 停止DOTween动画
            if (textRectTransform != null)
            {
                textRectTransform.DOKill();
            }
        }

        /// <summary>
        /// 滑动协程
        /// </summary>
        private IEnumerator ScrollCoroutine()
        {
            isScrolling = true;

            // 等待一帧确保UI布局完成
            yield return null;

            // 计算尺寸
            CalculateDimensions();

            // // 如果文本宽度小于等于容器宽度，不需要滑动
            // if (textWidth <= containerWidth)
            // {
            //     yield return new WaitForSeconds(startDelay + endDelay + 2f); // 显示2秒
            //     HideNotice();
            //     yield break;
            // }

            // 设置初始位置（文本完全在右侧外面）
            SetTextPosition(containerWidth);
            transform.localScale = new Vector3(1, 0, 1);
            yield return transform.DOScaleY(1, 0.3f).Play().WaitForCompletion();

            // 开始延迟
            // yield return new WaitForSeconds(startDelay);

            if (!isScrolling) yield break;

            // 计算滑动距离和时间
            float scrollDistance = containerWidth + textWidth;
            float scrollDuration = scrollDistance / scrollSpeed;

            // 执行滑动动画
            yield return textRectTransform.DOAnchorPosX(-textWidth, scrollDuration)
                .SetEase(Ease.Linear)
                .Play()
                .WaitForCompletion();

            if (!isScrolling) yield break;

            // 结束延迟
            yield return new WaitForSeconds(endDelay);

            yield return transform.DOScaleY(0, 0.3f).Play().WaitForCompletion();

            // 隐藏notice
            HideNotice();
        }

        /// <summary>
        /// 计算容器和文本的尺寸
        /// </summary>
        private void CalculateDimensions()
        {
            if (noticeContainer != null)
            {
                containerWidth = noticeContainer.rect.width;
            }

            if (textRectTransform != null && noticeContext != null)
            {
                // 强制更新文本布局
                Canvas.ForceUpdateCanvases();

                // 获取文本的首选宽度
                textWidth = noticeContext.preferredWidth;

                // 如果首选宽度为0，使用当前宽度
                if (textWidth <= 0)
                {
                    textWidth = textRectTransform.rect.width;
                }
            }
        }

        /// <summary>
        /// 设置文本位置
        /// </summary>
        /// <param name="xPosition">X坐标位置</param>
        private void SetTextPosition(float xPosition)
        {
            if (textRectTransform != null)
            {
                Vector2 anchoredPosition = textRectTransform.anchoredPosition;
                anchoredPosition.x = xPosition;
                textRectTransform.anchoredPosition = anchoredPosition;
            }
        }

        /// <summary>
        /// 隐藏notice
        /// </summary>
        public void HideNotice()
        {
            StopScrolling();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 当对象被销毁时清理
        /// </summary>
        void OnDestroy()
        {
            StopScrolling();
        }

        /// <summary>
        /// 当对象被禁用时停止滑动
        /// </summary>
        void OnDisable()
        {
            StopScrolling();
        }
    }
}
