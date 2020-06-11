using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadein_Game : MonoBehaviour
{
    protected AudioSource Source;

    protected GameObject GameOverUI_Sound;

    private FadeManager FadeObj;

    public bool IsFade;

    public double FadeInSeconds = 1.0f;

    bool GameClearFlg_Soundin;

    bool GameOverFlg_Soundin;

    double FadeDeltaTime = 0;

    double BaseVolume;

    void Start()
    {
        Source = this.GetComponent<AudioSource>();
        BaseVolume = (float)PlayerPrefs.GetInt("VOLUME_BGM", 5);
        //フェードパネルとUIの親取得
        FadeObj = GameObject.Find("FadePanel").GetComponent<FadeManager>();
        GameOverUI_Sound = GameObject.Find("GameoverUI");

        GameClearFlg_Soundin = false;
        GameOverFlg_Soundin = false;
    }

    void Update()
    {
        //Debug.Log("フェード");

        GameClearFlg_Soundin = GameOverUI_Sound.GetComponent<GAMEOVER>().GetOverSoundFlg();

        if (IsFade && GameClearFlg_Soundin)//&& !FadeObj.GetFadeOutFlg()
        {
            Debug.Log("フェード");

            Debug.Log(BaseVolume);
            Debug.Log(FadeInSeconds);
            //Debug.Log(FadeDeltaTime);
            Debug.Log(Source.volume);
            FadeDeltaTime += Time.deltaTime;
            if (FadeDeltaTime >= (FadeInSeconds * BaseVolume * 0.1))
            {
                FadeDeltaTime = FadeInSeconds * BaseVolume * 0.1;
                IsFade = false;
            }
            Source.volume = (float)(FadeDeltaTime / FadeInSeconds);
        }
    }
}