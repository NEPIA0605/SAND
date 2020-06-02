using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class PauseManager : MonoBehaviour
{
    //　ポーズした時に表示するUI
    [SerializeField]
    private GameObject pauseUI;
    public GameObject ExplanationObj;
    private GameObject ReadyObj;
    private GameObject GameClearObj;
    private GameObject GameOverObj;

    [SerializeField] AudioClip[] clips;//サウンド

    //SEです。
    protected AudioSource Source;


    void Start()
    {
        ReadyObj = GameObject.Find("ReadyStart");
        GameClearObj = GameObject.Find("GameClearUI"); 
        GameOverObj = GameObject.Find("GameoverUI");

        pauseUI.SetActive(false);
        Time.timeScale = 1f;

        //サウンド
        Source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!ReadyObj.activeSelf && !GameClearObj.activeSelf && !GameOverObj.activeSelf)
        {
            //操作説明が出ていないとき
            if (!ExplanationObj.activeSelf)
            {
                if (Input.GetKeyDown("r") || Input.GetKeyDown("joystick button 7"))
                {
                    Source.PlayOneShot(clips[0]);

                    //　ポーズUIのアクティブ、非アクティブを切り替え
                    pauseUI.SetActive(!pauseUI.activeSelf);
                }
                //ポーズUIが表示されてる時は停止
                if (pauseUI.activeSelf)
                {
                    Time.timeScale = 0f;
                    //ポーズUIが表示されてなければ通常通り進行
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
