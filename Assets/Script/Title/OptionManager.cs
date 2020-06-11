using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class OptionManager : MonoBehaviour
{
    //定数宣言
    private const int FIRST_OPT = 0;
    private const int LAST_OPT = 3;

    //enumみたいな
    private const int OPT_BGM = 0;
    private const int OPT_SE = 1;
    private const int OPT_HOWTO = 2;
    private const int OPT_TITLE = 3;

    [Header("現在選択されているオプション")]
    [SerializeField] public int NowSelOpt;


    [Header("各種オブジェクト")]
    public GameObject[] option; //オプションオブジェクト
    public GameObject cursor; //カーソル部品　本体
    public GameObject cursorL; //カーソル部品　L
    public GameObject cursorR; //カーソル部品　R
    [SerializeField] private OptCorsorMove OpCM; //カーソル動かすコンポネ
    public SEVolumeInspecter se1;   //TitleUIの方
    public SEVolumeInspecter se2;   //Optionsの方
    public BGMVolumeInspecter bgm;  //BGM

    public Button button;

    public float dist;      //距離

    //タイム計測用
    public bool Push_Title;
    public bool time_option;
    public bool time;
    public float time_Out = 0.3f;
    private float timer;

    [SerializeField] AudioClip[] clips;//サウンド

    //SEです。
    protected AudioSource Source;

    // Start is called before the first frame update
    void Start()
    {
        OpCM = cursor.GetComponent<OptCorsorMove>();

        Push_Title = false;
        time = false;
        time_option = false;
        timer = 0;

        //サウンド
        Source = GetComponent<AudioSource>();
        se1 = this.GetComponent<SEVolumeInspecter>();
    }

    // Update is called once per frame
    void Update()
    {
        //カーソル移動中は入力できないようにする
        if (OpCM.GetMoveEnd())
        {
            //操作説明表示中は操作できない
            if (option[OPT_HOWTO].GetComponent<HowToChange>().GetHowToFlg() == false)
            {
                //キー操作で操作できるようにする
                if ((Input.GetAxisRaw("Vertical") > 0) || (Input.GetKeyDown(KeyCode.UpArrow)))
                {
                    if (GetPrevOpt())
                    {
                        //カーソル選択音
                        Source.PlayOneShot(clips[0]);
                    }
                    //上へ
                    OpCM.GoPrev();
                }
                else if ((Input.GetAxisRaw("Vertical") < 0) || (Input.GetKeyDown(KeyCode.DownArrow)))
                {
                    if (GetNextOpt())
                    {
                        //カーソル選択音
                        Source.PlayOneShot(clips[0]);
                    }
                    //下へ
                    OpCM.GoNext();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown("joystick button 1") || Push_Title == true)
                {
                    Debug.Log("A");
                    time = true;
                    //戻る際のSE
                    Source.PlayOneShot(clips[2]);
                    Push_Title = false;
                }
                if (time)
                {
                    timer += Time.deltaTime;

                }
                if (timer >= time_Out)
                {
                    Debug.Log("A");
                    time = false;
                    timer = 0;
                    this.gameObject.SetActive(false);

                    //PushTitleBackButton();
                }
            }

            //選んでいる項目によって操作を変える
            switch (NowSelOpt)
            {
                case OPT_TITLE:
                    if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
                    {
                        time_option = true;
                        //戻る際のSE
                        Source.PlayOneShot(clips[2]);
                    }
                    else if (time_option)
                    {
                        timer += Time.deltaTime;

                    }
                    if (timer >= time_Out)
                    {
                        Debug.Log("C");
                        time_option = false;
                        timer = 0;
                    }

                    break;
                default:
                    break;
            }
        }
    }


    //NowSelOptの設定
    public void SetNowSelOpt(int OptNo)
    {
        NowSelOpt = NowSelOpt + OptNo;
    }

    //次オプションのGetter
    public bool GetNextOpt()
    {
        if (NowSelOpt == LAST_OPT)
        {
            //最後のステージを選択していたら移動させない
            return false;
        }

        return true;
    }

    //前オプションのGetter
    public bool GetPrevOpt()
    {
        if (NowSelOpt == FIRST_OPT)
        {
            //最初のステージを選択していたら移動させない
            return false;
        }

        return true;
    }

    //次のオプションの場所Getter
    public Vector3 GetNextPos()
    {
        //さらに調整
        float ajuL, ajuR;
        float scale = this.GetComponent<RectTransform>().localScale.x;
        if (NowSelOpt + 1 >= 2)
        {
            ajuL = 130 * scale;
            ajuR = -130 * scale;
        }
        else
        {
            ajuL = -20 * scale;
            ajuR = -50 * scale;
        }

        float tmp = option[NowSelOpt + 1].transform.position.x -
                                                        (option[NowSelOpt + 1].GetComponent<RectTransform>().sizeDelta.x * scale / 2) - (dist * scale) + ajuL;
        cursorL.GetComponent<OptLRMove>().SetEndPosX(tmp);

        tmp = option[NowSelOpt + 1].transform.position.x +
                                                        (option[NowSelOpt + 1].GetComponent<RectTransform>().sizeDelta.x * scale / 2) + (dist * scale) + ajuR;
        cursorR.GetComponent<OptLRMove>().SetEndPosX(tmp);
        return option[NowSelOpt + 1].transform.position;
    }

    //前オプションの場所Getter
    public Vector3 GetPrevPos()
    {
        //さらに調整
        float ajuL, ajuR;
        float scale = this.GetComponent<RectTransform>().localScale.x;
        if (NowSelOpt - 1 == 2)
        {
            ajuL = 130 * scale;
            ajuR = -130 * scale;
        }
        else
        {
            ajuL = -20 * scale;
            ajuR = -50 * scale;
        }

        float tmp = option[NowSelOpt - 1].transform.position.x -
                                                (option[NowSelOpt - 1].GetComponent<RectTransform>().sizeDelta.x * scale / 2) - (dist * scale) + ajuL;
        cursorL.GetComponent<OptLRMove>().SetEndPosX(tmp);

        tmp = option[NowSelOpt - 1].transform.position.x +
                                                        (option[NowSelOpt - 1].GetComponent<RectTransform>().sizeDelta.x * scale / 2) + (dist * scale) + ajuR;
        cursorR.GetComponent<OptLRMove>().SetEndPosX(tmp);
        return option[NowSelOpt - 1].transform.position;
    }

    public Vector3 GetBBPos(int id)
    {
        //さらに調整
        float ajuL, ajuR;
        float scale = this.GetComponent<RectTransform>().localScale.x;
        if (NowSelOpt - 1 == 2)
        {
            ajuL = 130 * scale;
            ajuR = -130 * scale;
        }
        else
        {
            ajuL = -20 * scale;
            ajuR = -50 * scale;
        }

        float tmp = option[OPT_BGM].transform.position.x -
                                                (option[OPT_BGM].GetComponent<RectTransform>().sizeDelta.x * scale / 2) - (dist * scale) + ajuL;

        if(id == 2)
        {
            return new Vector3(tmp, option[OPT_BGM].transform.position.y, option[OPT_BGM].transform.position.z);
        }

        tmp = option[OPT_BGM].transform.position.x +
                                                        (option[OPT_BGM].GetComponent<RectTransform>().sizeDelta.x * scale / 2) + (dist * scale) + ajuR;

        if (id == 3)
        {
            return new Vector3(tmp, option[OPT_BGM].transform.position.y, option[OPT_BGM].transform.position.z);
        }

        return option[OPT_BGM].transform.position;
    }

    //操作説明押したとき
    public void PushHowToButton()
    {
        //決定の際のSE
        Source.PlayOneShot(clips[1]);

        //決定
        option[OPT_HOWTO].GetComponent<HowToChange>().HowToOpen();
    }

    //タイトルに戻るボタン押したとき
    public void PushTitleBackButton()
    {
        //決定ボタンでオプション消す
        button.Select();
        Push_Title = true;
        //this.gameObject.SetActive(false);
    }

    //プレイワンショットするだけ…じゃない
    public void AsPlayOs(int num)
    {
        Source.PlayOneShot(clips[num]);

        bgm.BGMVolChange();
        se1.SEVolChange();
        se2.SEVolChange();
    }

    public void CReset()
    {
        NowSelOpt = FIRST_OPT;

        cursor.GetComponent<OptCorsorMove>().CursorReset();
        cursorR.GetComponent<OptLRMove>().CLRReset();
        cursorL.GetComponent<OptLRMove>().CLRReset();
    }
}
