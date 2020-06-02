using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShaderScript : MonoBehaviour
{
    //定数定義
    private const float TIME_MAX = 5.0f;            //時間の最大値
    private const float TIME_AJUST = 1.0f;          //調整

    [Header("色範囲(最大値)")]
    [Range(0.0f, 5.0f)] public float ColRangeMAX;   //時間が最大の時の色塗り範囲

    [Header("各種パラメータ")]
    public Color SandColor;                         //中砂の色
    public GameObject Player;                       //プレイヤ

    [Header("以下触らんでいいやつ")]
    public float NowTime;                           //今の時間
    public Vector3 PLWorldPos;                      //プレイヤのワールド座標

    public float TimeRate;                          //今の時間：最大値　の比率(割合)
    private PlayerControler PLCon;                  //プレイヤのコントローラ
    public Material SandMat;                        //マテリアル

    // Start is called before the first frame update
    void Start()
    {
        //コントローラとマテリアル取得
        PLCon = Player.GetComponent<PlayerControler>();
        SandMat = this.GetComponent<Renderer>().material;

        //中砂の色設定
        SandMat.SetColor("_SandColor", SandColor);
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤの反転状態によってどっちかの時間を取る
        if (PLCon.GetPlayerTurn())
        {
            //逆状態
            NowTime = PLCon.GetPlayerBackTime();
        }
        else
        {
            //通常状態
            NowTime = PLCon.GetPlayerNormalTime();
        }

        TimeRate = NowTime / TIME_MAX;               //今の時間と最大値の比率を取る
        PLWorldPos = this.transform.position;    //プレイヤの座標を取る

        //パラメータをシェーダ側に設定
        SandMat.SetFloat("_ColRange", (ColRangeMAX * TimeRate));
        if(PLCon.GetPlayerEnpty())
        {
            SandMat.SetFloat("_IsStop", 1.0f);
        }
        else
        {
            SandMat.SetFloat("_IsStop", 0.0f);
        }
    }
}
