using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class TitleManager : MonoBehaviour
{
    private GameObject FadeObj; //フェードのパネル
    public GameObject OptionObj;    //オプションのウインドウ
    public Button button;

    float time_Title;
    float time_Title_MAX = 0.25f;
    bool TITLE_Lag;

    [SerializeField] AudioClip[] clips;//サウンド

    //SEです。
    protected AudioSource Source;

    public float Select;

    // Start is called before the first frame update
    void Start()
    {
        FadeObj = GameObject.Find("FadePanel");
        OptionObj.SetActive(false);
        button.Select();
        Select = 0;

        //サウンド
        Source = GetComponent<AudioSource>();
        time_Title = 0;
        TITLE_Lag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OptionObj.activeSelf)
        {
            Select = 0;
        }
        else if (!OptionObj.activeSelf)
        {
            //キー操作で操作できるようにする
            if ((Input.GetAxisRaw("Vertical") > 0) || (Input.GetKeyDown(KeyCode.UpArrow)))
            {
                if ((Select > 0) && (!TITLE_Lag))
                {

                    Debug.Log("傾けたタイトル");
                    Select -= 1;
                    //カーソル選択音
                    Source.PlayOneShot(clips[0]);
                    TITLE_Lag = true;
                }

            }
            else if ((Input.GetAxisRaw("Vertical") < 0) || (Input.GetKeyDown(KeyCode.DownArrow)))
            {
                if ((Select < 4) && (!TITLE_Lag))
                {
                    Debug.Log("タイトル傾けた");

                    Select += 1;
                    //カーソル選択音
                    Source.PlayOneShot(clips[0]);
                    TITLE_Lag = true;
                }
            }
            if (TITLE_Lag)
            {
                time_Title += Time.deltaTime;
            }
            if (time_Title >= time_Title_MAX)
            {
                time_Title = 0;
                TITLE_Lag = false;
            }
        }
    }

    //初めからボタンを押したとき
    public void PushStartButton()
    {
        //フェード中入力できなくする処理
        if (FadeObj.GetComponent<FadeManager>().GetFadeInFlg() == false)
        {
            //決定の際のSE
            Source.PlayOneShot(clips[1]);

            Debug.Log("はじめから　データ初期化");
            //始めからを選択した場合はデータを初期化する
            PlayerPrefs.DeleteKey("STAGE_FLAG_1");
            PlayerPrefs.DeleteKey("STAGE_FLAG_2");
            PlayerPrefs.DeleteKey("STAGE_FLAG_3");
            PlayerPrefs.DeleteKey("STAGE_FLAG_4");
            PlayerPrefs.DeleteKey("WORLD_FLAG");
            FadeObj.GetComponent<FadeManager>().FadeScene(1);
        }
    }

    //続きからボタンを押したとき
    public void PushReStartButton()
    {
        //フェード中入力できなくする処理
        if (FadeObj.GetComponent<FadeManager>().GetFadeInFlg() == false)
        {
            //決定の際のSE
            Source.PlayOneShot(clips[1]);

            Debug.Log("つづきから");
            FadeObj.GetComponent<FadeManager>().FadeScene(1);
        }
    }

    //オプションを押したとき
    public void PushOptionButton()
    {
        //フェード中入力できなくする処理
        if ((FadeObj.GetComponent<FadeManager>().GetFadeInFlg() == false) && (!OptionObj.activeSelf))
        {
            //決定の際のSE
            Source.PlayOneShot(clips[1]);

            Debug.Log("オプションを開く");
            OptionObj.SetActive(true);
        }
    }

    //ゲーム終了を押したとき
    public void PushExitButton()
    {
        //フェード中入力できなくする処理
        if ((FadeObj.GetComponent<FadeManager>().GetFadeInFlg() == false) && (!OptionObj.activeSelf))
        {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
#else
            Application.Quit();
#endif
        }
    }
}