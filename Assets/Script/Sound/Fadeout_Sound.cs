using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class Fadeout_Sound : MonoBehaviour
{
    protected AudioSource Source;

    public bool IsFade;

    private FadeManager FadeObj;

    public double FadeOutSeconds = 1.0;

    bool IsFadeOut = true;

    double BaseVolume_S;

    double FadeDeltaTime = 0;

    void Start()
    {
        Source = this.GetComponent<AudioSource>();

        //フェードパネルとUIの親取得
        FadeObj = GameObject.Find("FadePanel").GetComponent<FadeManager>();
    }

    void Update()
    {
        if (IsFadeOut && FadeObj.GetFadeOutFlg())
        {
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
            Source.volume = (float)((BaseVolume_S * 0.1)  - (FadeDeltaTime / FadeOutSeconds));
        }
    }
}