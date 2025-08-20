using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if DOTWEEN_ENABLED
using DG.Tweening;
#endif

namespace UIFramework
{
    public class GoldCollectionAnimation : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private GameObject moneyEffect;
        [SerializeField] private Transform targetPos;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject addTextNode;

        [Header("Customization")]
        [SerializeField] private bool useCustomIcon = false;
        [SerializeField] private Sprite customGoldIcon;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Start()
    {
        // 如果没有使用自定义图标，则从资源管理器获取
        if (!useCustomIcon)
        {
            UpdateIconFromResourceManager();
        }
    }

    /// <summary>
    /// 从资源管理器更新图标
    /// </summary>
    private void UpdateIconFromResourceManager()
    {
        var resourceManager = UIResourceManager.Instance;
        if (resourceManager != null)
        {
            var goldIcon = resourceManager.GetGoldIcon();
            if (goldIcon != null && moneyEffect != null)
            {
                var image = moneyEffect.GetComponent<UnityEngine.UI.Image>();
                if (image != null)
                {
                    image.sprite = goldIcon;
                }
            }
        }
    }

    /// <summary>
    /// 设置自定义图标
    /// </summary>
    public void SetCustomIcon(Sprite icon)
    {
        customGoldIcon = icon;
        useCustomIcon = true;

        if (moneyEffect != null)
        {
            var image = moneyEffect.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.sprite = icon;
            }
        }
    }

    public void ShowMoneyEffect(Vector3 worldPosition, double amount)
    {
        animator.enabled = false;
        moneyEffect.SetActive(true);
        ShowMutipleMoneyEffect(worldPosition, amount);
    }

    private void ShowMutipleMoneyEffect(Vector3 worldPosition, double amount)
    {
        for (int i = 0; i < 15; i++)
        {
            GameObject diamond = Instantiate(moneyEffect, transform);
            //飞行到diamondText位置销毁
            diamond.transform.position = worldPosition;

#if DOTWEEN_ENABLED
            List<Vector3> path = new List<Vector3>
            {
                worldPosition,
                new Vector3(worldPosition.x + Random.Range(-200,200), (targetPos.position.y - worldPosition.y) / 5 + worldPosition.y, 0),
                targetPos.position
            };
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(Random.Range(0, 0.5f));
            sequence.Append(diamond.transform.DOPath(path.ToArray(), 1f, PathType.CatmullRom, PathMode.TopDown2D));
            sequence.Play();
#else
            // 使用Unity内置动画作为备用方案
            StartCoroutine(AnimateToTarget(diamond, worldPosition, Random.Range(0, 0.5f)));
#endif
        }

#if DOTWEEN_ENABLED
        Sequence sequence2 = DOTween.Sequence();
        sequence2.AppendInterval(0.5f);
        sequence2.AppendCallback(() =>
        {
            FinishAnimation(amount);
        });
        sequence2.Play();
#else
        // 使用Unity内置延迟调用作为备用方案
        StartCoroutine(DelayedFinishAnimation(0.5f, amount));
#endif
    }

    private void FinishAnimation(double amount)
    {
        GetComponent<AddTextAnimation>().ShowAddCountText(amount.ToString());
        animator.enabled = true;
        addTextNode.SetActive(true);
        addTextNode.transform.position = targetPos.position;
        Destroy(gameObject, 1);
    }

#if !DOTWEEN_ENABLED
    private IEnumerator AnimateToTarget(GameObject obj, Vector3 startPos, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 midPoint = new Vector3(startPos.x + Random.Range(-200, 200), (targetPos.position.y - startPos.y) / 5 + startPos.y, 0);
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 简单的贝塞尔曲线动画
            Vector3 pos = Vector3.Lerp(Vector3.Lerp(startPos, midPoint, t), Vector3.Lerp(midPoint, targetPos.position, t), t);
            if (obj != null)
                obj.transform.position = pos;

            yield return null;
        }

        if (obj != null)
            Destroy(obj);
    }

    private IEnumerator DelayedFinishAnimation(float delay, double amount)
    {
        yield return new WaitForSeconds(delay);
        FinishAnimation(amount);
    }
#endif

    }
}
