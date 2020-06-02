using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManagement : MonoBehaviour
{
    GameObject PlayerObj;
    GameObject GameOverUI;
    private GameObject FadeObj;
    bool GameOverFlg;
    public Button button2;
    bool ButtonSelectFlg;

    [Header("WorldID")]
    public int WorldID;

    // Start is called before the first frame update
    void Start()
    {
        GameOverFlg = false;
        PlayerObj = GameObject.Find("player");
        GameOverUI = GameObject.Find("GameoverUI");
        FadeObj = GameObject.Find("FadePanel");
        ButtonSelectFlg = false;
        //button = GameObject.Find("OneMorePlayButton").GetComponent<Button>();
        //ボタンが選択された状態になる

    }

    // Update is called once per frame
    void Update()
    {
        //ゲームオーバーフラグ取得
        GameOverFlg = PlayerObj.GetComponent<PlayerControler>().GetGameOverFlg();
        if (GameOverFlg == true) {
            //ゲームオーバーテキストを表示
            GameOverUI.SetActive(true);
            if (ButtonSelectFlg == false)
            {
                button2.Select();
                ButtonSelectFlg = true;
            }

     
        }
        if (GameOverFlg == false)
        {
            GameOverUI.SetActive(false);
        }
    }

    public void PushPlayButton()
    {
        Debug.Log("もう一度遊ぶ");
        // 現在のScene名を取得する
        Scene loadScene = SceneManager.GetActiveScene();
        // Sceneの読み直し
        SceneManager.LoadScene(loadScene.name);

    }

    public void PushReturnStageSelectButton2()
    {
        Debug.Log("ステージ選択へ");

        FadeObj.GetComponent<FadeManager>().FadeScene(WorldID + 1);
    }

    public int GetWorldID()
    {
        return WorldID; 
    }

}
