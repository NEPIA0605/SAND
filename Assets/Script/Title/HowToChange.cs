using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToChange : MonoBehaviour
{
    [Header("フラグと本体")]
    public GameObject HowTo;    //操作説明ウィンドウ
    public bool HowToFlg;       //ウィンドウのフラグ

    // Start is called before the first frame update
    void Start()
    {
        HowToFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        //出したり消したりを反映
        HowTo.SetActive(HowToFlg);
    }

    //出す
    public void HowToOpen()
    {
        if(!HowToFlg)
        {
            HowToFlg = true;
        }
    }

    //消す
    public void HowToClose()
    {
        if (HowToFlg)
        {
            HowToFlg = false;
        }
    }

    //HowToFlgのGetter
    public bool GetHowToFlg()
    {
        return HowToFlg;
    }
}
