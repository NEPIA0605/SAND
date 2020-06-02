using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptLRMove : MonoBehaviour
{
    [Header("移動時間"), SerializeField, Range(0, 10)]
    public float MoveTime;   //移動時間

    [SerializeField]
    private float NowTime;          //経過時間
    private Vector3 StartPos;       //開始位置
    private Vector3 EndPos;         //終了位置

    private bool MoveEnd;           //移動終了フラグ

    // Start is called before the first frame update
    void Start()
    {
        NowTime = 0.0f;
        MoveEnd = true;
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
                float x = Vector3.Lerp(StartPos, EndPos, rate).x;
                transform.position = new Vector3(x, transform.position.y, transform.position.z);
            }
            else if (NowTime > MoveTime)
            {
                this.transform.position = new Vector3(EndPos.x, transform.position.y, transform.position.z);
                MoveEnd = true;
            }
        }
    }

    //EndPosのSetter
    public void SetEndPosX(float x)
    {
        if (MoveEnd)
        {
            StartPos = this.transform.position;
            //終了位置計算
            EndPos.x = x;
            NowTime = 0.0f;
            MoveEnd = false;
        }
    }
}
