using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public float speed = 0.005f;  //透明化の速さ
    float alfa;    //A値を操作するための変数
    float red, green, blue;    //RGBを操作するための変数
    bool FadeInFlg;   //FadeInのフラグ
    bool FadeOutFlg;   //FadeOutのフラグ
    int FadeSceneNo;   //シーン遷移の番号保存変数


    // Start is called before the first frame update
    void Start()
    {
        //Panelの色を取得
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
        alfa = GetComponent<Image>().color.a;

        FadeInFlg = true;
        FadeOutFlg = false;
        FadeSceneNo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //フェードイン
        if (FadeInFlg == true)
        {
            FadeIn();
        }
        //フェードアウト
        if (FadeOutFlg == true)
        {
            FadeOut();
        }
    }
    //フェードイン
    public void FadeIn()
    {
        //色反映
        GetComponent<Image>().color = new Color(red, green, blue, alfa);//Imageのカラーを変更。Colorの引数は（ 赤, 緑, 青, 不透明度 ）の順で指定
        //アルファ値が0以上の場合アルファ値を上げる
        if (alfa > 0.0f)
        {
            alfa -= speed;
        }
        //0以上になったらシーン遷移
        if (alfa <= 0.0f)
        {
            FadeInFlg = false;

        }
    }

    //フェードアウト
    public void FadeOut()
    {
        //色反映
        GetComponent<Image>().color = new Color(red, green, blue, alfa);//Imageのカラーを変更。Colorの引数は（ 赤, 緑, 青, 不透明度 ）の順で指定
        //アルファ値が1未満の場合アルファ値を下げる
        if (alfa < 1.0f)
        {
            alfa += speed;
        }
        //1以上になったらシーン遷移
        if (alfa >= 1.0f)
        {
            SceneManager.LoadScene(FadeSceneNo);
            FadeOutFlg = false;
        }
    }

    //FadeInFlgフラグのSetter
    public void SetFadeInFlg(bool F1)
    {
        FadeInFlg = F1;
    }


    //FadeInFlgフラグのGetter
    public bool GetFadeInFlg()
    {
        return FadeInFlg;
    }

    //FadeOutFlgフラグのSetter
    public void SetFadeOutFlg(bool F2)
    {
        FadeOutFlg = F2;
    }

    //FadeInFlgフラグのGetter
    public bool GetFadeOutFlg()
    {
        return FadeOutFlg;
    }

    //シーン遷移、引数にシーン遷移したいシーン番号を入れて呼び出したらフェードアウトしてシーン遷移をする
    public void FadeScene(int SceneNo)
    {
        FadeSceneNo = SceneNo;
        FadeOutFlg = true;
    }

}
