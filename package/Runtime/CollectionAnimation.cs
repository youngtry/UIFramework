using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UIFramework;
using UnityEngine;

public class CollectionAnimation : MonoBehaviour
{
    [HideInInspector]public bool isPlaying = false;
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
        isPlaying = true;
        if (data.isMultiple)
        {
            ShowMutipleEffect(data);
        }
        else
        {
            ShowSingleEffect(data);
        }



    }

    private void ShowSingleEffect(CollectionAnimationData data)
    {
        DOTween.Kill(this);
        GameObject icon = data.target[0];
        //飞行到diamondText位置销毁
        icon.transform.position = data.startPosition;
        icon.SetActive(true);
        Vector3 targetPosition = data.targetTransform.position;
        Vector3 startPosition = data.startPosition;
        if (data.isSpecifyEndPosition)
        {
            targetPosition = data.endPosition;
        }
        gameObject.SetActive(true);
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
            icon.SetActive(false);
            icon.transform.position = startPosition;
            UIManager.Instance.ShowAddTextEffect(targetPosition, data.amountStr);

        });
        sequence.AppendInterval(0.2f);
        sequence.AppendCallback(() =>
        {
            isPlaying = false;
        });

        sequence.Play();
    }

    private void ShowMutipleEffect(CollectionAnimationData data)
    {
        DOTween.Kill(this);
        for (int i = 0; i < data.target.Count; i++)
        {
            GameObject icon = data.target[i];
            //飞行到diamondText位置销毁
            icon.transform.position = data.startPosition;
            icon.SetActive(true);
            Vector3 targetPosition = data.targetTransform.position;
            Vector3 startPosition = data.startPosition;
            if (data.isSpecifyEndPosition)
            {
                targetPosition = data.endPosition;
            }
            gameObject.SetActive(true);
            List<Vector3> path = new List<Vector3>
            {
                startPosition,
                new Vector3(startPosition.x + Random.Range(-200,200), (targetPosition.y - startPosition.y) / 5 + startPosition.y, 0),
                targetPosition
            };
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(Random.Range(0, 0.5f));
            sequence.Append(icon.transform.DOPath(path.ToArray(), 1f, PathType.CatmullRom, PathMode.TopDown2D));
            sequence.AppendCallback(() =>
            {
                icon.SetActive(false);
                icon.transform.position = startPosition;
            });
            sequence.Play();
        }

        Sequence sequence2 = DOTween.Sequence();
        sequence2.AppendInterval(1.5f);
        sequence2.AppendCallback(() =>
        {
            ReturnPool();
            Vector3 targetPosition = data.targetTransform.position;
            if (data.isSpecifyEndPosition)
            {
                targetPosition = data.endPosition;
            }
            UIManager.Instance.ShowAddTextEffect(targetPosition, data.amountStr);
        });
        sequence2.AppendInterval(0.2f);
        sequence2.AppendCallback(() =>
        {
            isPlaying = false;
        });
        sequence2.Play();

    }

    private void ReturnPool()
    {
        gameObject.SetActive(false);
    }

}
