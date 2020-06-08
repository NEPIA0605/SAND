using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class GameOverManagement : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;//サウンド

    //SEです。
    protected AudioSource Source;

    GameObject PlayerObj;
    GameObject GameOverUI;
    private GameObject FadeObj;
    bool GameOverFlg;
    public Button button2;
    bool ButtonSelectFlg;

    [Header("WorldID")]
    public int WorldID;

    bool Stick_over;
    bool Time_over;
    bool restart_Stage;
    bool Stage_Select_over;

    float stick_over;
    float time_over;
    float over_Max = 0.5f;
    float Stick_over_MAX = 0.125f;


    // Start is called before the first frame update
    void Start()
    {
        GameOverFlg = false;
        PlayerObj = GameObject.Find("player");
        GameOverUI = GameObject.Find("GameoverUI");
        FadeObj = GameObject.Find("FadePanel");
        ButtonSelectFlg = false;
        //button = GameObject.Find("OneMorePlayButton").GetComponent<Button>();
        //ボタンが選択された状態になる

        stick_over = 0;
        time_over = 0;
        
        //サウンド
        Source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //ゲームオーバーフラグ取得
        GameOverFlg = PlayerObj.GetComponent<PlayerControler>().GetGameOverFlg();
        if (GameOverFlg == true) {
            //ゲームオーバーテキストを表示
            GameOverUI.SetActive(true);
            if (ButtonSelectFlg == false)
            {
                button2.Select();
                ButtonSelectFlg = true;
            }

     
        }
        if (GameOverFlg == false)
        {
            GameOverUI.SetActive(false);
        }

        if (GameOverUI.activeSelf)
        {
            //キー操作で操作できるようにする
            if ((Input.GetAxisRaw("Vertical") > 0) || (Input.GetKeyDown(KeyCode.UpArrow)))
            {
                if (!Time_over)
                {
                    //Debug.Log("動いてる？");
                    //カーソル選択音
                    Source.PlayOneShot(clips[0]);
                    Time_over = true;
                    Stick_over = true;
                }

            }
            else if ((Input.GetAxisRaw("Vertical") < 0) || (Input.GetKeyDown(KeyCode.DownArrow)))
            {
                if (!Time_over)
                {
                    //カーソル選択音
                    Source.PlayOneShot(clips[0]);
                    Time_over = true;
                    Stick_over = true;
                }
            }
            else
            {
                Stick_over = false;
            }

            if (Time_over)
            {
                //key_time += 1;
                stick_over += Time.deltaTime;
                if ((stick_over > Stick_over_MAX) && (!Stick_over))
                {
                    stick_over = 0;

                    Time_over = false;
                }
                else if ((stick_over > Stick_over_MAX) && (Stick_over))
                {
                    stick_over = 0;

                    Time_over = false;
                }
            }

            if (restart_Stage)
            {
                time_over += Time.deltaTime;
                if (time_over > over_Max)
                {
                    time_over = 0;
                    Debug.Log("もう一度遊ぶ");
                    // 現在のScene名を取得する
                    Scene loadScene = SceneManager.GetActiveScene();
                    // Sceneの読み直し
                    SceneManager.LoadScene(loadScene.name);

                }
            }
            else if (Stage_Select_over)
            {
                time_over += Time.deltaTime;
                if (time_over > over_Max)
                {
                    time_over = 0;

                    Debug.Log("ステージ選択へ");

                    FadeObj.GetComponent<FadeManager>().FadeScene(WorldID + 1);

                }
            }
        }

    }

    public void PushPlayButton()
    {

        restart_Stage = true;
        Source.PlayOneShot(clips[1]);

        //Debug.Log("もう一度遊ぶ");
        //// 現在のScene名を取得する
        //Scene loadScene = SceneManager.GetActiveScene();
        //// Sceneの読み直し
        //SceneManager.LoadScene(loadScene.name);

    }

    public void PushReturnStageSelectButton2()
    {
        Stage_Select_over = true;
        Source.PlayOneShot(clips[1]);


        //Debug.Log("ステージ選択へ");

        //FadeObj.GetComponent<FadeManager>().FadeScene(WorldID + 1);
    }

    public int GetWorldID()
    {
        return WorldID; 
    }

}
