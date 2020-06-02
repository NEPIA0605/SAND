using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandVecCal : MonoBehaviour
{
    [Tooltip("パラメータ")]
    public Vector3      SandVec;     //砂のベクトル
    public float        Speed;       //速度
    public GameObject   Player;      //プレイヤー
    public bool         IsStop;      //停止中か否か

    [Tooltip("デバッグモードスイッチ")]
    [SerializeField] private bool DebugMode;    //デバッグモードスイッチ

    // Start is called before the first frame update
    void Start()
    {
        if (!DebugMode)
        {
            //プレイヤ取得
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!DebugMode)
        {
            //方向取得
            SandVec = this.GetComponent<Quicksand>().GetSandDir();
            //スピード取得
            Speed = this.GetComponent<Quicksand>().GetSandSpeed();
            //停止フラグ
            IsStop = Player.GetComponent<PlayerControler>().GetPlayerEnpty();
        }
        //デバッグモード
        else
        {
            if(Input.GetKeyDown(KeyCode.Alpha0))
            {
                if(IsStop)
                {
                    IsStop = false;
                }
                else
                {
                    IsStop = true;
                }
            }
        }

        //ベクトルをラジアン角に変換して送る
        //yが0じゃない場合
        if(SandVec.y != 0.0f)
        {
            if(Mathf.Abs(SandVec.x) == Mathf.Abs(SandVec.z))
            {
                //テスト完了
                this.GetComponent<Renderer>().material.SetFloat("_Angle", Mathf.Atan2(-SandVec.x, SandVec.y));
            }
            else
            {
                //テスト完了
                //大きい方を使う
                this.GetComponent<Renderer>().material.SetFloat("_Angle", Mathf.Atan2(AbsMax(-SandVec.x, SandVec.z), SandVec.y));
            }
        }
        //yが0の場合
        else if(SandVec.y == 0.0f)
        {
            //テスト完了
            this.GetComponent<Renderer>().material.SetFloat("_Angle", Mathf.Atan2(-SandVec.x, SandVec.z));
        }

        //スピードと停止フラグを設定
        this.GetComponent<Renderer>().material.SetFloat("_Speed", Speed);
        
        if(IsStop || (SandVec.x == 0.0f && SandVec.y == 0.0f && SandVec.z == 0.0f))
        {
            this.GetComponent<Renderer>().material.SetFloat("_IsStop", 1.0f);
        }
        else
        {
            this.GetComponent<Renderer>().material.SetFloat("_IsStop", 0.0f);
        }
    }

    //絶対値が大きい方を返す関数
    private float AbsMax(float x,float y)
    {
        float tmp_x = Mathf.Abs(x);
        float tmp_y = Mathf.Abs(y);

        if(tmp_x > tmp_y)
        {
            return x;
        }
        else if(tmp_x < tmp_y)
        {
            return y;
        }

        return x;
    }

    private void OnValidate()
    {
        //デバッグ用
        if (DebugMode)
        {
            //SandDirの値を設定するときの上限設定
            {
                //x
                if (SandVec.x > 1.0f)
                {
                    SandVec.x = 1.0f;
                }
                else if (SandVec.x < -1.0f)
                {
                    SandVec.x = -1.0f;
                }

                //y
                if (SandVec.y > 1.0f)
                {
                    SandVec.y = 1.0f;
                }
                else if (SandVec.y < -1.0f)
                {
                    SandVec.y = -1.0f;
                }

                //z
                if (SandVec.z > 1.0f)
                {
                    SandVec.z = 1.0f;
                }
                else if (SandVec.z < -1.0f)
                {
                    SandVec.z = -1.0f;
                }
            }
        }
    }
}
