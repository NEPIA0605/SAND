using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class WSManager : MonoBehaviour
{
    //定数宣言
    private const int FIRST_WORLD = 0;
    private const int LAST_WORLD = 3;

    private bool MoveEndFlg;

    [Header("World")]
    public GameObject[] worlds; //ワールドオブジェクト

    [Header("UI")]
    public Button NextAllow;        //次へ
    public Button PrevAllow;        //前へ
    public Button OkButton;         //決定
    public Button GoBackButton;     //戻る

    public Text WorldNameText;      //ワールド名表示テキスト

    private FadeManager FadeObj; //フェードオブジェクト

    [Flags]
    private enum WorldFlags
    {
        NoWF = 0,           //素
        World_1 = 1 << 0,   //0001
        World_2 = 1 << 1,   //0010
        World_3 = 1 << 2,   //0100
        World_4 = 1 << 3,   //1000
        AllWF = (1 << 0) | (1 << 1) | (1 << 2) | (1 << 3),  //全開放(デバッグ)用
    }

    [SerializeField] WorldFlags wf;      //WorldFlag格納

    [Header("現在選択されているワールド")]
    [SerializeField] public int NowSelWorld;

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
            wf = WorldFlags.AllWF;
        }
        else
        {
            //ワールドのクリアデータを取得
            wf = (WorldFlags)PlayerPrefs.GetInt("WORLD_FLAG", 0);
        }

        //フェードパネルとUIの親取得
        FadeObj = GameObject.Find("FadePanel").GetComponent<FadeManager>();

        //無条件解放
        worlds[0].GetComponent<WorUnl>().SetUnlockFlg(true);

        //ワールド名設定
        WorldNameText.text = "World-" + (NowSelWorld + 1).ToString() + "\n" + worlds[NowSelWorld].GetComponent<WorUnl>().GetWorldName();

        //サウンド
        Source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //フラグ解放
        FlgCheck(wf);

        //フェード中と選択中入力できなくする処理
        if (FadeObj.GetFadeInFlg() == false && FadeObj.GetFadeOutFlg() == false && MoveEndFlg)
        {
            //キー操作で操作できるようにする
            if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxisRaw("Horizontal") > 0) && GetNextUnlock())
            {
                //カーソル選択音
                Source.PlayOneShot(clips[0]);

                //次へ
                NextAllow.onClick.Invoke();
            }
            else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxisRaw("Horizontal") < 0) && GetPrevUnlock())
            {
                //カーソル選択音
                Source.PlayOneShot(clips[0]);

                //前へ
                PrevAllow.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown("joystick button 0"))
            {
                //決定の際のSE
                Source.PlayOneShot(clips[1]);
                //決定
                GoSceneChange();
                //OkButton.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown("joystick button 1"))
            {
                //戻る際のSE
                Source.PlayOneShot(clips[2]);

                //戻る
                Debug.Log("b!");
                GoBackButton.onClick.Invoke();
            }
        }
    }

    //クリアフラグとアンロックフラグのチェック
    private void FlgCheck(WorldFlags wf)
    {
        //World_1クリア済み
        if ((wf & WorldFlags.World_1) == WorldFlags.World_1)
        {
            worlds[0].GetComponent<WorUnl>().SetClearFlg(true);
            worlds[1].GetComponent<WorUnl>().SetUnlockFlg(true);
        }
        else
        {
            worlds[0].GetComponent<WorUnl>().SetClearFlg(false);
            worlds[1].GetComponent<WorUnl>().SetUnlockFlg(false);
        }

        //World_2クリア済み
        if ((wf & WorldFlags.World_2) == WorldFlags.World_2)
        {
            worlds[1].GetComponent<WorUnl>().SetClearFlg(true);
            worlds[2].GetComponent<WorUnl>().SetUnlockFlg(true);
        }
        else
        {
            worlds[1].GetComponent<WorUnl>().SetClearFlg(false);
            worlds[2].GetComponent<WorUnl>().SetUnlockFlg(false);
        }

        //World_3クリア済み
        if ((wf & WorldFlags.World_3) == WorldFlags.World_3)
        {
            worlds[2].GetComponent<WorUnl>().SetClearFlg(true);
            worlds[3].GetComponent<WorUnl>().SetUnlockFlg(true);
        }
        else
        {
            worlds[2].GetComponent<WorUnl>().SetClearFlg(false);
            worlds[3].GetComponent<WorUnl>().SetUnlockFlg(false);
        }

        //World_4クリア済み
        if ((wf & WorldFlags.World_4) == WorldFlags.World_4)
        {
            worlds[3].GetComponent<WorUnl>().SetClearFlg(true);
        }
        else
        {
            worlds[3].GetComponent<WorUnl>().SetClearFlg(false);
        }
    }

    //NowSelWorldの設定、ワールド名の変更
    public void SetNowSelWorld(int WorNo)
    {
        NowSelWorld = NowSelWorld + WorNo;
        WorldNameText.text = "World-" + (NowSelWorld + 1).ToString() + "\n" + worlds[NowSelWorld].GetComponent<WorUnl>().GetWorldName();
    }

    //次のワールドのGetter
    public bool GetNextUnlock()
    {
        if (NowSelWorld == LAST_WORLD)
        {
            //最後のワールドを選択していたら移動させない
            return false;
        }

        if (worlds[NowSelWorld + 1].GetComponent<WorUnl>().GetUnlockFlg())
        {
            //次のワールドが解放されてたら動かす
            return true;
        }
        else
        {
            //ここに「まだ解放されていません」的なのを出す処理を書く予定
            return false;
        }
    }

    //前ワールドのGetter
    public bool GetPrevUnlock()
    {
        if (NowSelWorld == FIRST_WORLD)
        {
            //最初のワールドを選択していたら移動させない
            return false;
        }

        if (worlds[NowSelWorld - 1].GetComponent<WorUnl>().GetUnlockFlg())
        {
            //次のワールドが解放されてたら動かす
            return true;
        }
        else
        {
            //ここには来ないはずなので。
            Debug.Log("WSManagerでバグ吐いとるで。(GetPrevUnlock)");
            return false;
        }
    }

    //

    //シーンチェンジ
    public void GoSceneChange()
    {
        FadeObj.GetComponent<FadeManager>().FadeScene(worlds[NowSelWorld].GetComponent<WorUnl>().GetGoSceneNo());
    }

    //MoveEndFlgのSetter
    public void SetMoveEndFlg(bool me)
    {
        MoveEndFlg = me;
    }

    //デバッグモード用
    private void OnValidate()
    {
        if(DMSwitch)
        {
            DebugMode = true;
        }
        else
        {
            DebugMode = false;
        }
    }
}
