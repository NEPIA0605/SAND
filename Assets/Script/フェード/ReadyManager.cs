using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyManager : MonoBehaviour
{
    private GameObject FadeObj; //フェードのパネル
    private bool FadeInFlg;     //フェードインフラグ
    bool ReadyFlg;              //レディフラグ
    float ReadyCnt;               //レディカウント
    public float ReadyTime;              //レディの時間

    bool StartFlg;              //スタートフラグ
    float StartCnt;               //スタートカウント
    public float StartTime;              //スタートの時間

    GameObject ReadyUI; //レディのパネル
    GameObject StartUI; //スタートのパネル

    bool GameStartFlg;  //ゲームスタートを表すフラグ

    // Start is called before the first frame update
    void Start()
    {
        //変数初期化
        FadeObj = GameObject.Find("FadePanel");
        ReadyUI = GameObject.Find("ReadyUI");
        StartUI = GameObject.Find("StartUI");


        FadeInFlg = false;
        ReadyFlg = false;
        StartFlg = false;
        GameStartFlg = false;

        ReadyCnt = 0.0f;

        StartCnt = 0.0f;

        ReadyUI.SetActive(false);
        StartUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        FadeInFlg = FadeObj.GetComponent<FadeManager>().GetFadeInFlg();
        //Debug.Log(FadeInFlg);

        if (FadeInFlg == false)
        {
            ReadyFlg = true;
        }

        //レディを開始
        if (ReadyFlg == true)
        {

            if (ReadyCnt >= ReadyTime)
            {
                ReadyUI.SetActive(false);
                StartUI.SetActive(true);
                StartFlg = true;
            }
            else
            {
                ReadyUI.SetActive(true);

                //時間をカウントする
                ReadyCnt += Time.deltaTime;
            }
        }

        //スタートUI開始
        if (StartFlg == true)
        {
            if (StartCnt <= StartTime)
            {
                //時間をカウントする
                StartCnt += Time.deltaTime;
            }
            else
            {
                StartFlg = false;
                GameStartFlg = true;
                StartUI.SetActive(false);
            }
        }
           
        if (GameStartFlg == false)
        {
            Time.timeScale = 0f;
        }
        else
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    //FadeInFlgフラグのSetter
    public bool GetGameStartFlg()
    {
        return GameStartFlg;
    }

    //ReadyFlgフラグのSetter
    public bool GetReadyFlg()
    {
        return ReadyFlg;
    }

}
