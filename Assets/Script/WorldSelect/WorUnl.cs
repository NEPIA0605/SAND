using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorUnl : MonoBehaviour
{
    [Header("Flags")]
    public bool UnlockFlg;    //アンロックフラグ
    public bool ClearFlg;     //クリアフラグ

    [Header("Name")]
    public string WorldName;

    [Header("Scene")]
    public int GoSceneNo;   //行先のシーン番号

    // Start is called before the first frame update
    void Start()
    {
        UnlockFlg = ClearFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (UnlockFlg)
        {
            if (ClearFlg)
            {
                //クリアしてたら緑にする
                //this.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                //アンロックだけなら青くする
                //this.GetComponent<Renderer>().material.color = Color.blue;
            }
        }
        else
        {
            //アンロックされていない場合は赤くしてクリアフラグを切る
            //this.GetComponent<Renderer>().material.color = Color.red;
            ClearFlg = false;
        }
    }

    //UnlockフラグのSetter
    public void SetUnlockFlg(bool bU)
    {
        UnlockFlg = bU;
    }

    //ClearフラグのSetter
    public void SetClearFlg(bool bC)
    {
        ClearFlg = bC;
    }

    //UnlockフラグのGetter
    public bool GetUnlockFlg()
    {
        return UnlockFlg;
    }

    //ClearフラグのGetter
    public bool GetClearFlg()
    {
        return ClearFlg;
    }

    //遷移先NoのGetter
    public int GetGoSceneNo()
    {
        return GoSceneNo;
    }

    //ワールド名のGetter
    public string GetWorldName()
    {
        return WorldName;
    }
}
