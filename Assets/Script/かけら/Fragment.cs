﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weight;

//サウンド用
[RequireComponent(typeof(AudioSource))]

public class Fragment : MonoBehaviour
{

    // プレイヤーのスクリプト
    public GameObject playercontroler;

    // 変数宣言
    public Vector3 FtStartPos;      // かけらの初期位置
    [SerializeField] Vector3 SandDir;                // 流砂の流れている方向を保存する変数
    [SerializeField] Vector3 Wall_Ft_ColSandDir;     // かけらか壁に当たった時に方向を保存する変数
    [SerializeField] Vector3 SandRot;                // 流砂の角度を取って縦か横かを判断する
    Vector3 FragmentMoveFt;         // かけらに乗って一緒に移動
    bool P_SandEnpflg;              // プレイヤーの中砂の有無
    [SerializeField] bool SandCol_X, SandCol_Y;      // 横の流砂・縦の流砂に触れているかどうか
    bool WallCol;                   // 壁に触れているかどうか
    bool WallColFt;                 // 壁に触れているかけらかどうか
    [SerializeField] bool Ft_Col;   // かけらに触れてるかどうか
    bool RespawnFlg;                // リスポーンに触れたかどうか
    bool P_FtCol;                   // プレイヤーがかけらにふれてるかどうか

    //サウンド用
    float time_SE;
    float time_MAX = 2;
    bool SE_Lag;
    bool Player_Hit;

    // 当たり判定
    Rigidbody rb;
    [SerializeField] private Vector3 localGravity;      // 重力を与える向きと力の強さ？

    [SerializeField] public Vector3 SandMoveFtSp;  // 流砂の移動力

    //サウンド用
    [SerializeField] AudioClip[] clips;
    protected AudioSource Source;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        playercontroler = GameObject.FindGameObjectWithTag("Player");

        FtStartPos = this.transform.position;
        SandDir = new Vector3(0.0f, 0.0f, 0.0f);
        Wall_Ft_ColSandDir　= new Vector3(0.0f, 0.0f, 0.0f);
        SandRot = new Vector3(0.0f, 0.0f, 0.0f);
        SandMoveFtSp = new Vector3(0.0f, 0.0f, 0.0f);
        SandCol_X = false;
        SandCol_Y = false;
        WallCol = false;
        WallColFt = false;
        Ft_Col = false;
        RespawnFlg = false;
        P_FtCol = false;

        P_SandEnpflg = playercontroler.GetComponent<PlayerControler>().GetPlayerEnpty(); ;

        rb = this.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        Source = GetComponent<AudioSource>();

        Player_Hit = false;
        SE_Lag = false;
        time_SE = 0;

    }
    // Update is called once per frame
    void Update()
    {

        // プレイヤーの中砂の有無を常にもってくる
        P_SandEnpflg = playercontroler.GetComponent<PlayerControler>().GetPlayerEnpty();
        P_FtCol = playercontroler.GetComponent<PlayerControler>().GetFtCol();


        // 流砂が地面張られているときは重力をかける
        if (SandCol_X == true)
        {
            localGravity.y = -30;
            this.GetComponent<Rigidbody>().useGravity = true;
        }
       
        // 流砂が壁に貼られているときは重力を切る
        if (SandCol_Y == true)
        {
            localGravity.y = 0;
            this.GetComponent<Rigidbody>().useGravity = false;
        }

        // 流砂に触れているときにプレイヤーが押そうとするとき流砂を超えるようにするためにポジションをあげる
        if(!SandCol_Y && !SandCol_X)
        {
            if (P_FtCol)
            {
                this.transform.position += new Vector3(0.0f, 0.011f, 0.0f);
            }
        }

       

        // 中砂がない時の処理
        if (P_SandEnpflg == true)
        {
            // 壁の流砂が止まったら欠片も止まる
            if (SandCol_X == false && SandCol_Y == true)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            // 流砂に触れているときに少し下に力を加えることで流砂の影響を受けれるようにする
            if ((SandCol_X) || (SandCol_Y))
            {
                //this.transform.Translate(0.0f, -0.0001f, 0.0f);

                if (P_FtCol == false)
                {
                    this.transform.Translate(0.0f, -0.0001f, 0.0f);
                }
            }
        }

        //サウンド用
        //if (Player_Hit)
        //{
        //    if (!GetComponent<Rigidbody>().IsSleeping())
        //    {
        //        //欠片が押されている際のSE
        //        Source.PlayOneShot(clips[0]);
        //    }

        //}
        //サウンド用改
        if ((Player_Hit) && (!SE_Lag))
        {
            if (!GetComponent<Rigidbody>().IsSleeping())
            {
                SE_Lag = true;
                //欠片が押されている際のSE
                Source.PlayOneShot(clips[0]);
            }
        }
        if (SE_Lag)
        {
            time_SE += Time.deltaTime;
        }
        if (time_SE >= time_MAX)
        {
            time_SE = 0;
            SE_Lag = false;
        }

    }

    private void FixedUpdate()
    {
        SetLocalGravity(); //重力をAddForceでかけるメソッドを呼ぶ。
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Wall")
        {
            WallCol = true;
            Wall_Ft_ColSandDir = SandMoveFtSp;
        }

        if (collision.gameObject.tag == "Fragment")
        {
            Ft_Col = true;
            Wall_Ft_ColSandDir = SandMoveFtSp;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // 流砂の上にいるときに流砂の移動力を受け取る
        if (collision.gameObject.tag == "QuickSand_B")
        {
            // 流砂の角度を取得（床かそうじゃないかを判別）
            SandRot = collision.transform.localEulerAngles;

            // 流砂の移動量を取得
            SandMoveFtSp = collision.gameObject.GetComponent<Quicksand>().GetSandMove();
            SandMoveFtSp /= 50;
            SandDir = SandMoveFtSp;

            // 流砂平面かどうか
            if (SandRot == new Vector3(0.0f, 0.0f, 0.0f))
            {
                SandCol_X = true;

                if ((WallCol) || (Ft_Col))
                {

                    if (Wall_Ft_ColSandDir == SandMoveFtSp) 
                    {
                        SandMoveFtSp = new Vector3(0.0f, 0.0f, 0.0f);
                    }
                    else
                    {
                        SandMoveFtSp = collision.gameObject.GetComponent<Quicksand>().GetSandMove();
                        SandMoveFtSp /= 50;

                    }

                }

            }
            // 流砂が平面じゃない
            else
            {
                SandCol_Y = true;
                SandDir = new Vector3(0.0f, SandDir.y, 0.0f);

                if (SandCol_X && SandDir.y < 0.0f)
                {
                    SandMoveFtSp = new Vector3(0.0f, 0.0f, 0.0f);

                }
                else
                {
                    SandMoveFtSp = collision.gameObject.GetComponent<Quicksand>().GetSandMove();
                    SandMoveFtSp /= 50;

                }

            }

            this.transform.Translate(SandMoveFtSp);

        }

        // 無視砂の処理
        if (collision.gameObject.tag == "Mud")
        {

            // 流砂の角度を取得（床かそうじゃないかを判別）
            SandRot = collision.transform.localEulerAngles;

            // 流砂の移動量を取得
            SandMoveFtSp = collision.gameObject.GetComponent<FlowingSand>().GetFlowingSandMove();
            SandMoveFtSp /= 50;
            SandDir = SandMoveFtSp;

            // 流砂平面かどうか
            if (SandRot == new Vector3(0.0f, 0.0f, 0.0f))
            {
                SandCol_X = true;

                if ((WallCol) || (Ft_Col))
                {

                    if (Wall_Ft_ColSandDir == SandMoveFtSp)
                    {
                        SandMoveFtSp = new Vector3(0.0f, 0.0f, 0.0f);
                    }
                    else
                    {
                        SandMoveFtSp = collision.gameObject.GetComponent<FlowingSand>().GetFlowingSandMove();
                        SandMoveFtSp /= 50;

                    }

                }

            }
            // 流砂が平面じゃない
            else
            {
                SandCol_Y = true;
                SandDir = new Vector3(0.0f, SandDir.y, 0.0f);

                if (SandCol_X && SandDir.y < 0.0f)
                {
                    SandMoveFtSp = new Vector3(0.0f, 0.0f, 0.0f);

                }
                else
                {
                    SandMoveFtSp = collision.gameObject.GetComponent<FlowingSand>().GetFlowingSandMove();
                    SandMoveFtSp /= 50;

                }

            }

            this.transform.Translate(SandMoveFtSp);
        }

        // かけらがかけらに触れているときの処理
        if (collision.gameObject.tag == "Fragment")
        {

            // このかけらが流砂に触れていないとき
            if ((SandCol_X == false) && (SandCol_Y == false))
            {
                // 下にあるかけらの流砂での速度・方向を持ってくる
                FragmentMoveFt = collision.gameObject.GetComponent<Fragment>().GetSandMoveFtSp();

                // かけらが流砂にふれているかどうかを持ってくる
                bool Ft_SandCol_X = collision.gameObject.GetComponent<Fragment>().GetSandCol_X();
                bool Ft_SandCol_Y = collision.gameObject.GetComponent<Fragment>().GetSandCol_Y();

                // かけらが壁に触れているかどうかを持ってくる
                WallColFt = collision.gameObject.GetComponent<Fragment>().GetWallCol();

                // 壁に触れているかけらかどうかを持ってくる
                bool Ft_Wall_Ft = collision.gameObject.GetComponent<Fragment>().GetWallColFt();

                // かけらが地面の流砂に触れているときの処理
                if (Ft_SandCol_X)
                {
                    Ft_Col = true;
                    this.transform.Translate(FragmentMoveFt);

                    // かけらが壁に触れている・壁に触れているかけらに乗っているときに流れないようにする。
                    if ((WallColFt) || (Ft_Wall_Ft))
                    {
                        FragmentMoveFt = new Vector3(0.0f, 0.0f, 0.0f);
                    }
                }
                // 壁の流砂に触れたら重力をかけ、上に飛ばないように
                if (Ft_SandCol_Y)
                {
                    this.GetComponent<Rigidbody>().useGravity = false;
                    FragmentMoveFt = new Vector3(0.0f, 0.0f, 0.0f);
                }
            }
            // かけらが流砂に触れているとき
            else
            {
                // かけらが壁に触れているかどうかを持ってくる
                WallColFt = collision.gameObject.GetComponent<Fragment>().GetWallCol();
            }
        }

        if (collision.gameObject.tag == "Wall")
        {
            WallCol = true;
            SandMoveFtSp = new Vector3(0.0f, 0.0f, 0.0f);
        }

        //サウンド用
        if (collision.gameObject.tag == "Player")
        {
            Player_Hit = true;
        }
        else
        {
            Player_Hit = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // かけらが落下したときに初期に戻る
        if (other.gameObject.tag == "fallcol")
        {
            RespawnFlg = true;
            FragmentMoveFt = new Vector3(0.0f, 0.0f, 0.0f);
            this.transform.position = FtStartPos;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        //流砂から流砂へ移動するときに一旦SandMobeFtSpを初期化する
        if (collision.gameObject.tag == "QuickSand_B")
        {
            if (SandCol_X == true)
            {
                SandCol_X = false;
            }

            if (SandCol_Y == true)
            {
                SandCol_Y = false;
                localGravity.y = -30;
            }
            this.GetComponent<Rigidbody>().useGravity = true;
            SandMoveFtSp = new Vector3(0.0f, 0.0f, 0.0f);
            SandRot = new Vector3(0.0f, 0.0f, 0.0f);
            SandDir = new Vector3(0.0f, 0.0f, 0.0f);
        }

        //流砂から流砂へ移動するときに一旦SandMobeFtSpを初期化する
        if (collision.gameObject.tag == "Mud")
        {
            if (SandCol_X == true)
            {
                SandCol_X = false;
            }

            if (SandCol_Y == true)
            {
                SandCol_Y = false;
                localGravity.y = -30;
            }
            this.GetComponent<Rigidbody>().useGravity = true;
            SandMoveFtSp = new Vector3(0.0f, 0.0f, 0.0f);
            SandRot = new Vector3(0.0f, 0.0f, 0.0f);
            SandDir = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if (collision.gameObject.tag == "Wall")
        {
            WallCol = false;
            Wall_Ft_ColSandDir = new Vector3(0.0f, 0.0f, 0.0f);
        }

   

        if (collision.gameObject.tag == "Fragment")
        {
            Ft_Col = false;
            Wall_Ft_ColSandDir = new Vector3(0.0f, 0.0f, 0.0f);
            FragmentMoveFt = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    private void SetLocalGravity()
    {
        rb.AddForce(localGravity, ForceMode.Acceleration);
    }


    public Vector3 GetSandMoveFtSp()
    {
        return SandMoveFtSp;
    }
    public bool GetSandCol_X()
    {
        return SandCol_X;
    }
    public bool GetSandCol_Y()
    {
        return SandCol_Y;
    }

    public bool GetWallCol()
    {
        return WallCol;
    }

    public bool GetWallColFt()
    {
        return WallColFt;
    }

    public bool GetFt_Col()
    {
        return Ft_Col;
    }

    public Vector3 GetFragmentMoveFt()
    {
        return FragmentMoveFt;
    }
}
