using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]


public class GAMECLEAR : MonoBehaviour
{

    GameObject PlayerObj;

    GameObject ClearUI;

    float Audio_length_C;

    float Audio_Clip_C;

    bool Audio_time;

    bool Audio_flg_C;

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
        Audio_time = false;
        Audio_flg_C = false;
        Audio_length_C = 4;
        Audio_Clip_C = 0;
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
            Audio_time = true;
            Audio_Clip_C = Source.clip.length;
        }
        if (Audio_time)
        {
            Audio_length_C += Time.deltaTime;
            if (Audio_length_C > Audio_Clip_C)
            {
                Audio_flg_C = true;
            }
        }
    }

    public bool GetClearSoundFlg()
    {
        return Audio_flg_C;
    }
}