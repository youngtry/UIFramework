using System.Collections;
using System.Collections.Generic;
using CommonTools;
using UnityEngine;
#if DOTWEEN_ENABLED
using DG.Tweening;
#endif

namespace UIFramework
{
    public class DiamondCollectionAnimation : MonoBehaviour
    {


        [SerializeField] private GameObject diamondEffect;
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

        public void ShowDiamondEffect(Vector3 worldPosition, double amount, bool multible)
        {
            animator.enabled = false;
            addTextNode.SetActive(false);
            diamondEffect.SetActive(true);
            if (multible)
            {
                ShowMutipleDiamondEffect(worldPosition, amount);
            }
            else
            {
                ShowSingleDiamondEffect(worldPosition, amount);
            }
        }



        private void ShowSingleDiamondEffect(Vector3 worldPosition, double amount)
        {
            GameObject diamond = Instantiate(diamondEffect, transform);
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
            sequence.Append(diamond.transform.DOPath(path.ToArray(), 1f, PathType.CatmullRom, PathMode.TopDown2D));
            sequence.AppendCallback(() =>
            {
                Destroy(diamond);
                animator.enabled = true;
                addTextNode.SetActive(true);
                addTextNode.transform.position = animator.transform.position;
                if (amount > 0)
                    GetComponent<AddTextAnimation>().ShowAddCountText(amount.ToString());
                // UserDataManager.Instance.userDiamond.Value += amount;
                Destroy(gameObject, 1);
            });

            sequence.Play();
        }

        private void ShowMutipleDiamondEffect(Vector3 worldPosition, double amount)
        {
            for (int i = 0; i < 15; i++)
            {
                GameObject diamond = Instantiate(diamondEffect, transform);
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
                if (amount > 0)
                {
                    GetComponent<AddTextAnimation>().ShowAddCountText(amount.ToString());

                }
                animator.enabled = true;
                addTextNode.SetActive(true);
                addTextNode.transform.position = animator.transform.position;
                // UserDataManager.Instance.userDiamond.Value += amount;
                Destroy(gameObject, 1);
            });
            sequence2.Play();
        }
    }
}
