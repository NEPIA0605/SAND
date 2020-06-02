using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weight;


public class SandFragment : MonoBehaviour
{

    // プレイヤーのスクリプト
    public GameObject playercontroler;

    // 変数宣言
    public Vector3 FtStartPos;      // かけらの初期位置
    Vector3 SandDir;                // 流砂の向きを保存しておく変数
    bool P_SandEnpflg;              // プレイヤーの中砂の有無
    Vector3 P_Dir;                  // プレイヤーの向きを保存
    [SerializeField] bool P_FtColFrag;  // プレイヤーがかけらに当たっているかどうか
    [SerializeField] bool P_WallCol;    // プレイヤーが壁に触れているかどうか
    [SerializeField] bool Sft_WallCol;  // かけらが壁に触れているかどうか

    // 当たり判定
    Rigidbody rb;

    [SerializeField] Vector3 SandMoveFtSp;  // 流砂の移動力

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        playercontroler = GameObject.FindGameObjectWithTag("Player");

        FtStartPos = this.transform.position;
        SandDir = new Vector3(0.0f, 0.0f, 0.0f);
        SandMoveFtSp = new Vector3(0.0f, 0.0f, 0.0f);

        P_SandEnpflg = playercontroler.GetComponent<PlayerControler>().GetPlayerEnpty();
        P_FtColFrag = playercontroler.GetComponent<PlayerControler>().GetFtCol();
        P_WallCol = playercontroler.GetComponent<PlayerControler>().GetWallCol();
        Sft_WallCol = false;

        P_Dir = new Vector3(0.0f,0.0f,0.0f);

        rb = this.GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;

    }
    // Update is called once per frame
    void Update()
    {
        // プレイヤーの中砂の有無を常にもってくる
        P_SandEnpflg = playercontroler.GetComponent<PlayerControler>().GetPlayerEnpty();
        P_WallCol = playercontroler.GetComponent<PlayerControler>().GetWallCol();
        P_FtColFrag = playercontroler.GetComponent<PlayerControler>().GetFtCol();

    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            Sft_WallCol = true;
        }

        // 流砂の上にいるときに流砂の移動力を受け取る
        if (collision.gameObject.tag == "QuickSand_B")
        {
            SandMoveFtSp = collision.gameObject.GetComponent<Quicksand>().GetSandMove();
            SandMoveFtSp /= 50;

            // プレイヤーの中砂がないときの処理
            if (P_SandEnpflg == true)
            {
                // 中砂がないときに固定する
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
            // 中砂があるときの処理
            else
            {
                // 流砂が動いてるときだけ流砂の向きを保存しておく
                SandDir = SandMoveFtSp;

                // 流砂がｙ方向に力がかかっていなければ重力を付ける
                if (SandDir.y <= 0.0f)
                {
                    this.GetComponent<Rigidbody>().useGravity = true;
                }
                // 流砂がｙ方向に力がかかっていたら重力を切る
                else
                {
                    this.GetComponent<Rigidbody>().useGravity = false;
                }

                // プレイヤーの向いてる方向を保存
                P_Dir = playercontroler.GetComponent<PlayerControler>().GetPlayerDir();

                //プレイヤーが押せなくなる処理
                if (((P_Dir.x > 0.0f) && (SandDir.x > 0.0f) && !P_WallCol) ||
                    ((P_Dir.x < 0.0f) && (SandDir.x < 0.0f) && !P_WallCol) ||
                    ((P_Dir.z > 0.0f) && (SandDir.z > 0.0f) && !P_WallCol) ||
                    ((P_Dir.z < 0.0f) && (SandDir.z < 0.0f) && !P_WallCol))
                {
                    rb.constraints =
                    RigidbodyConstraints.FreezeAll;
                }
                else
                {
                    //プレイヤーが触れていないときの処理
                    if (SandDir.x != 0.0f)
                    {
                        rb.constraints =
                        RigidbodyConstraints.FreezePositionY |
                        RigidbodyConstraints.FreezePositionZ |
                        RigidbodyConstraints.FreezeRotation;
                    }

                    if (SandDir.y != 0.0f)
                    {
                        rb.constraints =
                        RigidbodyConstraints.FreezePositionX |
                        RigidbodyConstraints.FreezePositionZ |
                        RigidbodyConstraints.FreezeRotation;
                    }

                    if (SandDir.z != 0.0f)
                    {
                        rb.constraints =
                        RigidbodyConstraints.FreezePositionY |
                        RigidbodyConstraints.FreezePositionX |
                        RigidbodyConstraints.FreezeRotation;
                    }

                }

                // プレイヤーが流砂に逆らって押そうとするときに流砂の速度を速める（かけら）
                if (((P_Dir.x > 0.0f) && (SandDir.x < 0.0f) && P_FtColFrag) ||
                    ((P_Dir.x < 0.0f) && (SandDir.x > 0.0f) && P_FtColFrag) ||
                    ((P_Dir.z > 0.0f) && (SandDir.z < 0.0f) && P_FtColFrag) ||
                    ((P_Dir.z < 0.0f) && (SandDir.z > 0.0f) && P_FtColFrag))
                {
                    SandMoveFtSp *= 1.25f;
                }

                if (Sft_WallCol)
                {
                    //プレイヤーが触れていないときの処理
                    if (SandDir.x != 0.0f)
                    {
                        rb.constraints =
                        RigidbodyConstraints.FreezePositionY |
                        RigidbodyConstraints.FreezePositionZ |
                        RigidbodyConstraints.FreezeRotation;
                    }

                    if (SandDir.y != 0.0f)
                    {
                        rb.constraints =
                        RigidbodyConstraints.FreezePositionX |
                        RigidbodyConstraints.FreezePositionZ |
                        RigidbodyConstraints.FreezeRotation;
                    }

                    if (SandDir.z != 0.0f)
                    {
                        rb.constraints =
                        RigidbodyConstraints.FreezePositionY |
                        RigidbodyConstraints.FreezePositionX |
                        RigidbodyConstraints.FreezeRotation;
                    }
                }
            }
            this.transform.Translate(SandMoveFtSp);
            //rb.velocity = SandMoveFtSp;

        }

        // 無視砂の処理
        if (collision.gameObject.tag == "Mud")
        {
            SandMoveFtSp = collision.gameObject.GetComponent<FlowingSand>().GetFlowingSandMove();
            SandMoveFtSp /= 50;

            // 流砂が動いてるときだけ流砂の向きを保存しておく
            SandDir = SandMoveFtSp;

            // 流砂がｙ方向に力がかかっていなければ重力を付ける
            if (SandDir.y <= 0.0f)
            {
                this.GetComponent<Rigidbody>().useGravity = true;
            }
            // 流砂がｙ方向に力がかかっていたら重力を切る
            else
            {
                this.GetComponent<Rigidbody>().useGravity = false;
            }

            // プレイヤーの向いてる方向を保存
            P_Dir = playercontroler.GetComponent<PlayerControler>().GetPlayerDir();

            //プレイヤーが押せなくなる処理
            if (((P_Dir.x > 0.0f) && (SandDir.x > 0.0f) && !P_WallCol) ||
                ((P_Dir.x < 0.0f) && (SandDir.x < 0.0f) && !P_WallCol) ||
                ((P_Dir.z > 0.0f) && (SandDir.z > 0.0f) && !P_WallCol) ||
                ((P_Dir.z < 0.0f) && (SandDir.z < 0.0f) && !P_WallCol))
            {
                rb.constraints =
                RigidbodyConstraints.FreezeAll;
            }
            else
            {
                //プレイヤーが触れていないときの処理
                if (SandDir.x != 0.0f)
                {
                    rb.constraints =
                    RigidbodyConstraints.FreezePositionY |
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
                }

                if (SandDir.y != 0.0f)
                {
                    rb.constraints =
                    RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
                }

                if (SandDir.z != 0.0f)
                {
                    rb.constraints =
                    RigidbodyConstraints.FreezePositionY |
                    RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezeRotation;
                }

            }

            // プレイヤーが流砂に逆らって押そうとするときに流砂の速度を速める（かけら）
            if (((P_Dir.x > 0.0f) && (SandDir.x < 0.0f) && P_FtColFrag) ||
                ((P_Dir.x < 0.0f) && (SandDir.x > 0.0f) && P_FtColFrag) ||
                ((P_Dir.z > 0.0f) && (SandDir.z < 0.0f) && P_FtColFrag) ||
                ((P_Dir.z < 0.0f) && (SandDir.z > 0.0f) && P_FtColFrag))
            {
                SandMoveFtSp *= 1.25f;
            }

            if (Sft_WallCol)
            {
                //プレイヤーが触れていないときの処理
                if (SandDir.x != 0.0f)
                {
                    rb.constraints =
                    RigidbodyConstraints.FreezePositionY |
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
                }

                if (SandDir.y != 0.0f)
                {
                    rb.constraints =
                    RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
                }

                if (SandDir.z != 0.0f)
                {
                    rb.constraints =
                    RigidbodyConstraints.FreezePositionY |
                    RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezeRotation;
                }
            }
        }
        this.transform.Translate(SandMoveFtSp);

    }

    
    private void OnCollisionEnter(Collision other)
    {
        // かけらが落下したときに初期に戻る
        if (other.gameObject.tag == "fallcol")
        {
            this.transform.position = FtStartPos;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            Sft_WallCol = false;
        }

        //流砂から流砂へ移動するときに一旦SandMobeFtSpを初期化する
        if (collision.gameObject.tag == "QuickSand_B")
        {
            SandMoveFtSp = new Vector3(0.0f, 0.0f, 0.0f);
            this.GetComponent<Rigidbody>().useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;

        }
        //流砂から流砂へ移動するときに一旦SandMobeFtSpを初期化する
        if (collision.gameObject.tag == "Mud")
        {
            SandMoveFtSp = new Vector3(0.0f, 0.0f, 0.0f);
            this.GetComponent<Rigidbody>().useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;

        }

    }
}
