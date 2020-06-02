using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class PauseButtonManager : MonoBehaviour
{
    private GameObject FadeObj; //フェードのパネル
    public GameObject pauseObj;
    public GameObject ExplanationObj;
    private GameObject GameoverObj; //フェードのパネル

    public bool ReturnFlg;

    public Button ReStartButton;

    bool SelectButton;
    //Button ResetButton;
    //Button ExplanationButton;
    //Button ReturnStageSerectButton;

    float pause_select;
    float time_pause;
    float time_pause_MAX = 30f;
    float pause_se;
    float pause_se_MAX = 30f;

    float key_time;
    float button_time;

    bool pause_Return_G;
    bool pause_Return_St;
    bool pause_Return_S;
    bool pause_Tori;
    bool time_p;

    [SerializeField] AudioClip[] clips;//サウンド

    //SEです。
    protected AudioSource Source;


    // Start is called before the first frame update
    void Start()
    {
        //初期化
        ReturnFlg = false;
        FadeObj = GameObject.Find("FadePanel");
        GameoverObj = GameObject.Find("Gameover");
        ExplanationObj.SetActive(false);
        SelectButton = false;
        //ResetButton = GameObject.Find("ResetButton").GetComponent<Button>();
        //ExplanationButton = GameObject.Find("ExplanationButton").GetComponent<Button>();
        //ReturnStageSerectButton = GameObject.Find("ReturnStageSerectButton").GetComponent<Button>();

        // 最初に選択状態にしたいボタンの設定
        pause_se = 0;
        pause_Return_G = false;
        pause_Return_St = false;
        pause_Return_S = false;
        pause_Tori = false;
        time_p = false;
        time_pause = 0;
        pause_select = 0;

        key_time = 0;
        button_time = 0;

        //サウンド
        Source = GetComponent<AudioSource>();

    }
    void Update()
    {
 
        if (SelectButton == false)
        {
            if (pauseObj.activeSelf == true)
            {
                ReStartButton.Select();
                SelectButton = true;
            }
        }

        //キー操作で操作できるようにする
        if ((Input.GetAxisRaw("Vertical") > 0) || (Input.GetKeyDown(KeyCode.UpArrow)))
        {
            if (!time_p)
            {
                Debug.Log("動いてる？");
                //pause_select -= 1;
                //カーソル選択音
                Source.PlayOneShot(clips[0]);
                time_p = true;
            }

        }
        else if ((Input.GetAxisRaw("Vertical") < 0) || (Input.GetKeyDown(KeyCode.DownArrow)))
        {
            if (!time_p)
            {
                //pause_select += 1;
                //カーソル選択音
                Source.PlayOneShot(clips[0]);
                time_p = true;
            }
        }

        if (time_p)
        {
            key_time += 1;
            time_pause = key_time;
            if (time_pause > time_pause_MAX)
            {
                key_time = 0;
                time_pause = 0;
                time_p = false;
            }
        }


        //時間計測
        //ゲームに戻る
        if(pause_Return_G)
        {
            Debug.Log("動いてる？");
            button_time += 1;
            pause_se = button_time;
        } 
        //ステ選に戻る
        else if ((pause_Return_St)&& (!pause_Return_G))
        {
            button_time += 1;
            pause_se = button_time;
        }  
        //説明だす
        else if ((pause_Tori)&& (!pause_Return_G) && (!pause_Return_St))
        {
            button_time += 1;
            pause_se = button_time;
        }
        //やり直す
        else if ((pause_Return_S) && (!pause_Return_G) && (!pause_Return_St) && (!pause_Tori))
        {
            button_time += 1;
            pause_se = button_time;
        }
        

        //消える
        //ゲームに戻る
        if ((pause_se > pause_se_MAX) && (pause_Return_G))
        {
            Debug.Log("消えろ！");
            button_time = 0;
            pause_se = 0;
            pause_Return_G = false;
            pauseObj.SetActive(false);
        }
        //ステ選に戻る
        else if ((pause_se > pause_se_MAX) && (pause_Return_St))
        {
            button_time = 0;
            pause_se = 0;
            pause_Return_St = false;
            pauseObj.SetActive(false);
            FadeObj.GetComponent<FadeManager>().FadeScene(GameoverObj.GetComponent<GameOverManagement>().GetWorldID() + 1);
            //pauseObj.SetActive(false);
        }
        //説明だす
        else if ((pause_se > pause_se_MAX) && (pause_Tori))
        {
            button_time = 0;
            pause_se = 0;
            pause_Tori = false;
            //pauseObj.SetActive(false);
            ExplanationObj.SetActive(true);
        }
        //やり直す
        else if ((pause_se > pause_se_MAX) && (pause_Return_S))

        {
            button_time = 0;
            pause_se = 0;
            pause_Return_S = false;
            pauseObj.SetActive(false);

            // 現在のScene名を取得する
            Scene loadScene = SceneManager.GetActiveScene();
            // Sceneの読み直し
            SceneManager.LoadScene(loadScene.name);

        }

    }

    //ボタン押下時
    public void PushReStartButton()
    {
        //操作説明が出ていないとき
        if (!ExplanationObj.activeSelf)
        {
            if ((!pause_Return_S) && (!pause_Return_St) && (!pause_Tori))
            {
                Source.PlayOneShot(clips[0]);
                pause_Return_G = true;
                Debug.Log("ゲームに戻る");
                //pauseObj.SetActive(false);
            }
        }
    }
    public void PushReturnStageSerectButton()
    {
        //操作説明が出ていないとき
        if (!ExplanationObj.activeSelf)
        {
            if ((!pause_Return_S) && (!pause_Return_G) && (!pause_Tori))
            {
                Source.PlayOneShot(clips[0]);
                pause_Return_St = true;
                Debug.Log("ステージ選択に戻る");
                //pauseObj.SetActive(false);
                //FadeObj.GetComponent<FadeManager>().FadeScene(GameoverObj.GetComponent<GameOverManagement>().GetWorldID() + 1);
            }
        }

        ////操作説明が出ていないとき
        //if (!ExplanationObj.activeSelf)
        //{
        //    Source.PlayOneShot(clips[0]);

        //    Debug.Log("ステージ選択に戻る");
        //    pauseObj.SetActive(false);
      
        //    FadeObj.GetComponent<FadeManager>().FadeScene(GameoverObj.GetComponent<GameOverManagement>().GetWorldID() + 1);
        //    //SceneManager.LoadScene("WorldSerect");
        //}
    }
    public void PushExplanationButton()
    {
        //操作説明が出ていないとき
        if (!ExplanationObj.activeSelf)
        {
            if ((!pause_Return_S) && (!pause_Return_G) && (!pause_Return_St))
            {
                Source.PlayOneShot(clips[0]);
                pause_Tori = true;
                Debug.Log("操作説明を開く");
                //ExplanationObj.SetActive(true);
            }
        }

        //操作説明が出ていないとき
        //if (!ExplanationObj.activeSelf)
        //{
        //    Source.PlayOneShot(clips[0]);

        //    Debug.Log("操作説明を開く");
        //    ExplanationObj.SetActive(true);
        //}
    }
    public void ResetButton()
    {
        //操作説明が出ていないとき
        if (!ExplanationObj.activeSelf)
        {
            if ((!pause_Return_G) && (!pause_Return_St) && (!pause_Tori))
            {
                Source.PlayOneShot(clips[0]);
                pause_Return_S = true;
                Debug.Log("リセットボタン");
                //pauseObj.SetActive(false);


                // 現在のScene名を取得する
                //Scene loadScene = SceneManager.GetActiveScene();
                // Sceneの読み直し
                //SceneManager.LoadScene(loadScene.name);
                //Debug.Log("ゲームに戻る");
                //pauseObj.SetActive(false);
            }
            //// 現在のScene名を取得する
            //Scene loadScene = SceneManager.GetActiveScene();
            //// Sceneの読み直し
            //SceneManager.LoadScene(loadScene.name);
        }

        //操作説明が出ていないとき
        //if (!ExplanationObj.activeSelf)
        //{
        //    Source.PlayOneShot(clips[0]);

        //    Debug.Log("リセットボタン");
        //    pauseObj.SetActive(false);

        //    Source.PlayOneShot(clips[0]);

        //    // 現在のScene名を取得する
        //    Scene loadScene = SceneManager.GetActiveScene();
        //    // Sceneの読み直し
        //    SceneManager.LoadScene(loadScene.name);
        //}
    }
}
