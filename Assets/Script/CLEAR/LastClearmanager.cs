using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LastClearmanager : MonoBehaviour
{
    bool LastFadeFlg;
    public float speed = 0.001f;  //透明化の速さ
    float alfa;    //A値を操作するための変数
    float red, green, blue;    //RGBを操作するための変数

    public GameObject GameClearFade;

    // Start is called before the first frame update
    void Start()
    {
        //Panelの色を取得
        red = 1;
        green =1;
        blue = 1;
        alfa = GetComponent<Image>().color.a;

        LastFadeFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(alfa);

        if (LastFadeFlg == true)
        {
            //色反映
            GameClearFade.GetComponent<Image>().color = new Color(red, green, blue, alfa);//Imageのカラーを変更。Colorの引数は（ 赤, 緑, 青, 不透明度 ）の順で指定
                                                                            //アルファ値が1未満の場合アルファ値を下げる
            if (alfa < 1.0f)
            {
                alfa += speed;
            }
            //1以上になったらシーン遷移
            if (alfa >= 1.0f)
            {
                SceneManager.LoadScene(1);
                LastFadeFlg = false;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LastFadeFlg = true;
        }
    }

}
