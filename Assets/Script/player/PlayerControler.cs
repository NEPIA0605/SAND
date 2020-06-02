using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weight;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class PlayerControler : MonoBehaviour
{
    //変数宣言
    public float PlayerSp;       //プレイヤーの速度
    public bool PlayerTurn;     //プレイヤーの反転 trueなら反転 falseなら通常
    int PlayerTime;     //中砂の時間
    public float PlayerTotalTime;//中砂の合計の時間
    public Vector3 PlayerDir;   //プレイヤーの方向
    public bool ClearFlg;   //クリアフラグ
    Vector3 StartPlayerPos; //プレイヤーの初期位置
    private Rigidbody _rigidbody; //物理判定の速度変数
    bool GameOverFlg;           //ゲームオーバーフラグ、高いところから落ちたときやステージ外へ行ったときtrueとなる
    bool GameOverAnimFlg;       //ゲームオーバーアニメーションフラグ
    bool PlayerFloatFlg;        //プレイヤーが浮いているか
    float PlayerOldVelocity;    //プレイヤーの1フレーム前の加速度
    Vector3 PlayerGravity;        //プレイヤーの重力
    Animator animator;
    bool PlayerTurnAnimFlg;
    float PlayerTurnAnimTime;
    public float PlayerRotInvalidTime;//回転無効時間
    float PlayerGrabity;    //プレイヤーの重力
    bool BlockFlg;
    Vector3 SandSptmp;

    GameObject obj; //壊れるモデル
    public Vector3 PlayerMoveFt;        // かけらの上にいるときの変数
    public bool Wall_Col;               // 壁に触れているかどうか
    public bool FtCol;                  // かけらにふれているかどうか

    public bool PlayerYSandFlg;        //縦の流砂に触れているフラグ
    public bool PlayerXSandFlg;        //横の流砂に触れているフラグ
    bool PlayerYSandAddFlg;
    bool CollisionFlowSand;
    int PlayerMovevelo;
    bool PlayerVeloFlg;

    [SerializeField] bool CollisionSand;         //流砂に触れているかどうか
    [SerializeField] bool CollisionGround;       //床に触れてるかどうか

    [SerializeField] Vector3 SandMoveSp;  //流砂の移動力
    [SerializeField] float FallDeathPos;  //どれだけ高いところから落ちたときか

    [Header("時間")]
    [SerializeField] public float PlayerSandNomalTime;   //通常に流れるほうの砂の時間
    [SerializeField] public float PlayerSandBackTime;    //逆行して流れる砂の時間
    [SerializeField] private bool PlayerEnptyFlg;         //中砂が落ちきっているか判定
    [SerializeField] private Vector3 PlayerGameoverPos;         //ゲームオーバーの位置



    //入力変数
    float inputHorizontal;
    float inputVertical;
    Rigidbody rb;                //当たり判定

    //サウンド用
    [SerializeField] AudioClip[] clips;

    //SEです。
    protected AudioSource Source;
    //[Header("時間")]
    //[SerializeField]float PlayerSandNomalTime;  //通常に流れる中砂
    //[SerializeField]float PlayerSandBackTime;  //逆に流れる中砂

    // Start is called before the first frame update
    void Start()
    {
        //変数初期化
        PlayerEnptyFlg = false;
        PlayerSandBackTime = 0.0f;
        PlayerSandNomalTime = PlayerTotalTime;
        PlayerDir = new Vector3(0.0f, 0.0f, 0.0f);
        SandMoveSp = new Vector3(0.0f, 0.0f, 0.0f);
        SandSptmp = new Vector3(0.0f, 0.0f, 0.0f);
        CollisionSand = false;
        CollisionFlowSand = false;
        CollisionGround = false;
        ClearFlg = false;
        _rigidbody = this.GetComponent<Rigidbody>();
        GameOverFlg = false;
        GameOverAnimFlg = false;
        PlayerFloatFlg = false;
        PlayerOldVelocity = 0.0f;
        PlayerGravity = new Vector3(0.0f, -9.8f, 0.0f); 
        animator = GetComponent<Animator>();
        PlayerTurnAnimFlg = false;
        PlayerTurnAnimTime = 0.0f;
        PlayerRotInvalidTime = 0.8f;
        PlayerYSandFlg = false;
        PlayerXSandFlg = false;
        PlayerYSandAddFlg = false;
        PlayerMovevelo = 0;
        PlayerVeloFlg = false;

        BlockFlg = true;
        obj = (GameObject)Resources.Load("Player_Broken");
        PlayerMoveFt = new Vector3(0.0f, 0.0f, 0.0f);
        Wall_Col = false;
        FtCol = false;

        //初期位置設定
        StartPlayerPos = GameObject.Find("StartPlace").transform.position;
        this.transform.position = StartPlayerPos;
        this.transform.position += new Vector3(0, 5.0f, 0);
        rb = GetComponent<Rigidbody>();

        Source = GetComponent<AudioSource>();
    }



    // Update is called once per frame
    void Update()
    {
        //ポーズ画面処理
        if (Mathf.Approximately(Time.timeScale, 0f)) //時間が止まっていたら、Update処理をしない処理
        {
            return;
        }

        //入力処理
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        //デバッグ
        //Debug.Log("速度ベクトル: " + _rigidbody.velocity);
        Debug.Log("Y:" + PlayerYSandFlg);
        Debug.Log("X:" + PlayerXSandFlg);
        //Debug.Log(rb.velocity);

        
        //Debug.Log(PlayerTurnAnimTime);
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ClearFlg = true;
        }



        if (Input.GetKeyDown("joystick button 6"))
        {
            // 現在のScene名を取得する
            Scene loadScene = SceneManager.GetActiveScene();
            // Sceneの読み直し
            SceneManager.LoadScene(loadScene.name);
        }



        ////クリアしたら移動しないようにする
        //if (ClearFlg == true)
        //{
        //    animator.SetBool("Run", false);
        //    return;
        //}
        //ポーズ画面とreadystartの時動かなくする処理
        if (Mathf.Approximately(Time.timeScale, 0f) || ClearFlg == true) //時間が止まっていたら、Update処理をしない処理
        {
            animator.SetBool("Run", false);
            return;
        }


        //==================================================
        //ゲームオーバー処理
        //==================================================
        //trueになったら
        if (GameOverFlg == true)
        {
            //this.transform.position = StartPlayerPos;
            //this.GetComponent<Rigidbody>().useGravity = false;
            //this.transform.position.y = PlayerGameoverPos.y;

            //this.transform.position = new Vector3(this.transform.position.x, PlayerGameoverPos.y, this.transform.position.z);
            animator.SetBool("Run", false);
            animator.SetBool("Rot", false);

            if (GameOverAnimFlg == true)
            {
                GameObject instance = (GameObject)Instantiate(obj,
                                                        this.transform.position,
                                                       Quaternion.identity);
                GameOverAnimFlg = false;
                //this.SetActive(false);
                this.gameObject.SetActive(false);
            }
            return;
        }

        //===================================================
        //入力処理
        //===================================================
        //上下移動
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            //Debug.Log("上");
            animator.SetBool("Run", true);
        }
        else
        {
            //  PlayerAnimation.SetBool("Run", false);
        }

        //左右移動
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            animator.SetBool("Run", true);
           // Debug.Log("下");
        }
        else
        {
            //PlayerAnimation.SetBool("Run", false);
        }
        //何も入力してないとき
        if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            animator.SetBool("Run", false);

        }

        //=========================================================================================
        //移動処理
        //カメラの方向に準じて移動
        //=========================================================================================
        //カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        PlayerDir = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        //X軸流砂に触れている時
        if (CollisionSand == true)
        {
            //y軸の流砂上かつX軸上でないとき
            if (PlayerYSandFlg && !PlayerXSandFlg)
            {
                //中砂が入っている時
                if (PlayerEnptyFlg == false)
                {
                    //Y軸に力がかかっている時
                    if (SandMoveSp.y != 0 && !PlayerXSandFlg)
                    {
                        this.GetComponent<Rigidbody>().useGravity = false;
                        rb.velocity = PlayerDir * PlayerSp + SandMoveSp;
                        PlayerYSandAddFlg = true;
                    }
                    //X軸とY軸に力がかかっている時
                    else if (PlayerYSandAddFlg == true && PlayerYSandFlg && !PlayerXSandFlg && (SandMoveSp.x != 0.0f || SandMoveSp.z != 0.0f))
                    {
                        PlayerVeloFlg = true;
                        PlayerYSandAddFlg = false;
                    }
                    //X軸に力がかかっている時
                    else if (PlayerYSandAddFlg == false && !PlayerXSandFlg && (SandMoveSp.x != 0 || SandMoveSp.z != 0))
                    {
                    
                        this.GetComponent<Rigidbody>().useGravity = false;
                        rb.velocity =　PlayerDir * PlayerSp + SandMoveSp;
                        PlayerYSandAddFlg = false;
                    }
                }
                //中砂が入っていないとき
                else
                {
                    PlayerYSandAddFlg = false;
                    rb.velocity = PlayerDir * PlayerSp + new Vector3(0, rb.velocity.y, 0);

                    this.GetComponent<Rigidbody>().useGravity = true;
                    //Physics.gravity = new Vector3(0, -20, 0);

                }
                //this.gameObject.transform.position += PlayerDir * PlayerSp * 0.007f + SandMoveSp * 0.007f;
            }
            //
            if (PlayerYSandAddFlg == true && PlayerYSandFlg && PlayerXSandFlg && (SandMoveSp.x != 0 || SandMoveSp.z != 0))
            {
                rb.velocity = new Vector3(0, 10, 0);
                PlayerYSandAddFlg = false;
            }
            //y軸に力がかかっている時
            //else
            //{
            //    if (PlayerEnptyFlg == false)
            //    {
            //        this.GetComponent<Rigidbody>().useGravity = false;
            //    }
            //    //rb.velocity = PlayerDir * PlayerSp + new Vector3(0, rb.velocity.y, 0) + SandMoveSp;
            //    rb.velocity = PlayerDir * PlayerSp + SandMoveSp;
            //    //this.gameObject.transform.position += PlayerDir * PlayerSp * 0.007f + SandMoveSp * 0.007f;
            //}

            //X軸の流砂上かつY軸上でない時
            if (PlayerXSandFlg && !PlayerYSandFlg)
            {
                if (PlayerEnptyFlg == false)
                {
                    this.GetComponent<Rigidbody>().useGravity = false;
                    rb.velocity = PlayerDir * PlayerSp + SandMoveSp;
                }
                else
                {
                    this.GetComponent<Rigidbody>().useGravity = true;
                    rb.velocity = PlayerDir * PlayerSp;

                }
            }

            //X軸の流砂上かつY軸上の時
            if (PlayerXSandFlg && PlayerYSandFlg)
            {
                if (CollisionSand)
                {
                    //中砂が入っている時
                    if (PlayerEnptyFlg == false)
                    {
                        this.GetComponent<Rigidbody>().useGravity = false;
                        rb.velocity = PlayerDir * PlayerSp + SandMoveSp + new Vector3(0, 5, 0);
                    }
                    //中砂が入ってないとき
                    else
                    {
                        this.GetComponent<Rigidbody>().useGravity = true;
                        rb.velocity = PlayerDir * PlayerSp;
                    }
                }
            }
            //rb.velocity = PlayerDir * PlayerSp + SandMoveSp;
            //this.gameObject.transform.position = PlayerDir * PlayerSp + new Vector3(0, this.gameObject.transform.position.y, 0) + SandMoveSp;
        }

        //流れ続ける砂
        if (CollisionFlowSand == true)
        {
            //Y軸に力がかかっている時
            if (SandMoveSp.y != 0)
            {
                this.GetComponent<Rigidbody>().useGravity = false;
                rb.velocity = PlayerDir * PlayerSp + SandMoveSp;
                PlayerYSandAddFlg = true;
            }
            //X軸とY軸に力がかかっている時
            else if (PlayerYSandAddFlg == true && (SandMoveSp.x != 0 || SandMoveSp.z != 0))
            {
                rb.velocity = new Vector3(0, 10, 0);
                PlayerYSandAddFlg = false;
            }
            //X軸に力がかかっているかつ横流砂の時
            else if (PlayerYSandAddFlg == false && PlayerXSandFlg　&& (SandMoveSp.x != 0 || SandMoveSp.z != 0))
            {

                this.GetComponent<Rigidbody>().useGravity = false;
                rb.velocity = PlayerDir * PlayerSp + SandMoveSp;
                PlayerYSandAddFlg = false;
            }
            else if (PlayerYSandAddFlg == true && PlayerXSandFlg && PlayerYSandFlg && (SandMoveSp.x != 0 || SandMoveSp.z != 0))
            {

                this.GetComponent<Rigidbody>().useGravity = false;
                rb.velocity = PlayerDir * PlayerSp + SandMoveSp;
                PlayerYSandAddFlg = false;
            }

            //X軸の流砂上かつY軸上の時
            if (PlayerXSandFlg && PlayerYSandFlg)
            {
                this.GetComponent<Rigidbody>().useGravity = false;
                rb.velocity = PlayerDir * PlayerSp + SandMoveSp + new Vector3(0, 5, 0);
            }
        }

        if (!CollisionFlowSand && !CollisionSand && FtCol)
        {
            this.GetComponent<Rigidbody>().useGravity = false;
            //this.gameObject.transform.position += PlayerDir * PlayerSp * 0.007f;
            rb.velocity = PlayerDir * PlayerSp + new Vector3(0, rb.velocity.y, 0) + PlayerMoveFt;
        }

        //流砂とかけらに触れていない時
        if (!CollisionFlowSand && !CollisionSand && !FtCol)
        {
            this.GetComponent<Rigidbody>().useGravity = false;
            //this.gameObject.transform.position += PlayerDir * PlayerSp * 0.007f;
            rb.velocity = PlayerDir * PlayerSp + new Vector3(0, rb.velocity.y, 0);
        }

        //流砂に触れていないとき
        if (!PlayerXSandFlg && !PlayerYSandFlg)
        {
            this.GetComponent<Rigidbody>().useGravity = true;
        }
        // キャラクターの向きを進行方向に
        if (PlayerDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(PlayerDir);
        }



        //画面外に落ちたとき
        if (this.transform.position.y <= PlayerGameoverPos.y)
        {
            GameOverFlg = true;
        }


        if (PlayerVeloFlg == true)
        {
            rb.velocity = new Vector3(0, 10, 0);
            PlayerMovevelo++;
        }
        if (PlayerMovevelo >= 90)
        {
            PlayerVeloFlg = false;
            PlayerMovevelo = 0;
        }

        //=========================================================================================
        //回転処理
        //=========================================================================================
        //スペースキーまたはAボタンを押したとき
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0"))
        {
            if (PlayerTurnAnimFlg == false)
            {
                PlayerTurnAnimFlg = true;
                //時間逆行から通常へ変換
                if (PlayerTurn == true)
                {

                    animator.SetBool("Rot", true);

                    PlayerEnptyFlg = false;
                    PlayerTurn = false;
                    PlayerSandNomalTime = PlayerTotalTime - PlayerSandBackTime; //通常の中砂 を すべての中砂 から 逆行の中砂 を引いた分にする
                }
                //通常から時間逆行へ変換
                else
                {
                    animator.SetBool("Rot", true);

                    PlayerEnptyFlg = false;
                    PlayerTurn = true;
                    PlayerSandBackTime = PlayerTotalTime - PlayerSandNomalTime;  //逆行の中砂 を 全ての中砂 から 逆行の中砂 を引いた分にする
                }

            }
        }

        if (PlayerTurnAnimFlg == true)
        {
            PlayerTurnAnimTime += Time.deltaTime;
            if (PlayerTurnAnimTime >= PlayerRotInvalidTime)
            {
                PlayerTurnAnimTime = 0.0f;
                animator.SetBool("Rot", false);
                
                PlayerTurnAnimFlg = false;
            }
        }
        //=========================================================================================
        //時間処理
        //=========================================================================================
        //時間逆行の向きの場合
        if (PlayerTurn == true)
        {
            //中砂が残っている場合
            if (PlayerSandBackTime > 0.0f)
            {
                PlayerSandBackTime -= Time.deltaTime;
            }
            //時間逆行の中砂が落ちきった場合
            if (PlayerSandBackTime <= 0.0f)
            {
                PlayerSandBackTime = 0.0f;
                PlayerEnptyFlg = true;
            }
        }

        //通常の向きの場合
        if (PlayerTurn == false)
        {
            //中砂が残っている場合
            if (PlayerSandNomalTime > 0.0f)
            {
                PlayerSandNomalTime -= Time.deltaTime;
            }
            //時間逆行の中砂が落ちきった場合
            if (PlayerSandNomalTime <= 0.0f)
            {
                PlayerSandNomalTime = 0.0f;
                PlayerEnptyFlg = true;
            }
        }

    }

    private void FixedUpdate()
    {
        ////ポーズ画面とreadystartの時動かなくする処理
        //if (Mathf.Approximately(Time.timeScale, 0f) || ClearFlg == true) //時間が止まっていたら、Update処理をしない処理
        //{
        //    animator.SetBool("Run", false);
        //    return;
        //}

        //// 回転しない設定
        //_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        ////======================================================================================================
        ////移動処理
        ////======================================================================================================
        ////カメラの方向から、X-Z平面の単位ベクトルを取得
        //Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        //// 方向キーの入力値とカメラの向きから、移動方向を決定
        //PlayerDir = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

        ////プレイヤーを移動させる
        //if ((BlockFlg == false && CollisionSand == false))
        //{
        //    Debug.Log("A");
        //    this.gameObject.transform.position += (PlayerDir * PlayerSp + PlayerGravity) * Time.deltaTime;
        //}
        //else if (CollisionSand == true && PlayerEnptyFlg == true)
        //{
        //    Debug.Log("B");

        //    this.gameObject.transform.position += (PlayerDir * PlayerSp + SandMoveSp + PlayerGravity) * Time.deltaTime;
        //}
        //else if(CollisionSand == true)
        //{
        //    Debug.Log("C");

        //    this.gameObject.transform.position += (PlayerDir * PlayerSp + SandMoveSp) * Time.deltaTime;
        //}
        //else if (BlockFlg == true)
        //{
        //    Debug.Log("D");

        //    this.gameObject.transform.position += (PlayerDir * PlayerSp + PlayerGravity) * Time.deltaTime;

        //}
        //// キャラクターの向きを進行方向に
        //if (PlayerDir != Vector3.zero)
        //{
        //    transform.rotation = Quaternion.LookRotation(PlayerDir);
        //}
    }


    //１F前の加速度を取得
    void LateUpdate()
    {
        PlayerOldVelocity = _rigidbody.velocity.y;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Clear")
        {
            ClearFlg = true;
        }

        if (collision.gameObject.tag == "Wall")
        {
            Wall_Col = true;
        }

        if (collision.gameObject.tag == "Fragment")
        {
            FtCol = true;

            // 流砂に触れていなくて、かけらに触れているとき（乗り時）
            if (CollisionSand == false)
            {
                // 床・壁の砂に触れているかどうか
                bool Ft_SandCol_X = collision.gameObject.GetComponent<Fragment>().GetSandCol_X();
                bool Ft_SandCol_Y = collision.gameObject.GetComponent<Fragment>().GetSandCol_Y();

                // かけらが壁に触れているか
                bool Ft_WallCol = collision.gameObject.GetComponent<Fragment>().GetWallCol();

                // 壁に触れているかけらに触れているかどうか
                bool Ft_WallColFt = collision.gameObject.GetComponent<Fragment>().GetWallColFt();

                bool SandColFt = collision.gameObject.GetComponent<Fragment>().GetFt_Col();

                // 床の流砂に流れてる欠片の処理
                if ((Ft_SandCol_X))
                {
                    PlayerMoveFt = collision.gameObject.GetComponent<Fragment>().GetSandMoveFtSp();
                    PlayerMoveFt *= 50;

                    // 壁の流砂・かけらが壁に触れてる・壁に触れてるかけらに乗ってる時に流されないようにする処理
                    if ((Ft_SandCol_Y) || (Ft_WallCol) || Ft_WallColFt)
                    {
                        PlayerMoveFt = new Vector3(0.0f, 0.0f, 0.0f);
                    }
                }

                // かけらの上のかけらに乗っているときの処理
                if (SandColFt)
                {
                    PlayerMoveFt = collision.gameObject.GetComponent<Fragment>().GetFragmentMoveFt();
                    PlayerMoveFt *= 50;
                    // 壁の流砂・かけらが壁に触れてる・壁に触れてるかけらに乗ってる時に流されないようにする処理
                    if ((Ft_SandCol_Y) || (Ft_WallCol) || Ft_WallColFt)
                    {
                        PlayerMoveFt = new Vector3(0.0f, 0.0f, 0.0f);
                    }

                }
            }
        }

        //流砂
        if (collision.gameObject.tag == "QuickSand_B")
        {
            CollisionSand = true;

            //Vector3 tmp = other.gameObject.GetComponent<Quicksand>().GetSandMove();
            //Debug.Log(other.gameObject.GetComponent<Quicksand>().GetSandMove());

            ////yが大きい時に優先する
            //if (SandMoveSp.y < tmp.y)
            //{
            //    SandMoveSp.y = tmp.y;
            //}
            ////空っぽの時
            //else if (PlayerEnptyFlg)
            //{
            //    SandMoveSp = tmp;
            //}
            //傾いてないとき
            if (collision.gameObject.GetComponent<Quicksand>().GetRotSand().x % 180.0f == 0.0f &&
            collision.gameObject.GetComponent<Quicksand>().GetRotSand().z % 180.0f == 0.0f)
            {
                PlayerXSandFlg = true;
            }
            else
            {
                PlayerYSandFlg = true;
            }
            SandMoveSp = collision.gameObject.GetComponent<Quicksand>().GetSandMove();
        }
        //ずっと流れる流砂
        if (collision.gameObject.tag == "Mud")
        {
            CollisionFlowSand = true;

            //Vector3 tmp = other.gameObject.GetComponent<FlowingSand>().GetFlowingSandMove();
            ////yが大きい時に優先する
            //if (SandMoveSp.y < tmp.y)
            //{
            //    SandMoveSp.y = tmp.y;
            //}
            ////空っぽの時
            //else if (PlayerEnptyFlg)
            //{
            //    SandMoveSp = tmp;
            //}
            if (collision.gameObject.GetComponent<FlowingSand>().GetRotFlowingSand().x % 180.0f == 0.0f &&
            collision.gameObject.GetComponent<FlowingSand>().GetRotFlowingSand().z % 180.0f == 0.0f)
            {
                PlayerXSandFlg = true;
            }
            else
            {
                PlayerYSandFlg = true;
            }

            SandMoveSp = collision.gameObject.GetComponent<FlowingSand>().GetFlowingSandMove();

            //SandMoveSp = other.gameObject.GetComponent<Quicksand>().GetSandMove();
        }

        //if (collision.gameObject.tag == "Block")
        //{
        //    BlockFlg = true;
        //}

        //接触したオブジェクトのタグが"Block"のとき(SE用)
        if (collision.gameObject.tag == "Block")
        {
            CollisionGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Clear")
        {
            ClearFlg = false;
        }
        if (collision.gameObject.tag == "Wall")
        {
            Wall_Col = false;
        }
        if (collision.gameObject.tag == "Fragment")
        {
            FtCol = false;
            //PlayerMoveFt = new Vector3(0.0f,0.0f,0.0f);
        }
        if (collision.gameObject.tag == "Block")
        {
            CollisionGround = false;
        }
        //流砂
        if (collision.gameObject.tag == "QuickSand_B")
        {
            //this.GetComponent<Rigidbody>().useGravity = true;

            //Debug.Log("流はなれ");
            CollisionSand = false;
            SandMoveSp = new Vector3(0.0f, 0.0f, 0.0f);

            if (collision.gameObject.GetComponent<Quicksand>().GetRotSand().x % 180.0f == 0.0f &&
            collision.gameObject.GetComponent<Quicksand>().GetRotSand().z % 180.0f == 0.0f)
            {
                PlayerXSandFlg = false;
            }
            else
            {
                PlayerYSandFlg = false;
            }
        }

        //ずっと流れる流砂
        if (collision.gameObject.tag == "Mud")
        {
            CollisionFlowSand = false;
            SandMoveSp = new Vector3(0.0f, 0.0f, 0.0f);

            if (collision.gameObject.GetComponent<FlowingSand>().GetRotFlowingSand().x % 180.0f == 0.0f &&
            collision.gameObject.GetComponent<FlowingSand>().GetRotFlowingSand().z % 180.0f == 0.0f)
            {
                PlayerXSandFlg = false;
            }
            else
            {
                PlayerYSandFlg = false;
            }
        }

        if (collision.gameObject.tag == "Block")
        {
            BlockFlg = false;
        }
    }

    //流砂の処理(板ver)とか
    private void OnCollisionStay(Collider other)
    {

    }

    //流砂から離れるときに流砂の影響を消す　　とか
    private void OnTriggerExit(Collider other)
    {
        ////流砂
        //if (other.gameObject.tag == "QuickSand_B")
        //{
        //    //this.GetComponent<Rigidbody>().useGravity = true;

        //    //Debug.Log("流はなれ");
        //    CollisionSand = false;
        //    SandMoveSp = new Vector3(0.0f, 0.0f, 0.0f);
        //}
        ////ずっと流れる流砂
        //if (other.gameObject.tag == "Mud")
        //{
        //    CollisionSand = false;
        //    //this.GetComponent<Rigidbody>().useGravity = false;
        //    SandMoveSp = new Vector3(0.0f, 0.0f, 0.0f);
        //}
        //if (other.gameObject.tag == "Block")
        //{
        //    BlockFlg = false;
        //}
    }
    //なにかと当たった時
    private void OnCollisionEnter(Collision collision)
    {
        //高いところから落ちたとき
        if (PlayerOldVelocity <= FallDeathPos)
        {
            GameOverAnimFlg = true;
            GameOverFlg = true;
        }
    }

    //プレイヤーが反転しているかどうかの変数Getter
    public bool GetPlayerTurn()
    {
        return PlayerTurn;
    }

    //プレイヤーの中砂が流れ落ちているかどうかの変数のGetter
    public bool GetPlayerEnpty()
    {
        return PlayerEnptyFlg;
    }

    //プレイヤーのカメラに対する向き
    public Vector3 GetPlayerDir()
    {
        return PlayerDir;
    }

    //プレイヤーのワールドでの向き
    public Vector3 GetPlayerRot()
    {
        return transform.localEulerAngles;
    }

    //プレイヤーのゲームオーバー判定
    public bool GetGameOverFlg()
    {
        return GameOverFlg;
    }


    //プレイヤーのゲームオーバー判定
    public bool GetGameClearFlg()
    {
        return ClearFlg;
    }

    //プレイヤーの時間(α版用)
    public float GetPlayerNormalTime()
    {
        return PlayerSandNomalTime;
    }
    //プレイヤーの時間(α版用)
    public float GetPlayerBackTime()
    {
        return PlayerSandBackTime;
    }

    public bool GetWallCol()
    {
        return Wall_Col;
    }

    public bool GetFtCol()
    {
        return FtCol;
    }

    public bool GetPlayerTurnAnimFlg()
    {
        return PlayerTurnAnimFlg;
    }

    //足音
    public void PlaySE()
    {
        if (CollisionGround == true)
        {
            Source.PlayOneShot(clips[0]);
        }
    }

    //時間逆行
    public void PlaySE_Time()
    {
        Source.PlayOneShot(clips[2]);
    }
}
