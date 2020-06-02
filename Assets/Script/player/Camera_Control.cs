using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    //　キャラクターのTransform
    [SerializeField]
    private Transform charaLookAtPosition;
    //　カメラの移動スピード
    [SerializeField]
    private float cameraMoveSpeed = 2f;
    //　カメラの回転スピード
    [SerializeField]
    private float cameraRotateSpeed = 90f;
    //　カメラのキャラクターからの相対値を指定
    [SerializeField]
    private Vector3 basePos = new Vector3(0f, 0f, 2f);
    // 障害物とするレイヤー
    [SerializeField]
    private LayerMask obstacleLayer;

    public Transform Target;
    public float DistanceToPlayerM = 25f;    // カメラとプレイヤーとの距離[m]
    public float SlideDistanceM = 0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    public float HeightM = 5.0f;            // 注視点の高さ[m]
    public float RotationSensitivity = 100f;// 感度
    float rotX;
    float rotY;

    void Start()
    {
        DistanceToPlayerM = 25f;    // カメラとプレイヤーとの距離[m]
        HeightM = 5.0f;            // 注視点の高さ[m]
        if (Target == null)
        {
            Debug.LogError("ターゲットが設定されていない");
            Application.Quit();
        }
        charaLookAtPosition = GameObject.Find("player").transform;
        // カーソル非表示
        Cursor.visible = false;
    }

    void Update()
    {
        //　通常のカメラ位置を計算
        var cameraPos = charaLookAtPosition.position + (-charaLookAtPosition.forward * basePos.z) + (Vector3.up * basePos.y);
        //　カメラの位置をキャラクターの後ろ側に移動させる
        //transform.position = Vector3.Lerp(transform.position, cameraPos, cameraMoveSpeed * Time.deltaTime);

        RaycastHit hit;
        //　キャラクターとカメラの間に障害物があったら障害物の位置にカメラを移動させる
        if (Physics.Linecast(charaLookAtPosition.position, transform.position, out hit, obstacleLayer))
        {
            transform.position = hit.point;
        }
        //　レイを視覚的に確認
        Debug.DrawLine(charaLookAtPosition.position, transform.position, Color.red, 0f, false);

        //　スピードを考慮しない場合はLookAtで出来る
        //transform.LookAt(charaTra.position);
        //　スピードを考慮する場合
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(charaLookAtPosition.position - transform.position), cameraRotateSpeed * Time.deltaTime);
    }


    void FixedUpdate()
    {
        rotX = (Input.GetAxisRaw("Horizontal2") * 0.8f * Time.deltaTime * RotationSensitivity) + (Input.GetAxis("Mouse X") * Time.deltaTime * RotationSensitivity * 4);
        rotY = (Input.GetAxisRaw("Vertical2") * 0.8f * Time.deltaTime * RotationSensitivity) + (Input.GetAxis("Mouse Y") * Time.deltaTime * -RotationSensitivity * 4);
        var lookAt = Target.position + Vector3.up * HeightM;

        // 回転
        transform.RotateAround(lookAt, Vector3.up, rotX);
        // カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
        if (transform.forward.y > 0.9f && rotY < 0)
        {
            rotY = 0;
        }
        if (transform.forward.y < -0.9f && rotY > 0)
        {
            rotY = 0;
        }
        transform.RotateAround(lookAt, transform.right, rotY);

        // カメラとプレイヤーとの間の距離を調整
        transform.position = lookAt - transform.forward * DistanceToPlayerM;

        // 注視点の設定
        transform.LookAt(lookAt);

        // カメラを横にずらして中央を開ける
        transform.position = transform.position + transform.right * SlideDistanceM;
    }
}