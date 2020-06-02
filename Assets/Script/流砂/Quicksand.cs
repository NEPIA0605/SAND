using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weight;

public class Quicksand : MonoBehaviour
{
    [Header("流砂の流れの設定")]
    [Tooltip("x,y,z　それぞれに -1.0 ~ 1.0 の値を入れてください(超過対応済み)")]
    public Vector3           SandDir;           //流砂の正方向
    [Tooltip("ここは見る用の場所なので、値は入れないでください")]
    [SerializeField] Vector3 InvSandDir;        //流砂の逆方向
    [Tooltip("流砂が流れる速度を設定できます")]
    public float             SandSpd;           //流れる速度

    [Header("各種フラグ")]
    [SerializeField] bool    IsInverse;         //流れの正逆判定(正：false、逆：true)
    [SerializeField] bool    IsStop;            //流れの停止判定(動：false、止：true)

    [Header("その他")]
    [SerializeField] private Vector3 SandMove;  //流砂の移動力(方向 * 速度)
    public static WEIGHT  SandWeight;           //重さ
    [SerializeField] private bool IsTriggerFlg; //プレイヤのトリガーに当たっていたらtrue

    public GameObject playerControler;

    // Start is called before the first frame update
    void Start()
    {
        //逆ベクトルを保存
        InvSandDir = SandDir * -1.0f;
        //正逆判定をfalse
        IsInverse = false;
        //停止判定をfalse
        IsStop = false;

        playerControler = GameObject.FindGameObjectWithTag("Player");

        //ついでに実装してみる機能(設置がしやすいかも)
        if (Mathf.Abs(this.transform.up.x) != 0.0f)
        {
            if (this.transform.up.x > 0.0f)
            {
                this.transform.position += new Vector3(0.01f, 0, 0);
            }
            else if (this.transform.up.x < 0.0f)
            {
                this.transform.position += new Vector3(-0.01f, 0, 0);
            }
        }
        if (Mathf.Abs(this.transform.up.y) != 0.0f)
        {
            if (this.transform.up.y > 0.0f)
            {
                this.transform.position += new Vector3(0, 0.01f, 0);
            }
            else if (this.transform.up.y < 0.0f)
            {
                this.transform.position += new Vector3(0, -0.01f, 0);
            }
        }
        if (Mathf.Abs(this.transform.up.z) != 0.0f)
        {
            if (this.transform.up.z > 0.0f)
            {
                this.transform.position += new Vector3(0, 0, 0.01f);
            }
            else if (this.transform.up.z < 0.0f)
            {
                this.transform.position += new Vector3(0, 0, -0.01f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの正逆、停止と同期する
        IsInverse = playerControler.GetComponent<PlayerControler>().GetPlayerTurn();
        IsStop = playerControler.GetComponent<PlayerControler>().GetPlayerEnpty();

        //毎フレーム移動力を計算する
        SandMove = MoveCal(IsInverse, IsStop);

        //Shader用にworld座標を送る
        this.GetComponent<Renderer>().material.SetVector("ObjWorldPos", this.gameObject.transform.position + (this.gameObject.transform.localScale / 2));
    }

    //移動力の計算をする関数
    private Vector3 MoveCal(bool inv,bool stop)
    {
        //止まっている状態
        if(stop)
        {
            return new Vector3(0.0f, 0.0f, 0.0f);
        }
        else
        {
            //正
            if(!inv)
            {
                return (SandDir * SandSpd);
            }
            //逆
            else
            {
                return (InvSandDir * SandSpd);
            }
        }
    }

    //プレイヤのトリガーに当たった時
    private void OnTriggerStay(Collider other)
    {
        //流砂(板以外)
        if (this.tag == "QuickSand")
        {
            //プレイヤのトリガーに反応
            if (other.gameObject.tag == "Player")
            {
                IsTriggerFlg = true;
            }
        }
    }

    //プレイヤのトリガーから離れた時
    private void OnTriggerExit(Collider other)
    {
        //流砂(板以外)
        if (this.tag == "QuickSand")
        {
            //プレイヤのトリガーに反応
            if (other.gameObject.tag == "Player")
            {
                IsTriggerFlg = false;
            }
        }
    }

    //移動力のGetter
    public Vector3 GetSandMove()
    {
        //流砂(板以外)
        if (this.tag == "QuickSand")
        {
            if (IsTriggerFlg)
            {
                //+のy成分を与えない
                if (SandMove.y >= 0.0f)
                {
                    return new Vector3(SandMove.x, 0.0f, SandMove.z);
                }
                else if (SandMove.y < 0.0f)
                {
                    //yが-でも、角度が付いてなければy成分を与えない
                    if (this.transform.eulerAngles.x == 0.0f &&
                       this.transform.eulerAngles.z == 0.0f)
                    {
                        return new Vector3(SandMove.x, 0.0f, SandMove.z);
                    }
                }
            }
            else if (!IsTriggerFlg)
            {
                //yが+の場合だけyを与える
                if (SandMove.y < 0.0f)
                {
                    return new Vector3(SandMove.x, 0.0f, SandMove.z);
                }
            }
        }
        //流砂(板)
        else if (this.tag == "QuickSand_B")
        {
            //角度で判断する
            if (this.transform.eulerAngles.x % 180.0f == 0.0f &&
            this.transform.eulerAngles.z % 180.0f == 0.0f)
            {
                return new Vector3(SandMove.x, 0.0f, SandMove.z);
            }
            else
            {
                if (SandMove.y == 0.0f)
                {
                    return SandMove;
                }
            }
        }
        return SandMove;
    }

    //方向のGetter
    public Vector3 GetSandDir()
    {
        if (!IsInverse)
        {
            return SandDir;
        }
        else
        {
            return InvSandDir;
        }
    }

    //スピードのGetter
    public float GetSandSpeed()
    {
        return SandSpd;
    }


    //方向のGetter
    public Vector3 GetRotSand()
    {
        return this.transform.eulerAngles;
    }

    //インスペクタで値が変更された時に発動する関数
    private void OnValidate()
    {
        //SandDirの値を設定するときの上限設定
        {
            //x
            if (SandDir.x > 1.0f)
            {
                SandDir.x = 1.0f;
            }
            else if (SandDir.x < -1.0f)
            {
                SandDir.x = -1.0f;
            }

            //y
            if (SandDir.y > 1.0f)
            {
                SandDir.y = 1.0f;
            }
            else if (SandDir.y < -1.0f)
            {
                SandDir.y = -1.0f;
            }

            //z
            if (SandDir.z > 1.0f)
            {
                SandDir.z = 1.0f;
            }
            else if (SandDir.z < -1.0f)
            {
                SandDir.z = -1.0f;
            }
        }

        //SandSpdの値が０以下にならないようにする
        if (SandSpd <= 1.0f)
        {
            SandSpd = 1.0f;
        }
    }
}
