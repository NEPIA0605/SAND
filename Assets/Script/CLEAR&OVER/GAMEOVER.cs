using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class GAMEOVER : MonoBehaviour
{
    GameObject PlayerObj;

    GameObject GameOverUI;

    float Audio_length;
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
        Audio_length = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GameOverFlg_BGM = PlayerObj.GetComponent<PlayerControler>().GetGameOverFlg();

        if ((GameOverUI.activeSelf) && (GAMEOVER_BGM) && (GameOverFlg_BGM))
        {
            Debug.Log("シナモンロールCV俺");
            Source.PlayOneShot(clips[0]);
            GAMEOVER_BGM = false;
            Audiotime = true;
        }
        if (Audiotime)
        {
            Audio_length += Time.deltaTime;
            if (Audio_length > Source.clip.length)
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