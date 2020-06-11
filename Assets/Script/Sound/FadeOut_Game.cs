using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut_Game : MonoBehaviour
{
    protected AudioSource Source;

    public bool IsFade;

    private GameObject PlayerObj_Sound;

    public double FadeOutSeconds = 1.0;

    bool IsFadeOut = true;

    bool GameClearFlg_Sound;

    bool GameOverFlg_Sound;

    double BaseVolume_S;

    double FadeDeltaTime = 0;

    void Start()
    {
        Source = this.GetComponent<AudioSource>();

        PlayerObj_Sound = GameObject.Find("player");

        GameClearFlg_Sound = false;

        GameOverFlg_Sound = false;
    }

    void Update()
    {
        GameClearFlg_Sound = PlayerObj_Sound.GetComponent<PlayerControler>().GetGameClearFlg();
        GameOverFlg_Sound = PlayerObj_Sound.GetComponent<PlayerControler>().GetGameOverFlg();
        if (IsFadeOut)
        {
            if (GameClearFlg_Sound || GameOverFlg_Sound)
            {
                Debug.Log("ゲームの方");

                BaseVolume_S = (float)PlayerPrefs.GetInt("VOLUME_BGM", 5);

                Debug.Log("フェードアウト開始");
                Debug.Log(BaseVolume_S);
                FadeDeltaTime += Time.deltaTime;
                if (FadeDeltaTime >= FadeOutSeconds * BaseVolume_S * 0.1)
                {
                    Debug.Log("フェードアウト終了");
                    FadeDeltaTime = FadeOutSeconds * BaseVolume_S * 0.1;
                    IsFadeOut = false;
                }
                Source.volume = (float)((BaseVolume_S * 0.1) - (FadeDeltaTime / FadeOutSeconds));
            }
        }
    }
}