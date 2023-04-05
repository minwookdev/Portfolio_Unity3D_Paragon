using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Maxbuy : MonoBehaviour
{
    public Text buytext;
    int BuyCount;
    // Start is called before the first frame update
    void Start()
    {
        BuyCount = 0;
    }

    // Update is called once per frame
    public void TextChange()
    {
        BuyCount++;
        if(BuyCount >= 3)
        {
            buytext.text = "Max";
            BuyCount = 0;
        }
    }
}
