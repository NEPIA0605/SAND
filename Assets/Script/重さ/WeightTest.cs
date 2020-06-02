using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weight;

public class WeightTest : MonoBehaviour
{
    //このオブジェクトの重さ
    public WEIGHT weight;

    //テスト用のやつ
    public WEIGHT test1;
    public WEIGHT test2;

    // Start is called before the first frame update
    void Start()
    {
        weight = WEIGHT.WT_PLAYER;

        test1 = WEIGHT.WT_PLAYER;
        test2 = WEIGHT.WT_FR_RUINS;
    }

    // Update is called once per frame
    void Update()
    {
        //テストプログラム
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("WEIGHT：" + weight);
            Debug.Log("test1：" + test1);
            Debug.Log("test2：" + test2);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("比較（１，２）結果：" + Hikaku(test1, test2));
            Debug.Log("比較（２，１）結果：" + Hikaku(test2, test1));
        }
    }

    //比較する関数
    public COM_RESULT Hikaku(WEIGHT one,WEIGHT two)
    {
        //同じ
        if(one == two)
        {
            return COM_RESULT.CR_EQUAL;
        }
        //引数１が大きい
        else if(one > two)
        {
            return COM_RESULT.CR_ONE;
        }
        //引数２が大きい
        else
        {
            return COM_RESULT.CR_TWO;
        }
    }
}
