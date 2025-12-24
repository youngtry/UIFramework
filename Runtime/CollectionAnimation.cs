using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UIFramework;
using UnityEngine;

public class CollectionAnimation : MonoBehaviour
{
    [SerializeField] private GameObject iconNode;
    [SerializeField] private Transform targetNode;

    private List<GameObject> icons = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowEffect(CollectionAnimationData data)
    {

        if (data.isMultiple)
        {
            ShowMutipleEffect(data);
        }
        else
        {
            ShowSingleEffect(data);
        }

        gameObject.SetActive(true);

    }

    private void ShowSingleEffect(CollectionAnimationData data)
    {
        GameObject icon = data.target[0];
        //飞行到diamondText位置销毁
        icon.transform.position = data.startPosition;
        icon.SetActive(true);
        Vector3 targetPosition = data.targetTransform.position;
        Vector3 startPosition = data.startPosition;
        if(data.isSpecifyEndPosition)
        {
            targetPosition = targetNode.position;
        }
        List<Vector3> path = new List<Vector3>
        {
            startPosition,
            new Vector3(startPosition.x + Random.Range(-200,200), (targetPosition.y - startPosition.y) / 5 + startPosition.y, 0),
            // new Vector3(worldPosition.x + 50, (diamondText.transform.position.y - worldPosition.y) / 3 + worldPosition.y, 0),
            targetPosition
        };
        Sequence sequence = DOTween.Sequence();
        sequence.Append(icon.transform.DOPath(path.ToArray(), 1f, PathType.CatmullRom, PathMode.TopDown2D));
        sequence.AppendCallback(() =>
        {
            ReturnPool();
            UIManager.Instance.ShowAddTextEffect(targetPosition, data.amountStr);
            
        });

        sequence.Play();
    }

    private void ShowMutipleEffect(CollectionAnimationData data)
    {

    }

    private void ReturnPool()
    {
        gameObject.SetActive(false);
    }

}
