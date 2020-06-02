using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
    GameObject PlayerObj;
    GameObject ClearUI;
    GameObject FadeObject;

    bool GameClearFlg;
    public Button button;
    bool ButtonSelectFlg;
    float ClearUITime; //クリアUIを出す時間測定
    [Header("ID")]
    [Tooltip("このステージが所属するワールドNo(1~4)")] public int WorldID;     //1~4
    [Tooltip("このステージのステージNo(1~5(ワールド1は1~4))")] public int StageID;  //1~5か1~4

    // Start is called before the first frame update
    void Start()
    {
        GameClearFlg = false;
        PlayerObj = GameObject.Find("player");
        ClearUI = GameObject.Find("GameClearUI");
        FadeObject = GameObject.Find("FadePanel");
        ButtonSelectFlg = false;
        ClearUITime = 0.0f;
        //button = GameObject.Find("NextStage").GetComponent<Button>();


    }

    // Update is called once per frame
    void Update()
    {
        GameClearFlg = PlayerObj.GetComponent<PlayerControler>().GetGameClearFlg();
        if(GameClearFlg == true)
        {
            if (ClearUITime < 4.5f)
            {
                ClearUITime += Time.deltaTime;
            }
            if (ClearUITime >= 4.5f)
            {
                ClearUI.SetActive(true);
                if (ButtonSelectFlg == false)
                {
                    //ステージクリア処理
                    StageClear();
                    //ボタンが選択された状態になる
                    button.Select();
                    ButtonSelectFlg = true;
                }
            }
        }
        else if (GameClearFlg == false)
        {
            ClearUI.SetActive(false);
        }
    }


    public void PushNextStageButton()
    {
        Debug.Log("次のステージへ");
        // 現在のScene名を取得する
        //Scene loadScene = SceneManager.GetActiveScene();
        int NextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Sceneの読み直し
        SceneManager.LoadScene(NextSceneIndex);
    }
    public void PushReturnStageSelectButton()
    {
        Debug.Log("ステージ選択へ");

        FadeObject.GetComponent<FadeManager>().FadeScene(WorldID + 1);
    }

    //ステージクリア処理
    public void StageClear()
    {
        if(WorldID == 0 ||
            StageID == 0)
        {
            Debug.Log("セーブ失敗しました(IDに問題あり)");
            return;
        }
        //データ取得
        string SDataName, WDataName;
        int Tmp_Flag;
        SDataName = "STAGE_FLAG_" + WorldID.ToString();
        WDataName = "WORLD_FLAG";

        //ステージクリア情報を更新して保存
        Tmp_Flag = PlayerPrefs.GetInt(SDataName, 0);
        //未クリアの時だけやる
        if ((Tmp_Flag & (1 << (StageID - 1))) == 0)
        {
            Debug.Log("すてくり");
            Tmp_Flag = Tmp_Flag | (1 << (StageID - 1));
            PlayerPrefs.SetInt(SDataName, Tmp_Flag);
        }

        //最後のステージだった場合はワールドクリア処理をする
        //ワールド１は4がラスステ、他は5がラスステ
        if ((WorldID == 1 && StageID == 4) ||
            (WorldID != 1 && StageID == 5))
        {
            //保険
            Tmp_Flag = 0;

            //ワールドクリア情報を更新して保存
            Tmp_Flag = PlayerPrefs.GetInt(WDataName, 0);
            //未クリアの時だけやる
            if ((Tmp_Flag & (1 << (WorldID - 1))) == 0)
            {
                Debug.Log("わるくり");
                Tmp_Flag = Tmp_Flag | (1 << (WorldID - 1));
                PlayerPrefs.SetInt(WDataName, Tmp_Flag);
            }
        }

        //保存
        PlayerPrefs.Save();
    }

    private void OnValidate()
    {
        //IDの例外補正
        if (WorldID > 4)
        {
            WorldID = 0;
        }

        if(StageID > 5)
        {
            StageID = 0;
        }
    }
}
