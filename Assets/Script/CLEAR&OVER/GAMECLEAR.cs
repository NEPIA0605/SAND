using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]


public class GAMECLEAR : MonoBehaviour
{

    GameObject PlayerObj;

    GameObject ClearUI;

    bool CLEAR_BGM;

    bool GameClearFlg_BGM;

    [SerializeField] AudioClip[] clips;//サウンド

    //SEです。
    protected AudioSource Source;

    // Start is called before the first frame update
    void Start()
    {
        GameClearFlg_BGM = false;

        PlayerObj = GameObject.Find("player");

        ClearUI = GameObject.Find("GameClearUI");
        //サウンド
        Source = GetComponent<AudioSource>();
        CLEAR_BGM = true;
    }

    // Update is called once per frame
    void Update()
    {
        GameClearFlg_BGM = PlayerObj.GetComponent<PlayerControler>().GetGameClearFlg();

        if ((ClearUI.activeSelf) && (CLEAR_BGM) && (GameClearFlg_BGM))
        {
            Debug.Log("ハローキティ");
            Source.PlayOneShot(clips[0]);
            CLEAR_BGM = false;
        }
    }
}