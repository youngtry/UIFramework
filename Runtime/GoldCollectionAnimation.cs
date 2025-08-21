using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace UIFramework
{
    public class GoldCollectionAnimation : MonoBehaviour
    {
        
    [SerializeField] private GameObject moneyEffect;
    [SerializeField] private Transform targetPos;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject addTextNode;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
            List<Vector3> path = new List<Vector3>
        {
            worldPosition,
            new Vector3(worldPosition.x + Random.Range(-200,200), (targetPos.position.y - worldPosition.y) / 5 + worldPosition.y, 0),
            // new Vector3(worldPosition.x + 50, (diamondText.transform.position.y - worldPosition.y) / 3 + worldPosition.y, 0),
            targetPos.position
        };
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(Random.Range(0, 0.5f));
            sequence.Append(diamond.transform.DOPath(path.ToArray(), 1f, PathType.CatmullRom, PathMode.TopDown2D));
            sequence.Play();
        }

        Sequence sequence2 = DOTween.Sequence();
        sequence2.AppendInterval(0.5f);
        sequence2.AppendCallback(() =>
        {
            GetComponent<AddTextAnimation>().ShowAddCountText(amount.ToString());

            animator.enabled = true;
            addTextNode.SetActive(true);
            addTextNode.transform.position = targetPos.position;
            Destroy(gameObject, 1);
        });
        sequence2.Play();
    }

    }
}
