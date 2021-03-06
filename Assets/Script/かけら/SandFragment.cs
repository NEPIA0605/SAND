﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weight;


public class SandFragment : MonoBehaviour
{

    // プレイヤーのスクリプト
    public GameObject playercontroler;

    // 変数宣言
    public Vector3 FtStartPos;              // かけらの初期位置
    [SerializeField] Vector3 SandDir;       // 流砂の向きを保存しておく変数
    [SerializeField] Vector3 Wall_Ft_ColSandDir;     // かけらか壁に当たった時に方向を保存する変数
    Vector3 SandRot;                        // 流砂の角度を取って縦か横かを判断する
    bool P_SandEnpflg;                      // プレイヤーの中砂の有無
    public bool SandCol_X, SandCol_Y;              // 横の流砂・縦の流砂に触れているかどうか
    [SerializeField] bool P_FtColFrag;                       // プレイヤーがかけらに当たっているかどうか
    bool P_WallCol;                         // プレイヤーが壁に触れているかどうか
    bool Sft_WallCol;                       // かけらが壁に触れているかどうか
    bool Ft_Col;                            // かけらがかけらに触れているかどうか
    bool P_SandCol_X, P_SandCol_Y;          // プレイヤーがX・Y砂に触れているかどうか


    // 当たり判定
    [SerializeField] private Vector3 localGravity;      // 重力を与える向きと力の強さ？
    private Rigidbody rb;

    [SerializeField] Vector3 SandMoveFtSp;  // 流砂の移動力

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        playercontroler = GameObject.FindGameObjectWithTag("Player");

        FtStartPos = this.transform.position;
        SandDir = new Vector3(0.0f, 0.0f, 0.0f);
        Wall_Ft_ColSandDir = new Vector3(0.0f, 0.0f, 0.0f);
        SandMoveFtSp = new Vector3(0.0f, 0.0f, 0.0f);
        SandRot = new Vector3(0.0f, 0.0f, 0.0f);
        SandCol_X = false;
        SandCol_Y = false;
        Sft_WallCol = false;
        Ft_Col = false;
        P_SandCol_X = false;
        P_SandCol_Y = false;
        P_SandEnpflg = playercontroler.GetComponent<PlayerControler>().GetPlayerEnpty();
        P_FtColFrag = playercontroler.GetComponent<PlayerControler>().GetFtCol();
        P_WallCol = playercontroler.GetComponent<PlayerControler>().GetWallCol();


        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = true; //最初にrigidBodyの重力をかける

        rb.constraints = RigidbodyConstraints.FreezePositionX |
                         RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotation;

    }
    // Update is called once per frame
    void Update()
    {
        

        // プレイヤーの中砂の有無を常にもってくる
        P_SandEnpflg = playercontroler.GetComponent<PlayerControler>().GetPlayerEnpty();

        // プレイヤーが壁に当たっているかどうかを持ってくる
        P_WallCol = playercontroler.GetComponent<PlayerControler>().GetWallCol();

        // プレイヤーがかけらにふれているかどうか
        P_FtColFrag = playercontroler.GetComponent<PlayerControler>().GetFtCol();
        P_SandCol_X = playercontroler.GetComponent<PlayerControler>().GetPlayerSandCol_X();
        P_SandCol_Y = playercontroler.GetComponent<PlayerControler>().GetPlayerSandCol_Y();



        // 流砂が地面張られているときは重力をかける
        if (SandCol_X)
        {
            this.GetComponent<Rigidbody>().useGravity = true;
        }

        // 流砂が壁に貼られているときは重力を切る
        if (SandCol_Y == true)
        {
            this.GetComponent<Rigidbody>().useGravity = false;
        }

        //// 流砂に触れているときにプレイヤーが押そうとするとき流砂を超えるようにするためにポジションをあげる
        //if ((!SandCol_X) && (!SandCol_Y))
        //{
        //    localGravity.y = -10000;
        //}


        SetLocalGravity(); //重力をAddForceでかけるメソッドを呼ぶ。
        


        // 中砂がない時の処理
        if (P_SandEnpflg == true)
        {
            // 壁の流砂が止まったら欠片も止まる
            if (SandCol_X && SandCol_Y)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
        else
        {

            if(SandCol_X)
            {
                // ｘ方向にしか動かないようにする
                if (SandMoveFtSp.x != 0.0f)
                {
                    rb.constraints =
                //    RigidbodyConstraints.FreezePositionY |
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
                }

                // ｚ方向にしか動かないようにする
                if (SandMoveFtSp.z != 0.0f)
                {
                    rb.constraints =
             //       RigidbodyConstraints.FreezePositionY |
                    RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezeRotation;
                }


            }

            if (SandCol_Y)
            {
                // ｙ方向にしか動かないようにする
                if (SandMoveFtSp.y != 0.0f)
                {
                    rb.constraints =
                    RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
                }
            }


            // 流砂に触れているときに少し下に力を加えることで流砂の影響を受けれるようにする
            if ((SandCol_X) || (SandCol_Y))
            {
                //this.transform.Translate(0.0f, -0.0001f, 0.0f);

                if (P_FtColFrag == false)
                {
                    this.transform.Translate(0.0f, -0.0001f, 0.0f);
                }
            }
        }

    }
    //private void FixedUpdate()
    //{
    //    SetLocalGravity(); //重力をAddForceでかけるメソッドを呼ぶ。
    //}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Sft_WallCol = true;
            Wall_Ft_ColSandDir = SandMoveFtSp;
        }

        if (collision.gameObject.tag == "Fragment")
        {
            Ft_Col = true;
            Wall_Ft_ColSandDir = SandMoveFtSp;
        }
        if (collision.gameObject.tag == "SandFragment")
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
                localGravity.y = -1000;

                if ((Sft_WallCol) || (Ft_Col))
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
                localGravity.y = 0;
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


            if ((Sft_WallCol) || (Ft_Col))
            {
                if (SandDir == SandMoveFtSp)
                {
                    SandMoveFtSp = new Vector3(0.0f, 0.0f, 0.0f);
                }
                else
                {
                    SandMoveFtSp = collision.gameObject.GetComponent<Quicksand>().GetSandMove();
                    SandMoveFtSp /= 50;


                }
            }

            // ｘ方向にしか動かないようにする
            if (SandMoveFtSp.x != 0.0f)
            {
                rb.constraints =
                RigidbodyConstraints.FreezePositionY |
                RigidbodyConstraints.FreezePositionZ |
                RigidbodyConstraints.FreezeRotation;
            }

            // ｚ方向にしか動かないようにする
            if (SandMoveFtSp.z != 0.0f)
            {
                rb.constraints =
                RigidbodyConstraints.FreezePositionY |
                RigidbodyConstraints.FreezePositionX |
                RigidbodyConstraints.FreezeRotation;
            }

            // ｙ方向にしか動かないようにする
            if (SandMoveFtSp.y != 0.0f)
            {
                rb.constraints =
                RigidbodyConstraints.FreezePositionX |
                RigidbodyConstraints.FreezePositionZ |
                RigidbodyConstraints.FreezeRotation;
            }

            // 流砂平面かどうか
            if (SandRot == new Vector3(0.0f, 0.0f, 0.0f))
            {
                SandCol_X = true;

            }
            // 流砂が平面じゃない
            else
            {
                SandCol_Y = true;
                if (SandCol_X)
                {
                    if (SandMoveFtSp.y < 0.0f)
                    {
                        SandMoveFtSp.y = 0.0f;
                    }
                }
            }
            this.transform.Translate(SandMoveFtSp);
        }


    }

    private void OnTrrigerEnter(Collider other)
    {
        // かけらが落下したときに初期に戻る
        if (other.gameObject.tag == "fallcol")
        {
            this.transform.position = FtStartPos;
        }
    }

    // 重力をかける関数
    private void SetLocalGravity()
    {
        rb.AddForce(localGravity, ForceMode.Acceleration);
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Sft_WallCol = false;
            Wall_Ft_ColSandDir = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if (collision.gameObject.tag == "Player")
        {
            Ft_Col = false;
            SandDir = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if (collision.gameObject.tag == "Fragment")
        {
            Ft_Col = false;
            Wall_Ft_ColSandDir = new Vector3(0.0f, 0.0f, 0.0f);
        }
        if (collision.gameObject.tag == "SandFragment")
        {
            Ft_Col = false;
            Wall_Ft_ColSandDir = new Vector3(0.0f, 0.0f, 0.0f);
        }

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

    }

    public Vector3 GetSandMoveSFtSp()
    {
        return SandMoveFtSp;
    }

    public bool GetSftSandCol_X()
    {
        return SandCol_X;
    }
    public  bool GetSftSandCol_Y()
    {
        return SandCol_Y;
    }

    public bool GetSft_WallCol()
    {
        return Sft_WallCol;
    }

}



