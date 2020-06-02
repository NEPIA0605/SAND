using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingSandVecCal : MonoBehaviour
{
    [Tooltip("パラメータ")]
    public Vector3 FlowingSandVec;     //砂のベクトル
    public float FlowingSpeed;       //速度
    public GameObject Player;      //プレイヤー

    [Tooltip("デバッグモードスイッチ")]
    [SerializeField] private bool FlowingDebugMode;    //デバッグモードスイッチ

    // Start is called before the first frame update
    void Start()
    {
        if (!FlowingDebugMode)
        {
            //プレイヤ取得
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!FlowingDebugMode)
        {
            //方向取得
            FlowingSandVec = this.GetComponent<FlowingSand>().GetFlowingSandDir();
            //スピード取得
            FlowingSpeed = this.GetComponent<FlowingSand>().GetFlowingSandSpeed();
        }

        //ベクトルをラジアン角に変換して送る
        //yが0じゃない場合
        if (FlowingSandVec.y != 0.0f)
        {
            if (Mathf.Abs(FlowingSandVec.x) == Mathf.Abs(FlowingSandVec.z))
            {
                //テスト完了
                this.GetComponent<Renderer>().material.SetFloat("_Angle", Mathf.Atan2(-FlowingSandVec.x, FlowingSandVec.y));
            }
            else
            {
                //テスト完了
                //大きい方を使う
                this.GetComponent<Renderer>().material.SetFloat("_Angle", Mathf.Atan2(AbsMax(-FlowingSandVec.x, FlowingSandVec.z), FlowingSandVec.y));
            }
        }
        //yが0の場合
        else if (FlowingSandVec.y == 0.0f)
        {
            //テスト完了
            this.GetComponent<Renderer>().material.SetFloat("_Angle", Mathf.Atan2(-FlowingSandVec.x, FlowingSandVec.z));
        }

        //スピードと停止フラグを設定
        this.GetComponent<Renderer>().material.SetFloat("_Speed", FlowingSpeed);

        if (FlowingSandVec.x == 0.0f && FlowingSandVec.y == 0.0f && FlowingSandVec.z == 0.0f)
        {
            this.GetComponent<Renderer>().material.SetFloat("_IsStop", 1.0f);
        }
        else
        {
            this.GetComponent<Renderer>().material.SetFloat("_IsStop", 0.0f);
        }
    }

    //絶対値が大きい方を返す関数
    private float AbsMax(float x, float y)
    {
        float tmp_x = Mathf.Abs(x);
        float tmp_y = Mathf.Abs(y);

        if (tmp_x > tmp_y)
        {
            return x;
        }
        else if (tmp_x < tmp_y)
        {
            return y;
        }

        return x;
    }

    private void OnValidate()
    {
        //デバッグ用
        if (FlowingDebugMode)
        {
            //SandDirの値を設定するときの上限設定
            {
                //x
                if (FlowingSandVec.x > 1.0f)
                {
                    FlowingSandVec.x = 1.0f;
                }
                else if (FlowingSandVec.x < -1.0f)
                {
                    FlowingSandVec.x = -1.0f;
                }

                //y
                if (FlowingSandVec.y > 1.0f)
                {
                    FlowingSandVec.y = 1.0f;
                }
                else if (FlowingSandVec.y < -1.0f)
                {
                    FlowingSandVec.y = -1.0f;
                }

                //z
                if (FlowingSandVec.z > 1.0f)
                {
                    FlowingSandVec.z = 1.0f;
                }
                else if (FlowingSandVec.z < -1.0f)
                {
                    FlowingSandVec.z = -1.0f;
                }
            }
        }
    }
}
