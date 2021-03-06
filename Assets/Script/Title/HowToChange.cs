﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToChange : MonoBehaviour
{
    [Header("フラグと本体")]
    public GameObject HowTo;    //操作説明ウィンドウ
    public bool HowToFlg;       //ウィンドウのフラグ
    public Button HowToButton;  //操作説明のボタン
    public OptionManager OptMan;    //おぷまね

    // Start is called before the first frame update
    void Start()
    {
        HowToFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (HowToFlg)
        {
            //Bで消す
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown("joystick button 1"))
            {
                HowToFlg = false;
                HowToButton.Select();
                OptMan.AsPlayOs(2);
                Debug.Log("kokoka");
            }
        }

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
