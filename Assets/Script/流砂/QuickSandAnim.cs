using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSandAnim : MonoBehaviour
{
    public GameObject root;     //親オブジェクト
    private Vector2   Offset;     //UVのオフセット
    private string    ObjName;     //このオブジェクトの名前

    // Start is called before the first frame update
    void Start()
    {
        //親を取得
        root = this.root.gameObject;

        //名前を取得
        ObjName = this.name;
    }

    // Update is called once per frame
    void Update()
    {
        //SandMoveを取得
        Vector3 tmpVec = root.GetComponent<Quicksand>().GetSandMove();

        //部位によって動かす向きを変える
        if (ObjName == "Top")
        {
            //xxyz
            this.gameObject.GetComponent<Renderer>().material.SetVector("_Test", new Vector2(tmpVec.x, tmpVec.z));
        }
        else if (ObjName == "Bottom")
        {
            //xxyz
            this.gameObject.GetComponent<Renderer>().material.SetVector("_Test", new Vector2(tmpVec.x, tmpVec.z));
        }
        else if (ObjName == "Front")
        {
            //xxyy
            this.gameObject.GetComponent<Renderer>().material.SetVector("_Test", new Vector2(-tmpVec.x, -tmpVec.y));
        }
        else if (ObjName == "Back")
        {
            //xxyy
            this.gameObject.GetComponent<Renderer>().material.SetVector("_Test", new Vector2(-tmpVec.x, -tmpVec.y));
        }
        else if (ObjName == "Right")
        {
            //xzyy
            this.gameObject.GetComponent<Renderer>().material.SetVector("_Test", new Vector2(tmpVec.z, -tmpVec.y));
        }
        else if (ObjName == "Left")
        {
            //xzyy
            this.gameObject.GetComponent<Renderer>().material.SetVector("_Test", new Vector2(-tmpVec.z, -tmpVec.y));
        }
        else
        {
            Debug.Log("QuickSandAnimが変なのについてます");
        }
    }
}
