using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadein_Game : MonoBehaviour
{
    protected AudioSource Source;

    protected GameObject GameOverUI_S;

    protected GameObject GameClearUI_S;

    private FadeManager FadeObj;

    public bool IsFade;

    public double FadeInSeconds = 1.0f;

    bool GC_Flg;

    bool GO_Flg;

    double FadeDeltaTime = 0;

    double BaseVolume;

    void Start()
    {
        Source = this.GetComponent<AudioSource>();
        BaseVolume = (float)PlayerPrefs.GetInt("VOLUME_BGM", 5);
        //フェードパネルとUIの親取得
        FadeObj = GameObject.Find("FadePanel").GetComponent<FadeManager>();
        GameOverUI_S = GameObject.Find("BGM_GAMEOVER");
        GameClearUI_S = GameObject.Find("BGM_CLEAR");

        GC_Flg = false;
        GO_Flg = false;
    }

    void Update()
    {
        //Debug.Log("フェード");

        GO_Flg = GameOverUI_S.GetComponent<GAMEOVER>().GetOverSoundFlg();
        GC_Flg = GameClearUI_S.GetComponent<GAMECLEAR>().GetClearSoundFlg();
        
        if (IsFade )//&& !FadeObj.GetFadeOutFlg()
        {
            if (GC_Flg || GO_Flg)
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
}