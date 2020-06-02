using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //定数定義
    private const float SANDTIME_MAX = 5.0f;

    [Header("UIの砂部分(上下)")]
    public Image[] UI_Sand;     //０：上　１：下

    [Header("ボタンUI")]
    public Image A_Button;      //Aボタン

    [Header("回転のアニメーションが入ったアニメーターObj")]
    public Animator TurnAnimator;      //回転アニメーター

    [Header("プレイヤコントローラ")]
    [SerializeField] private PlayerControler PLCon;  //アニメーションのフラグを取る

    [Header("その他パラメータ")]
    private bool OldPlayerTurnAnimFlg = false;  //前のPTAフラグ
    private bool PlayerTurn;                    //プレイヤの正逆

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤコントローラを取る
        PLCon = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();

        //FillOriginを設定
        UI_Sand[0].fillOrigin = UI_Sand[1].fillOrigin = (int)Image.OriginVertical.Bottom;
    }

    // Update is called once per frame
    void Update()
    {
        //Old(略)で分岐
        if (OldPlayerTurnAnimFlg)
        {
            //Player(略)で分岐
            if (PLCon.GetPlayerTurnAnimFlg())
            {
                //回転中＆再回転クール待ち中はボタンを灰色にする
                A_Button.color = new Color(127 / 255.0f, 127 / 255.0f, 127 / 255.0f, 1.0f);
            }
            else
            {
                //プレイヤ側で再回転できるようになった
                OldPlayerTurnAnimFlg = false;

                //ボタンの色を元に戻す
                A_Button.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            //Player(略)で分岐
            if (PLCon.GetPlayerTurnAnimFlg())
            {
                //正逆でアニメーション分岐
                if (PLCon.GetPlayerTurn())
                {
                    TurnAnimator.Play("Turn_1");
                    PlayerTurn = true;
                }
                else
                {
                    TurnAnimator.Play("Turn_2");
                    PlayerTurn = false;
                }

                //UI砂のFillOriginを上⇔下入れ替え
                if (UI_Sand[0].fillOrigin == (int)Image.OriginVertical.Top)
                {
                    UI_Sand[0].fillOrigin = UI_Sand[1].fillOrigin = (int)Image.OriginVertical.Bottom;
                }
                else if (UI_Sand[0].fillOrigin == (int)Image.OriginVertical.Bottom)
                {
                    UI_Sand[0].fillOrigin = UI_Sand[1].fillOrigin = (int)Image.OriginVertical.Top;
                }

                OldPlayerTurnAnimFlg = true;
            }
        }

        //ゲージの増減
        float tmp_SandRate;     //最大と今の割合

        //プレイヤの正逆で分岐
        if (PlayerTurn)
        {
            tmp_SandRate = PLCon.GetPlayerBackTime() / SANDTIME_MAX;

            UI_Sand[1].fillAmount = tmp_SandRate;       //普通の割合
            UI_Sand[0].fillAmount = (1 - tmp_SandRate); //1-普通の割合
        }
        else
        {
            tmp_SandRate = PLCon.GetPlayerNormalTime() / SANDTIME_MAX;

            UI_Sand[0].fillAmount = tmp_SandRate;       //普通の割合
            UI_Sand[1].fillAmount = (1 - tmp_SandRate); //1-普通の割合
        }
    }
}
