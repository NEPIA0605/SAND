using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OptCorsorMove : MonoBehaviour
{
    //定数宣言
    private const int STAY = 0;
    private const int NEXT = 1;
    private const int PREV = -1;

    [Header("移動時間"), SerializeField, Range(0, 10)]
    public float MoveTime;   //移動時間

    [SerializeField]
    private float NowTime;          //経過時間
    private Vector3 StartPos;       //開始位置
    private Vector3 EndPos;         //終了位置

    private bool MoveEnd;           //移動終了フラグ
    private int MoveNP;             //+1：Next　-1：Prev

    public OptionManager OptMObj;   //ステージセレクトマネージャーのオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        NowTime = 0.0f;
        MoveEnd = true;
        OptMObj = GameObject.FindWithTag("OptionManager").GetComponent<OptionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!MoveEnd)
        {
            if (NowTime <= MoveTime)
            {
                NowTime = Time.deltaTime + NowTime;

                //進行度で位置を決める
                var rate = NowTime / MoveTime;
                transform.position = Vector3.Lerp(StartPos, EndPos, rate);
            }
            else if (NowTime > MoveTime)
            {
                this.transform.position = EndPos;
                //選択の更新
                OptMObj.SetNowSelOpt(MoveNP);
                MoveNP = STAY;

                MoveEnd = true;
            }
        }
    }

    //右に移動(Next)
    public void GoNext()
    {
        if (MoveEnd && OptMObj.GetNextOpt())
        {
            StartPos = this.transform.position;
            //終了位置計算
            EndPos = OptMObj.GetNextPos();
            NowTime = 0.0f;
            MoveNP = NEXT;
            MoveEnd = false;
        }
    }

    //左に移動(Prev)
    public void GoPrev()
    {
        if (MoveEnd && OptMObj.GetPrevOpt())
        {
            StartPos = this.transform.position;
            //終了位置計算
            EndPos = OptMObj.GetPrevPos();
            NowTime = 0.0f;
            MoveNP = PREV;
            MoveEnd = false;
        }
    }

    //MoveEndのGetter
    public bool GetMoveEnd()
    {
        return MoveEnd;
    }

    //移動時間の最低値を決めとく
    private void OnValidate()
    {
        if (MoveTime <= 0)
        {
            MoveTime = 0.1f;
        }
    }
}
