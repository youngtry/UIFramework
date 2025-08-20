using System.Collections;
using System.Collections.Generic;
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
    
    public void ShowAddCountText(string count)
    {
        addCountText.text = "+" + count;
    }
}
