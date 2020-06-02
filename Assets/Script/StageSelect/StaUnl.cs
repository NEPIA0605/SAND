using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaUnl : MonoBehaviour
{
    //定数
    private const int OP_TIME = 20;   //開けの時間
    private const int CL_TIME = 20;   //閉めの時間

    [Header("Flags")]
    public bool UnlockFlg;    //アンロックフラグ
    public bool ClearFlg;     //クリアフラグ

    [Header("Text & Model")]
    public Text ClearText;    //クリアテキスト
    public GameObject StageObj; //ステージオブジェクト
    public GameObject Locks;     //ロックされてる時の画像

    [Header("Name")]
    public string StageName;

    [Header("Scene")]
    public int GoSceneNo;   //行先のシーン番号

    [Header("他パラメータ")]
    private bool OpenFlg;   //開いた時の拡
    private bool CloseFlg;   //閉じた時の縮
    private int OpClCnt;     //拡縮の時間測り

    // Start is called before the first frame update
    void Start()
    {
        //UnlockFlg = ClearFlg = false;
        //見えないようにする
        this.GetComponent<RectTransform>().localScale = new Vector3(0.75f, 0.0f, 1.0f);
        Locks.GetComponent<LockLook>().SetWorldNum(this.transform.root.GetComponent<SSManager>().GetWorldNum());
    }

    // Update is called once per frame
    void Update()
    {
        if (UnlockFlg)
        {
            //アンロックなら鎖を消す
            Locks.transform.parent.gameObject.SetActive(false);
            if (ClearFlg)
            {
                //クリアしてたらクリアを出す
                ClearText.enabled = true;
            }
            else
            {
                ClearText.enabled = false;
            }
        }
        else
        {
            //アンロックされていない場合は上張りを張ってクリアフラグを切る
            Locks.transform.parent.gameObject.SetActive(true);
            ClearText.enabled = false;
            ClearFlg = false;
        }

        //開閉の時に拡縮する
        if(OpenFlg)
        {
            if (OpClCnt < OP_TIME)
            {
                //OPCL_TIMEフレームかけて拡大する
                this.GetComponent<RectTransform>().localScale += new Vector3(0.25f / OP_TIME, 1.0f / OP_TIME, 0.0f);
                OpClCnt++;
            }
            else
            {
                //拡大終了
                this.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                OpenFlg = false;
            }
        }
        else if (CloseFlg)
        {
            if (OpClCnt < CL_TIME)
            {
                //OPCL_TIMEフレームかけて縮小する
                this.GetComponent<RectTransform>().localScale -= new Vector3(0.25f / CL_TIME, 1.0f / CL_TIME, 0.0f);
                OpClCnt++;
            }
            else
            {
                //縮小終了
                this.GetComponent<RectTransform>().localScale = new Vector3(0.75f, 0.0f, 1.0f);
                CloseFlg = false;
            }
        }
    }

    //棺桶の開閉アニメーション
    //開
    public void OpenAnim()
    {
        //Debug.Log(StageName + "：Open");
        OpenFlg = true;
        CloseFlg = false;
        OpClCnt = 0;
        StageObj.GetComponent<Animator>().Play("Open");
    }

    //閉
    public void CloseAnim()
    {
        //Debug.Log(StageName + "：Close");
        OpenFlg = false;
        CloseFlg = true;
        OpClCnt = 0;
        StageObj.GetComponent<Animator>().Play("Close");
    }

    //ロックされているときの動き
    public void LockedMove()
    {
        Locks.GetComponent<LockLook>().LockMove();
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
    public string GetStageName()
    {
        return StageName;
    }
}
