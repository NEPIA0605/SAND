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
    public bool time_option;
    public bool time;
    public float time_Out = 0.3f;
    private float timer;


    private float RightInputTime;
    private float LeftInputTime;

    private bool RightInputFlg;
    private bool LeftInputFlg;

    [SerializeField] AudioClip[] clips;//サウンド

    //SEです。
    protected AudioSource Source;

    // Start is called before the first frame update
    void Start()
    {
        OpCM = cursor.GetComponent<OptCorsorMove>();
        RightInputTime = 0.0f;
        LeftInputTime = 0.0f;
        RightInputFlg = false;
        LeftInputFlg = false;
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
            if (Input.GetAxisRaw("Horizontal") > 0)
            {

                RightInputTime += Time.deltaTime;
            }
            if (Input.GetAxisRaw("Horizontal") < 0)
            {

                LeftInputTime += Time.deltaTime;
            }

            if ((RightInputTime >= 0.1f) || (Input.GetKeyDown(KeyCode.RightArrow)))
            {
                RightInputFlg = true;
                RightInputTime = 0.0f;
            }
            if ((LeftInputTime >= 0.1f) || (Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                LeftInputFlg = true;
                LeftInputTime = 0.0f;
            }

            //操作説明表示中は操作できない
            if (option[OPT_HOWTO].GetComponent<HowToChange>().GetHowToFlg() == false)
            {
                //キー操作で操作できるようにする
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw("Vertical") > 0)
                {
                    if (GetPrevOpt())
                    {
                        //カーソル選択音
                        Source.PlayOneShot(clips[0]);
                    }
                    //上へ
                    OpCM.GoPrev();
                }
                else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxisRaw("Vertical") < 0))
                {
                    if (GetNextOpt())
                    {
                        //カーソル選択音
                        Source.PlayOneShot(clips[0]);
                    }
                    //下へ
                    OpCM.GoNext();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown("joystick button 1"))
                {
                    Debug.Log("A");
                    time = true;
                    //戻る際のSE
                    Source.PlayOneShot(clips[2]);
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

                    //決定ボタンでオプション消す
                    button.Select();
                    this.gameObject.SetActive(false);
                }

                ////戻る際のSE
                //Source.PlayOneShot(clips[2]);

                ////戻る
                //button.Select();
                //this.gameObject.SetActive(false);

            }

            //選んでいる項目によって操作を変える
            switch (NowSelOpt)
            {
                //BGM
                case OPT_BGM:
                    if (RightInputFlg == true)
                    {
                        //音量上げ
                        option[NowSelOpt].GetComponent<VolumeChange>().VolUp();
                        bgm.BGMVolChange();
                        Source.PlayOneShot(clips[0]);
                        RightInputFlg = false;
                    }
                    else if (LeftInputFlg == true)
                    {
                        //音量下げ
                        option[NowSelOpt].GetComponent<VolumeChange>().VolDown();
                        bgm.BGMVolChange();
                        Source.PlayOneShot(clips[0]);
                        LeftInputFlg = false;
                    }

                    break;
                //SE
                case OPT_SE:
                    if (RightInputFlg == true)
                    {
                        //音量上げ
                        option[NowSelOpt].GetComponent<VolumeChange>().VolUp();
                        se1.SEVolChange();
                        se2.SEVolChange();
                        Source.PlayOneShot(clips[0]);
                        RightInputFlg = false;

                    }
                    else if (LeftInputFlg == true)
                    {
                        //音量下げ
                        option[NowSelOpt].GetComponent<VolumeChange>().VolDown();
                        se1.SEVolChange();
                        se2.SEVolChange();
                        Source.PlayOneShot(clips[0]);
                        LeftInputFlg = false;

                    }

                    break;
                //操作説明
                case OPT_HOWTO:
                    if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
                    {
                        //決定の際のSE
                        Source.PlayOneShot(clips[1]);

                        //決定
                        option[NowSelOpt].GetComponent<HowToChange>().HowToOpen();
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown("joystick button 1") )
                    {
                        if (!time)
                        {
                            Debug.Log("B");
                            //戻る際のSE
                            Source.PlayOneShot(clips[2]);
                        }
                        //閉じる
                        option[NowSelOpt].GetComponent<HowToChange>().HowToClose();
                    }

                    break;
                //タイトルに戻る
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

                        //決定ボタンでオプション消す
                        button.Select();
                        this.gameObject.SetActive(false);
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
}
