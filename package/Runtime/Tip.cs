using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CommonTools;

namespace UIFramework
{
    public class Tips : MonoBehaviour
    {
        [Header("UI组件")]
        [SerializeField] private Text tipsText;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("自动隐藏设置")]
        [SerializeField] private bool enableAutoHide = true;
        [SerializeField] private float autoHideDelay = 2f; // 自动隐藏延迟时间（秒）
        [SerializeField] private float fadeOutDuration = 0.5f; // 淡出动画时长
        [SerializeField] private float moveUpDistance = 50f; // 向上移动的距离（像素）
        [SerializeField] private bool pauseOnHover = true; // 鼠标悬停时暂停自动隐藏

        [HideInInspector] public bool isPlaying = false;
        // 内部状态
        private Coroutine autoHideCoroutine;
        private Tween fadeTween;
        private bool isMouseOver = false;

        void Start()
        {
            // 确保初始状态正确
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }

            // 初始隐藏
            // gameObject.SetActive(false);
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="tips">提示文本</param>
        public void ShowTips(string tips)
        {
            isPlaying = true;
            ShowTips(tips, autoHideDelay);
        }

        /// <summary>
        /// 显示提示信息（自定义自动隐藏时间）
        /// </summary>
        /// <param name="tips">提示文本</param>
        /// <param name="customAutoHideDelay">自定义自动隐藏延迟时间，-1表示不自动隐藏</param>
        public void ShowTips(string tips, float customAutoHideDelay)
        {
            // 停止之前的动画和协程
            // StopAutoHide();

            // 设置文本
            if (tipsText != null)
            {
                tipsText.text = tips;
            }

            // 显示UI
            gameObject.SetActive(true);

            // 重置透明度
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
            }

            HideTips(true);

            // // 启动自动隐藏（如果启用且延迟时间大于0）
            // if (enableAutoHide && customAutoHideDelay > 0)
            // {
            //     autoHideCoroutine = StartCoroutine(AutoHideCoroutine(customAutoHideDelay));
            // }

            Utils.Log($"Tips shown: {tips}", Utils.LogType.Info);
        }

        /// <summary>
        /// 手动隐藏提示
        /// </summary>
        public void HideTips()
        {
            HideTips(true);
        }

        /// <summary>
        /// 隐藏提示
        /// </summary>
        /// <param name="useAnimation">是否使用淡出动画</param>
        public void HideTips(bool useAnimation)
        {
            // StopAutoHide();

            if (useAnimation)
            {
                // 获取当前位置
                transform.localPosition = new Vector3(transform.localPosition.x, 0, 0);
                RectTransform rectTransform = GetComponent<RectTransform>();
                if (rectTransform != null && canvasGroup != null)
                {
                    // 创建动画序列
                    Sequence sequence = DOTween.Sequence();

                    // 添加向上移动动画
                    Vector2 startPosition = rectTransform.anchoredPosition;
                    Vector2 endPosition = new Vector2(startPosition.x, startPosition.y + moveUpDistance);
                    sequence.AppendInterval(autoHideDelay);
                    sequence.Append(rectTransform.DOAnchorPos(endPosition, fadeOutDuration));

                    // 同时添加淡出动画
                    sequence.Join(canvasGroup.DOFade(0f, fadeOutDuration));

                    // 动画完成后销毁游戏对象
                    sequence.AppendCallback(() =>
                    {
                        gameObject.SetActive(false);
                    });
                    sequence.AppendInterval(0.2f);
                    sequence.AppendCallback(() =>
                    {
                        isPlaying = false;
                    });

                    sequence.Play();

                    // 保存动画引用以便可以停止
                    fadeTween = sequence;
                }
                else
                {
                    // 如果没有必要的组件，直接销毁
                    gameObject.SetActive(false);
                }
            }
            else
            {
                // 不使用动画，直接销毁
                gameObject.SetActive(false);

            }

            Utils.Log("Tips hidden and will be destroyed", Utils.LogType.Info);
        }

        /// <summary>
        /// 自动隐藏协程
        /// </summary>
        private IEnumerator AutoHideCoroutine(float delay)
        {
            float elapsedTime = 0f;

            while (elapsedTime < delay)
            {
                // 如果启用了鼠标悬停暂停功能且鼠标在上方，则暂停计时
                if (pauseOnHover && isMouseOver)
                {
                    yield return null;
                    continue;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 时间到，自动隐藏
            HideTips(true);
        }

        /// <summary>
        /// 停止自动隐藏
        /// </summary>
        private void StopAutoHide()
        {
            if (autoHideCoroutine != null)
            {
                StopCoroutine(autoHideCoroutine);
                autoHideCoroutine = null;
            }

            if (fadeTween != null)
            {
                fadeTween.Kill();
                fadeTween = null;
            }
        }

        /// <summary>
        /// 重置自动隐藏计时器
        /// </summary>
        public void ResetAutoHideTimer()
        {
            if (enableAutoHide && gameObject.activeInHierarchy)
            {
                StopAutoHide();
                autoHideCoroutine = StartCoroutine(AutoHideCoroutine(autoHideDelay));
            }
        }

        /// <summary>
        /// 设置自动隐藏启用状态
        /// </summary>
        public void SetAutoHideEnabled(bool enabled)
        {
            enableAutoHide = enabled;
            if (!enabled)
            {
                StopAutoHide();
            }
        }

        /// <summary>
        /// 设置自动隐藏延迟时间
        /// </summary>
        public void SetAutoHideDelay(float delay)
        {
            autoHideDelay = delay;
        }

        /// <summary>
        /// 检查提示是否正在显示
        /// </summary>
        public bool IsShowing => gameObject.activeInHierarchy;

        #region 鼠标事件处理

        /// <summary>
        /// 鼠标进入时暂停自动隐藏
        /// </summary>
        public void OnMouseEnter()
        {
            isMouseOver = true;
        }

        /// <summary>
        /// 鼠标离开时恢复自动隐藏
        /// </summary>
        public void OnMouseExit()
        {
            isMouseOver = false;
        }

        #endregion

        #region 静态工厂方法

        /// <summary>
        /// 创建并显示新的Tips实例
        /// </summary>
        /// <param name="tips">提示文本</param>
        /// <param name="parent">父级Transform，如果为null则查找Canvas</param>
        /// <param name="autoHideDelay">自动隐藏延迟时间，-1表示不自动隐藏</param>
        /// <returns>创建的Tips实例</returns>
        public static Tips CreateAndShow(string tips, Transform parent = null, float autoHideDelay = 2f)
        {
            // 查找父级Canvas
            if (parent == null)
            {
                Canvas canvas = FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    parent = canvas.transform;
                }
            }

            // 创建Tips GameObject
            GameObject tipsGO = new GameObject("Tips");
            if (parent != null)
            {
                tipsGO.transform.SetParent(parent, false);
            }

            // 添加必要的组件
            RectTransform rectTransform = tipsGO.AddComponent<RectTransform>();
            CanvasGroup canvasGroup = tipsGO.AddComponent<CanvasGroup>();

            // 创建文本子对象
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(tipsGO.transform, false);

            RectTransform textRect = textGO.AddComponent<RectTransform>();
            Text textComponent = textGO.AddComponent<Text>();

            // 设置文本组件属性
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.fontSize = 16;
            textComponent.color = Color.white;
            textComponent.alignment = TextAnchor.MiddleCenter;

            // 设置RectTransform
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            // 添加Tips组件
            Tips tipsComponent = tipsGO.AddComponent<Tips>();
            tipsComponent.tipsText = textComponent;
            tipsComponent.canvasGroup = canvasGroup;
            tipsComponent.autoHideDelay = autoHideDelay;

            // 设置默认位置（屏幕中央偏上）
            rectTransform.anchorMin = new Vector2(0.5f, 0.7f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.7f);
            rectTransform.sizeDelta = new Vector2(200f, 50f);
            rectTransform.anchoredPosition = Vector2.zero;

            // 显示提示
            tipsComponent.ShowTips(tips, autoHideDelay);

            return tipsComponent;
        }

        /// <summary>
        /// 快速显示提示（使用默认设置）
        /// </summary>
        /// <param name="tips">提示文本</param>
        /// <param name="autoHideDelay">自动隐藏延迟时间</param>
        public static void Show(string tips, float autoHideDelay = 2f)
        {
            CreateAndShow(tips, null, autoHideDelay);
        }

        #endregion

        /// <summary>
        /// 组件销毁时清理
        /// </summary>
        private void OnDestroy()
        {
            StopAutoHide();
        }
    }
}
