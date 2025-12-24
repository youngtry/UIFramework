using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AddTextAnimation : MonoBehaviour
{
   
    [SerializeField]private Text addCountText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void ShowAddCountText(Vector3 worldPosition ,string count)
    {
        transform.position = worldPosition;
        addCountText.text = "+" + count;
        gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1.15f);
        sequence.AppendCallback(() =>
        {
            gameObject.SetActive(false);
        });
        sequence.Play();
    }
}
