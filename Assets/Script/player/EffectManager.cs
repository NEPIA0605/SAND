using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EffectManager : MonoBehaviour
{
    // Post Process Volume がついているGameObject
    private PostProcessVolume postProcessVolume;
    Vignette vignette;
    GameObject PlayerObj;
    bool PlayerTurnFlg;
    bool TimeCntFlg;
    float ColorIntensity;
    float ColorTime;
    bool IntensityFlg;  //Intensityが完了したかどうか
    public float MaxColorTime;
    public float MaxIntensity;
    
    void Start()
    {

        ColorTime = 0.0f;
        ColorIntensity = 0.0f;
        IntensityFlg = false;
        PlayerObj = GameObject.Find("player");
        PlayerTurnFlg = false;
        TimeCntFlg = false;

        //Bloom効果のインスタンスの作成
        vignette = ScriptableObject.CreateInstance<Vignette>();
        vignette.enabled.Override(true);
        vignette.intensity.Override(ColorIntensity);

        //　ポストプロセスボリュームに反映
        postProcessVolume = PostProcessManager.instance.QuickVolume(gameObject.layer, 0f, vignette);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTurnFlg = PlayerObj.GetComponent<PlayerControler>().GetPlayerTurn();
        vignette.intensity.Override(ColorIntensity);

        Debug.Log("色" + ColorIntensity);
        //Debug.Log("時間" + ColorTime);

        Debug.Log("反転 :"+PlayerTurnFlg);

        if (PlayerTurnFlg == true)
        {
            if (ColorIntensity <= 0.0f)
            {
                IntensityFlg = true;
                TimeCntFlg = true;
            }

        }
        if (PlayerTurnFlg == false)
        {
            if (ColorIntensity >  0.0f)
            {
                IntensityFlg = true;
                TimeCntFlg = true;
            }
            //vignette.enabled.Override(false);

        }

        if (TimeCntFlg == true)
        {
            //Debug.Log("さｓだｄｗだｓだ");

            if (IntensityFlg == true)
            {
                ColorTime += Time.deltaTime;
            }
            //縮まる
            if (PlayerTurnFlg)
            {
                if (ColorTime <= MaxColorTime)
                {
                    ColorIntensity = (ColorTime / MaxColorTime) * MaxIntensity;
                    if (ColorIntensity > MaxIntensity)
                    {
                        ColorIntensity = MaxIntensity;
                    }
                }
                if(ColorTime >= MaxColorTime)
                {
                    ColorIntensity = MaxIntensity;
                    TimeCntFlg = false;
                    ColorTime = 0.0f;
                    IntensityFlg = false;
                }
            }
            //広がる
            if (!PlayerTurnFlg)
            {
                if (ColorTime <= MaxColorTime)
                {
                    ColorIntensity = MaxIntensity - ((ColorTime / MaxColorTime) * MaxIntensity);

                }
                else
                {
                    ColorIntensity = 0.0f;
                    TimeCntFlg = false;
                    ColorTime = 0.0f;
                    IntensityFlg = false;
                }
            }

        }
    }
}
