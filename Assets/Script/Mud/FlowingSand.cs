using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingSand : MonoBehaviour
{
    [Header("流れ続ける流砂の流れの設定")]
    [Tooltip("x,y,z　それぞれに -1.0 ~ 1.0 の値を入れてください(超過対応済み)")]
    public Vector3 FlowingSandDir;           //流砂の正方向
    [Tooltip("ここは見る用の場所なので、値は入れないでください")]
    [SerializeField] Vector3 InvFlowingSandDir;        //流砂の逆方向

    [Tooltip("流れ続ける流砂が流れる速度を設定できます")]
    public float FlowingSandSpd;           //流れる速度

    [Header("その他")]
    [SerializeField] private Vector3 FlowingSandMove;  //流砂の移動力(方向 * 速度)
    [SerializeField] private bool IsFlowingTriggerFlg; //プレイヤのトリガーに当たっていたらtrue

    public GameObject playerControler;

    // Start is called before the first frame update
    void Start()
    {
        //逆ベクトルを保存
        InvFlowingSandDir = FlowingSandDir * -1.0f;

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
        //毎フレーム移動力を計算する
        FlowingSandMove = FlowingMoveCal();

        //Shader用にworld座標を送る
        this.GetComponent<Renderer>().material.SetVector("ObjWorldPos", this.gameObject.transform.position + (this.gameObject.transform.localScale / 2));
    }

    //移動力の計算をする関数
    private Vector3 FlowingMoveCal()
    {
        return (FlowingSandDir * FlowingSandSpd);
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
                IsFlowingTriggerFlg = true;
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
                IsFlowingTriggerFlg = false;
            }
        }
    }

    //移動力のGetter
    public Vector3 GetFlowingSandMove()
    {
        //流砂(板以外)
        if (this.tag == "QuickSand")
        {
            if (IsFlowingTriggerFlg)
            {
                //+のy成分を与えない
                if (FlowingSandMove.y >= 0.0f)
                {
                    return new Vector3(FlowingSandMove.x, 0.0f, FlowingSandMove.z);
                }
                else if (FlowingSandMove.y < 0.0f)
                {
                    //yが-でも、角度が付いてなければy成分を与えない
                    if (this.transform.eulerAngles.x == 0.0f &&
                       this.transform.eulerAngles.z == 0.0f)
                    {
                        return new Vector3(FlowingSandMove.x, 0.0f, FlowingSandMove.z);
                    }
                }
            }
            else if (!IsFlowingTriggerFlg)
            {
                //yが+の場合だけyを与える
                if (FlowingSandMove.y < 0.0f)
                {
                    return new Vector3(FlowingSandMove.x, 0.0f, FlowingSandMove.z);
                }
            }
        }
        //流砂(板)
        else if (this.tag == "Mud")
        {
            //角度で判断する
            if (this.transform.eulerAngles.x % 180.0f == 0.0f &&
            this.transform.eulerAngles.z % 180.0f == 0.0f)
            {
                return new Vector3(FlowingSandMove.x, 0.0f, FlowingSandMove.z);
            }
            else
            {
                if (FlowingSandMove.y == 0.0f)
                {
                    return FlowingSandMove;
                }
            }
        }

        return FlowingSandMove;
    }

    //方向のGetter
    public Vector3 GetFlowingSandDir()
    {
        return FlowingSandDir;
    }

    //方向のGetter
    public Vector3 GetRotFlowingSand()
    {
        return this.transform.eulerAngles;
    }

    //スピードのGetter
    public float GetFlowingSandSpeed()
    {
        return FlowingSandSpd;
    }

    //インスペクタで値が変更された時に発動する関数
    private void OnValidate()
    {
        //SandDirの値を設定するときの上限設定
        {
            //x
            if (FlowingSandDir.x > 1.0f)
            {
                FlowingSandDir.x = 1.0f;
            }
            else if (FlowingSandDir.x < -1.0f)
            {
                FlowingSandDir.x = -1.0f;
            }

            //y
            if (FlowingSandDir.y > 1.0f)
            {
                FlowingSandDir.y = 1.0f;
            }
            else if (FlowingSandDir.y < -1.0f)
            {
                FlowingSandDir.y = -1.0f;
            }

            //z
            if (FlowingSandDir.z > 1.0f)
            {
                FlowingSandDir.z = 1.0f;
            }
            else if (FlowingSandDir.z < -1.0f)
            {
                FlowingSandDir.z = -1.0f;
            }
        }

        //SandSpdの値が０以下にならないようにする
        if (FlowingSandSpd <= 1.0f)
        {
            FlowingSandSpd = 1.0f;
        }
    }
}