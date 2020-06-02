using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearMotionManager : MonoBehaviour
{
    private GameObject PlayerObg;
    private bool GameClearFlg;
    float ClearModelAnimTime;
    Animator animator;

    bool RupeeAnimFlg;


    // Start is called before the first frame update
    void Start()
    {
        PlayerObg = GameObject.Find("player");
        animator = GetComponent<Animator>();
        ClearModelAnimTime = 0.0f;
        RupeeAnimFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameClearFlg = PlayerObg.GetComponent<PlayerControler>().GetGameClearFlg();
        if (GameClearFlg == true)
        {
            animator.SetBool("clear", true);
            ClearModelAnimTime += Time.deltaTime;
        }
        if (ClearModelAnimTime >= 1.5f)
        {
            RupeeAnimFlg = true;
        }
    }

    public bool GetRupeeAnimFlg()
    {
        return RupeeAnimFlg;
    }
}
