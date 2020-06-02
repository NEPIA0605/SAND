using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class SSManager : MonoBehaviour
{
    //定数宣言
    private const int FIRST_STAGE = 0;
    private const int LAST_STAGE = 4;

    [Header("Stage")]
    public GameObject[] stages; //ステージオブジェクト

    [Header("UI")]
    public Image SSCursor;          //ステージ選択のカーソル
    private CursorMove SSCM;        //カーソル動かしコンポネ
    public Button OkButton;         //決定
    public Button WSBackButton;     //戻る

    public Text StageNameText;      //ステージ名表示テキスト

    private FadeManager FadeObj;    //フェードオブジェクト

    [Flags]
    private enum StageFlags
    {
        NoSF = 0,           //素
        Stage_1 = 1 << 0,   //00001
        Stage_2 = 1 << 1,   //00010
        Stage_3 = 1 << 2,   //00100
        Stage_4 = 1 << 3,   //01000
        Stage_5 = 1 << 4,   //10000
        AllSF = (1 << 0) | (1 << 1) | (1 << 2) | (1 << 3) | (1 << 4),  //全開放(デバッグ)用
    }

    [SerializeField] StageFlags sf;      //StageFlag格納

    [Header("現在選択されているステージ")]
    [SerializeField] public int NowSelStage;

    [Header("ワールドNo")]
    public int WorldNum;

    [Header("デバッグモードスイッチ")]
    public bool DMSwitch;           //下のをなんやかんやするスイッチ
    public static bool DebugMode;   //デバッグモードにするとワールドが全開放されます

    [SerializeField] AudioClip[] clips;//サウンド

    //SEです。
    protected AudioSource Source;

    // Start is called before the first frame update
    void Start()
    {
        if (DebugMode)
        {
            sf = StageFlags.AllSF;
        }
        else
        {
            //ステージのクリアデータを取得
            string data = "STAGE_FLAG_" + WorldNum.ToString();
            sf = (StageFlags)PlayerPrefs.GetInt(data, 0);
            Debug.Log("データ名：" + data);
        }

        //動かすコンポネ取得
        SSCM = SSCursor.GetComponent<CursorMove>();

        //フェードパネルとUIの親取得
        FadeObj = GameObject.Find("FadePanel").GetComponent<FadeManager>();

        //無条件解放
        stages[NowSelStage].GetComponent<StaUnl>().SetUnlockFlg(true);
        stages[NowSelStage].GetComponent<StaUnl>().OpenAnim();
        SSCM.transform.position = stages[NowSelStage].transform.position;

        //ステージ名設定
        StageNameText.text = "Stage " + WorldNum.ToString() + " - " + (NowSelStage + 1).ToString() + "\n" + stages[NowSelStage].GetComponent<StaUnl>().GetStageName();

        //サウンド
        Source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //フラグ解放
        FlgCheck(sf);

        //フェード中とカーソル移動中に入力できなくする処理
        if (FadeObj.GetFadeInFlg() == false && FadeObj.GetFadeOutFlg() == false && SSCM.GetMoveEnd())
        {
            //キー操作で操作できるようにする
            if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxisRaw("Horizontal") > 0) && GetNextUnlock())
            {
                //カーソル選択音
                Source.PlayOneShot(clips[0]);

                //次へ
                SSCM.GoNext();
            }
            else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxisRaw("Horizontal") < 0) && GetPrevUnlock())
            {
                //カーソル選択音
                Source.PlayOneShot(clips[0]);

                //前へ
                SSCM.GoPrev();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown("joystick button 0"))
            {
                //決定の際のSE
                Source.PlayOneShot(clips[1]);

                //決定
                SSGoSceneChange();
                //OkButton.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown("joystick button 1"))
            {
                //戻る際のSE
                Source.PlayOneShot(clips[2]);
                //戻る
                Debug.Log("b!");
                WSBackButton.onClick.Invoke();
            }
        }
    }

    //クリアフラグとアンロックフラグのチェック
    private void FlgCheck(StageFlags sf)
    {
        //Stage_1クリア済み
        if ((sf & StageFlags.Stage_1) == StageFlags.Stage_1)
        {
            stages[0].GetComponent<StaUnl>().SetClearFlg(true);
            stages[1].GetComponent<StaUnl>().SetUnlockFlg(true);
        }
        else
        {
            stages[0].GetComponent<StaUnl>().SetClearFlg(false);
            stages[1].GetComponent<StaUnl>().SetUnlockFlg(false);
        }

        //Stage_2クリア済み
        if ((sf & StageFlags.Stage_2) == StageFlags.Stage_2)
        {
            stages[1].GetComponent<StaUnl>().SetClearFlg(true);
            stages[2].GetComponent<StaUnl>().SetUnlockFlg(true);
        }
        else
        {
            stages[1].GetComponent<StaUnl>().SetClearFlg(false);
            stages[2].GetComponent<StaUnl>().SetUnlockFlg(false);
        }

        //Stage_3クリア済み
        if ((sf & StageFlags.Stage_3) == StageFlags.Stage_3)
        {
            stages[2].GetComponent<StaUnl>().SetClearFlg(true);
            stages[3].GetComponent<StaUnl>().SetUnlockFlg(true);
        }
        else
        {
            stages[2].GetComponent<StaUnl>().SetClearFlg(false);
            stages[3].GetComponent<StaUnl>().SetUnlockFlg(false);
        }

        if (WorldNum != 1)
        {
            //Stage_4クリア済み
            if ((sf & StageFlags.Stage_4) == StageFlags.Stage_4)
            {
                stages[3].GetComponent<StaUnl>().SetClearFlg(true);
                stages[4].GetComponent<StaUnl>().SetUnlockFlg(true);
            }
            else
            {
                stages[3].GetComponent<StaUnl>().SetClearFlg(false);
                stages[4].GetComponent<StaUnl>().SetUnlockFlg(false);
            }

            //Stage_5クリア済み
            if ((sf & StageFlags.Stage_5) == StageFlags.Stage_5)
            {
                stages[4].GetComponent<StaUnl>().SetClearFlg(true);
            }
            else
            {
                stages[4].GetComponent<StaUnl>().SetClearFlg(false);
            }
        }
        else
        {
            //ワールド１専用　Stage_4クリア済み
            if ((sf & StageFlags.Stage_4) == StageFlags.Stage_4)
            {
                stages[3].GetComponent<StaUnl>().SetClearFlg(true);
            }
            else
            {
                stages[3].GetComponent<StaUnl>().SetClearFlg(false);
            }
        }
    }

    //NowSelStageの設定、ワールド名の変更
    public void SetNowSelStage(int WorNo)
    {
        NowSelStage = NowSelStage + WorNo;
        StageNameText.text = "Stage " + WorldNum.ToString() + " - " + (NowSelStage + 1).ToString() + "\n" + 
                            stages[NowSelStage].GetComponent<StaUnl>().GetStageName();
    }

    //次のステージのGetter
    public bool GetNextUnlock()
    {
        if (NowSelStage == LAST_STAGE ||
            (WorldNum == 1 && (NowSelStage == (LAST_STAGE - 1))))
        {
            //最後のステージを選択していたら移動させない
            return false;
        }

        if (stages[NowSelStage + 1].GetComponent<StaUnl>().GetUnlockFlg())
        {
            //次のステージが解放されてたら動かす
            stages[NowSelStage].GetComponent<StaUnl>().CloseAnim();
            stages[NowSelStage + 1].GetComponent<StaUnl>().OpenAnim();
            return true;
        }
        else
        {
            //未開放時の動き
            stages[NowSelStage + 1].GetComponent<StaUnl>().LockedMove();
            return false;
        }
    }

    //前ステージのGetter
    public bool GetPrevUnlock()
    {
        if (NowSelStage == FIRST_STAGE)
        {
            //最初のステージを選択していたら移動させない
            return false;
        }

        if (stages[NowSelStage - 1].GetComponent<StaUnl>().GetUnlockFlg())
        {
            //次のステージが解放されてたら動かす
            stages[NowSelStage].GetComponent<StaUnl>().CloseAnim();
            stages[NowSelStage - 1].GetComponent<StaUnl>().OpenAnim();
            return true;
        }
        else
        {
            //ここには来ないはずなので。
            Debug.Log("SSManagerでバグ吐いとるで。(GetPrevUnlock)");
            return false;
        }
    }

    //次のステージの場所Getter
    public Vector3 GetNextPos()
    {
        return stages[NowSelStage + 1].transform.position;
    }

    //前ステージの場所Getter
    public Vector3 GetPrevPos()
    {
        return stages[NowSelStage - 1].transform.position;
    }

    //WorldNumのGetter
    public int GetWorldNum()
    {
        return WorldNum;
    }

    //シーンチェンジ
    public void SSGoSceneChange()
    {
        FadeObj.GetComponent<FadeManager>().FadeScene(stages[NowSelStage].GetComponent<StaUnl>().GetGoSceneNo());
    }

    //デバッグモード用
    private void OnValidate()
    {
        if (DMSwitch)
        {
            DebugMode = true;
        }
        else
        {
            DebugMode = false;
        }
    }
}
