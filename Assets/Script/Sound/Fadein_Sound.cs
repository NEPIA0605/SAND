using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class Fadein_Sound : MonoBehaviour
{
    protected AudioSource Source;

    private FadeManager FadeObj;

    public bool IsFade;

    public double FadeInSeconds = 1.0f;

    double FadeDeltaTime = 0;

    double BaseVolume;

    void Start()
    {
        Source = this.GetComponent<AudioSource>();
        BaseVolume = (float)PlayerPrefs.GetInt("VOLUME_BGM", 5);
        //フェードパネルとUIの親取得
        FadeObj = GameObject.Find("FadePanel").GetComponent<FadeManager>();
    }

    void Update()
    {
        Debug.Log("フェード");

        if (IsFade)//&& !FadeObj.GetFadeOutFlg()
        {
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
            Source.volume = (float)(FadeDeltaTime / FadeInSeconds) ;
        }
    }
}