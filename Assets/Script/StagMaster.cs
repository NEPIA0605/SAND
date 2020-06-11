using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagMaster : MonoBehaviour
{
    [Header("上下運動")]
    [Tooltip("上下運動を行うか")]
    public bool     UpDownMove;
    [Header("移動量：(ランダムの最大と最小)")]
    public float    Max;
    public float    Min;
    [Header("移動にかかるフレーム数(1sec = 60fps)")]
    public int      MoveFrame;
    [Header("開始時の上昇：(初期設定用)")]
    [Tooltip("チェックを入れて開始すると上昇から始まります")]
    public bool     MoveUp;

    //他パラメータ
    private float   InitPosY;               //始点
    private float   StaPosY;                //切り替え時の始点
    private float   EndPosY;                //終点
    private float   MoveDist;               //移動量
    private float   NowFrame = 0;           //今のフレーム数
    private float   AllFrame;               //総フレーム数
    private bool    FirstSwitch = true;     //初回の切り替え

    [Header("回転運動を行うか")]
    public bool     RotateMove;
    [Header("回転軸の指定")]
    public bool     x;
    public bool     y;
    public bool     z;
    [Header("回転速度")]
    public float    Speed = 1.0f;

    [Header("デバッグ回数")]
    public int Dnum = 0;

    // Start is called before the first frame update
    void Start()
    {
        //上下運動
        //初期座標
        StaPosY = this.transform.position.y;
        InitPosY = StaPosY;
        //最初の終点座標を決める
        MoveDist = Random.Range(Min, Max);  //移動距離
        if(MoveUp)
        {
            //開始時上昇
            EndPosY = StaPosY + MoveDist;
        }
        else
        {
            //開始時下降
            EndPosY = StaPosY - MoveDist;
        }
        //実際に使うのはAllFrameなので
        AllFrame = (float)MoveFrame;

        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        //上下運動
        if(UpDownMove)
        {
            //時間の割合を出す
            float t = Mathf.Min(NowFrame / AllFrame, 1.0f);
            float leapt = (t * t) * (3.0f - (2.0f * t));    //ゆっくり動き出してゆっくり止まる公式：(t^2(3 - 2t))
            
            if(MoveUp)
            {
                //上昇時
                if (t < 1.0f)
                {
                    this.transform.position = new Vector3(this.transform.position.x, Mathf.Lerp(InitPosY, EndPosY, leapt), this.transform.position.z);
                }
                else if (t == 1.0f)
                {
                    //念のため補正
                    this.transform.position = new Vector3(this.transform.position.x, EndPosY, this.transform.position.z);
                    //最初の切り替え
                    if(FirstSwitch)
                    {
                        AllFrame   *= 2;  //片道分を2倍にする
                        FirstSwitch = false;
                    }

                    //リセット
                    NowFrame = 0;
                    InitPosY  = this.transform.position.y;
                    MoveDist = Random.Range(Min, Max);
                    EndPosY  = StaPosY - MoveDist;
                    MoveUp   = false;
                    Dnum++;
                }
            }
            else
            {
                //下降時
                if (t < 1.0f)
                {
                    this.transform.position = new Vector3(this.transform.position.x, Mathf.Lerp(InitPosY, EndPosY, leapt), this.transform.position.z);
                }
                else if (t == 1.0f)
                {
                    //念のため補正
                    this.transform.position = new Vector3(this.transform.position.x, EndPosY, this.transform.position.z);
                    //最初の切り替え
                    if (FirstSwitch)
                    {
                        AllFrame *= 2;  //片道分を2倍にする
                        FirstSwitch = false;
                    }

                    //リセット
                    NowFrame = 0;
                    InitPosY = this.transform.position.y;
                    MoveDist = Random.Range(Min, Max);
                    EndPosY = StaPosY + MoveDist;
                    MoveUp = true;
                    Dnum++;
                }
            }

            //時間を進める
            NowFrame++;
        }

        //回転運動
        if(RotateMove)
        {
            Vector3 rot_tmp = new Vector3();
            if(x)
            {
                rot_tmp.x = Speed;
            }

            if(y)
            {
                rot_tmp.y= Speed;
            }

            if (z)
            {
                rot_tmp.z = Speed;
            }

            this.transform.Rotate(rot_tmp);
        }


    }

    //最低値とかを決めとく
    private void OnValidate()
    {
        //MoveFrame 最低値：1
        if (MoveFrame <= 0)
        {
            MoveFrame = 1;
        }

        //Max 初期値：1
        if (Max <= 0.0f)
        {
            Max = 1.0f;
        }

        //Speed 初期値：1
        if (Speed <= 0.0f)
        {
            Speed = 1.0f;
        }
    }
}
