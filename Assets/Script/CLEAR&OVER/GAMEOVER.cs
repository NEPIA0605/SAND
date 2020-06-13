using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class GAMEOVER : MonoBehaviour
{
    GameObject PlayerObj;

    GameObject GameOverUI;

    float Audio_length;

    float Audio_Clip;

    float Time_Stop;

    float Time_Stop_Max = 2.0f;

    bool Audiotime;

    bool Audio_flg;

    bool GAMEOVER_BGM;

    bool GameOverFlg_BGM;

    [SerializeField] AudioClip[] clips;//サウンド

    //SEです。
    protected AudioSource Source;

    // Start is called before the first frame update
    void Start()
    {
        GameOverFlg_BGM = false;

        PlayerObj = GameObject.Find("player");

        GameOverUI = GameObject.Find("GameoverUI");

        //サウンド
        Source = GetComponent<AudioSource>();
        GAMEOVER_BGM = true;
        Audiotime = false;
        Audio_flg = false;
        Audio_length = 1;
        Audio_Clip = 0;
        Time_Stop = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        GameOverFlg_BGM = PlayerObj.GetComponent<PlayerControler>().GetGameOverFlg();

        if ((GameOverUI.activeSelf) && (GAMEOVER_BGM) && (GameOverFlg_BGM))
        {
            Time_Stop += Time.deltaTime;
            if (Time_Stop > Time_Stop_Max)
            {
                //Debug.Log("シナモンロールCV俺");
                Source.PlayOneShot(clips[0]);
                GAMEOVER_BGM = false;
                Audiotime = true;
                Audio_Clip = Source.clip.length;
                Time_Stop = 0;
            }
        }
        if (Audiotime)
        {
            Audio_length += Time.deltaTime;
            if (Audio_length > Audio_Clip)
            {
                Audio_flg = true;
            }
        }
    }

    public bool GetOverSoundFlg()
    {
        return Audio_flg;
    }
}