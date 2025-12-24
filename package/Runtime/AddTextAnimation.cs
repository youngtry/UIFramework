using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AddTextAnimation : MonoBehaviour
{
   [HideInInspector] public bool isPlaying = false;
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
        isPlaying = true;
        transform.position = new Vector3(worldPosition.x+50, worldPosition.y, 0);
        addCountText.text = "+" + count;
        gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1.15f);
        sequence.AppendCallback(() =>
        {
            gameObject.SetActive(false);
        });
        sequence.AppendInterval(0.2f);
        sequence.OnComplete(() =>
        {
            isPlaying = false;
        });
        sequence.Play();
    }
}
