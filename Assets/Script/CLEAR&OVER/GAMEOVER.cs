using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class GAMEOVER : MonoBehaviour
{
    GameObject PlayerObj;

    GameObject GameOverUI;


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
        }

    }
}
