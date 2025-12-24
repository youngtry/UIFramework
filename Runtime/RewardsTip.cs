using System.Collections;
using System.Collections.Generic;
using CommonTools;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RewardsTip : MonoBehaviour
{
    [Header("UI组件")]
    [SerializeField] private Text amountText;

    [SerializeField] private Transform targetContainer;

    [Header("动画设置")]
    [SerializeField] private float autoDestroyTime = 2f;
    [SerializeField] private float moveUpDistance = 100f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private bool isDestroying = false;

    void Awake()
    {
        // 获取或添加CanvasGroup组件用于透明度控制
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 初始化奖励提示并开始自动消失倒计时
    /// </summary>
    public void Initialize(GameObject target, string amountStr, float destroyTime = 2f)
    {
        autoDestroyTime = destroyTime;
        target.transform.SetParent(targetContainer);
        amountText.text = amountStr;

        // 开始自动消失倒计时
        StartCoroutine(AutoDestroyCoroutine());
    }

    
    /// <summary>
    /// 自动消失协程
    /// </summary>
    private IEnumerator AutoDestroyCoroutine()
    {
        // 等待指定时间
        yield return new WaitForSeconds(autoDestroyTime - fadeOutDuration);

        // 开始消失动画
        StartDestroyAnimation();
    }

    /// <summary>
    /// 开始消失动画
    /// </summary>
    public void StartDestroyAnimation()
    {
        if (isDestroying) return;

        isDestroying = true;

        // 使用DOTween创建动画序列
        Sequence destroySequence = DOTween.Sequence();

        // 向上移动动画
        destroySequence.Append(rectTransform.DOAnchorPosY(
            rectTransform.anchoredPosition.y + moveUpDistance,
            fadeOutDuration
        ).SetEase(Ease.OutQuart));

        // 淡出动画（与移动同时进行）
        destroySequence.Join(canvasGroup.DOFade(0f, fadeOutDuration).SetEase(Ease.OutQuart));

        // 动画完成后销毁对象
        destroySequence.OnComplete(() =>
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        });
    }

    /// <summary>
    /// 立即销毁（无动画）
    /// </summary>
    public void DestroyImmediate()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 设置自动消失时间
    /// </summary>
    public void SetAutoDestroyTime(float time)
    {
        autoDestroyTime = time;
    }

    /// <summary>
    /// 设置移动距离
    /// </summary>
    public void SetMoveUpDistance(float distance)
    {
        moveUpDistance = distance;
    }

    /// <summary>
    /// 设置淡出持续时间
    /// </summary>
    public void SetFadeOutDuration(float duration)
    {
        fadeOutDuration = duration;
    }

    void OnDestroy()
    {
        // 清理DOTween动画
        DOTween.Kill(rectTransform);
        DOTween.Kill(canvasGroup);
    }
}
