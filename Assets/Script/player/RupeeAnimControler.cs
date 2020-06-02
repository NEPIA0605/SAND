using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RupeeAnimControler : MonoBehaviour
{

    private GameObject Rupee;
    private GameObject TreasureboxObj;
    private GameObject PlayerObg;
    
    private bool RupeeAnimFlg;
    private bool RupeePosSetFlg;
    private Vector3 p0, p1, p2;
    float t;
    
    
    float RupeeAnimTime;
    // Start is called before the first frame update
    void Start()
    {
        Rupee = GameObject.Find("rupee");
        TreasureboxObj = GameObject.Find("Treasurebox");
        PlayerObg = GameObject.Find("player");
    
        t = 0.0f;
        RupeeAnimTime = 0.0f;
    
        RupeeAnimFlg = false;
        RupeePosSetFlg = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        RupeeAnimFlg = TreasureboxObj.GetComponent<ClearMotionManager>().GetRupeeAnimFlg();
        Debug.Log(this.transform.localScale);

        if (RupeeAnimFlg == true)
        {
            if (this.transform.localPosition.y <= 0.1f)
            {
                this.transform.position += new Vector3(0.0f, 0.02f, 0.0f);
            }
            RupeeAnimTime += Time.deltaTime;
        }
    
        if (RupeeAnimTime >= 1.5f && RupeePosSetFlg == false)
        {
            p0 = Rupee.transform.position;
            p1 = (Rupee.transform.position + PlayerObg.transform.position) / 2;
            p1.y += Rupee.transform.position.y - 0.1f;
            p2 = PlayerObg.transform.position;
            RupeeAnimFlg = false;
            RupeePosSetFlg = true;
        }
    
        if (RupeePosSetFlg == true) 
        {
            t += Time.deltaTime;
            Vector3 Q0 = Vector3.Lerp(p0, p1, t);
            Vector3 Q1 = Vector3.Lerp(p1, p2, t);
            Vector3 Q2 = Vector3.Lerp(Q0, Q1, t);

            this.transform.position = Q2;

            if (this.transform.localScale.y >= 1.0f)
            {
                this.transform.localScale = new Vector3(8 - t * 8, 6.5f - t * 6.5f, 6.5f - t * 6.5f);
            }
            if (this.transform.position == PlayerObg.transform.position)
            {
                this.gameObject.SetActive(false);
            }
        }
    
    
    }
}