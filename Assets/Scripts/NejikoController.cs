using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1.0f;//1m
    const int DefaultLife = 3;
    const float StunDuration = 0.5f;

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;
    int life = DefaultLife;
    float recoverTime = 0.0f;

    public float gravity; //自分で重力をつける
    public float speedZ;
    public float speedX;//X軸方向の移動速度
    public float speedJump;
    public float accelerationZ;//加速度
    
    public int Life()
    {
        return life;
    }

    bool IsStun()//recoverTimeに値が入っているときかlifeが０以下のときTrueを返す（気絶状態）
    {
        return recoverTime > 0.0f || life <= 0;
    }
    void Start()
    {
        //必要なコンポーネントを自動取得
        //フィールドで宣言しておいてスタートで実際のコンポーネントの参照取得
        //いきなりGetComponentでこのスクリプトがアタッチしてあるゲームオブジェクトからComponentを取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //デバック用
        if (Input.GetKeyDown("left")) MoveToLeft();　//GetKeyだったら押している間ずっとTrue
        if (Input.GetKeyDown("right")) MoveToRight();　//GetKeyDownは押しさげしたときに1回True（押した回数だけTrue）
        if (Input.GetKeyDown("space")) Jump();　//

        if (IsStun())//気絶状態がTrueだったら
        {
            //動きを止め、気絶状態からの復帰カウントを進める
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;//0.0168を1秒間に60回（＝1）を引く　0.5秒で0になる
            //ぶつかったらrecoverTimeにStunDuration=0.5が入る（下のところ）

            /*
             transfer.Translate(0,0,1)1秒間に1進
            1秒間割るフレーム数がTime.deltaTime
            trasfer.rotate(0,60,0)*Time.deltaTime 1秒間に60度回る
            Time.deltatime書けてなかったら　60×60回/秒で3600度回る
             */
        }
        else
        {
            //徐々に加速しZ方向に常に前進させる
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);//accelerationZ=10を代入　1秒間に10(m)進速度で加速
            //moveDirection.zはベクトル。最初0　最初のフレームで10に0.0168をかけた値になる　その次のフレームで0.32 
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);//speedzが最大速度= 5　Clampは第一引数を第２引数以上第３引数以内

            //X方向は目標のポジションまでの差分の割合で速度を計算
            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth; //transform.position.xはネジコの位置最初0
            //targetLane=-2~2

            moveDirection.x = ratioX * speedX;//speedX=3を代入　
            //前進5右に3を足したベクトルでネジコは移動
            //ネジコ右に行くのか左にいくかをやりたい（方向成分がほしい）
            //横方向の移動速度を求めている
            //最初早くて（遠いと早くて）最後（近づくと）ゆっくりの速度
            //-6~6の速度で移動している
        }

        /*
        //キャラクターコンポーネントはisGroundedというプロパティを持っている
        //足元が接触していたらTrue
        //足が地面に接地しているときのみプレイヤーのキー入力を受け付ける
        if (controller.isGrounded) {
            
            //Input.GetAxis↑に押すと正（１）、下を押すと負（ー１） 
            //キーボードの上下やじるしキーのことをvertical
            //0以上の値を利用することで、キャラクターがバックしないようにしている
            if(Input.GetAxis("Vertical") > 0.0f) {

                //上下の入力を前進に切り替え
                moveDirection.z = Input.GetAxis("Vertical") * speedZ;

            } else {
                //押してないとき、接地してないときは止まる
                moveDirection.z = 0;
            }
            //Horizontalは左右キーに対応　右押すと時計方向に回る　左押すと反時計回りに回る
            //Y軸を中心にキャラクターを回転させる
            transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);

            if (Input.GetButton("Jump")) {
                //ジャンプを押されたらy軸方向に力を入れる
                moveDirection.y = speedJump;
                //ジャンプボタンを押したら一回ジャンプをする
                animator.SetTrigger("jump");
                //InputのJumpに割り当てられているキー入力があった場合、上方向にJumpSpeedを適用します。
                //同時にAnimatorに対してJumpトリガーを渡し、アニメーションの切り替えを行う
            }
        }
        */

        //重力分の力を毎フレーム追加（フレームごとに重力分のベロシティを下方に加えます。）
        //Time.deltaTime Timetimeゲームが始まってからの時間
        //Time.timescale　時間の進行速度（timescale1 で1秒間に1進む）
        //Time.deltatime１フレームにかかっている時間（１秒間に60回なので1フレーム0.0168くらい）
        //どの端末でしても速度差が出ないようにするためにTime.deltatimeは必要
        //(おんぼろマシーンでしたら1秒間に30でする　なので1フレーム0.3くらい)
        moveDirection.y -= gravity * Time.deltaTime;

        //移動実行
        //ネジコちゃんが向いている方向に進ませたいので向いている方向を加味したベクトルにする
        //transformが持っているtransformDirectionメソッドを使用してtransform（ネジコちゃんが向いている方向）を考慮したベクトルにする
        //引数はzの値　moveDirectionはもともと0だけど、キー入力でz軸やｙ軸に数値が入る
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        //ネジコ回転要素なくなったのであまり意味はない

        //Move関数で動く globalDirectionはネジコちゃんが向いている方向を加味したベクトルになっているのでネジコちゃんが向いている方向に進む
        //キャラクターコントローラーのこと　キャラクターコントローラーとはネジコちゃんに最初にアタッチしたネジコちゃんの動きを管理しているコンポーネント
        //Time.deltatimeで1秒間にそのGlobalDirectionだけ移動する
        controller.Move(globalDirection * Time.deltaTime);

        //移動後設置してたらY方向の速度はリセットする
        //接地してたら下方向に力くわえなくていい→接地してたら下方向移動0にする
        //上方向に入れているyの値を0にする（しないとyの値が引き続けているので負になっちゃう）
        // moveDirection.y -= gravity * Time.deltaTime;
        if (controller.isGrounded) {
            moveDirection.y = 0;
        }
        //速度が0以上なら走っているフラグをTrueにする
        //ネジコちゃんについているAnimatorコンポーネント（見た目の制御）のセットブールメソッド
        //runというパラメータに対してmoveDirection.zに正の値が入っているか（上の矢印が押されているか）
        //押されていたらTrueが入る
        animator.SetBool("run", moveDirection.z > 0f);
        //Trueになるとボタンのチェックが入って走る

        //Controllerで空間制御、Animatorで見た目の制御
    }

    //左のレーンに移動を開始
    public void MoveToLeft()
    {
        if (IsStun()) { return; }
        if (controller.isGrounded && targetLane > MinLane)
        {
            targetLane--;
        }
    }

    //右のレーンに移動を開始
    public void MoveToRight()
    {
        if (IsStun()) { return; }
        if (controller.isGrounded && targetLane < MaxLane)
        {
            targetLane++;
        }
    }

    public void Jump()
    {
        if (IsStun()) { return; }
        if (controller.isGrounded)
        {
            moveDirection.y = speedJump;

            //ジャンプトリガーを設定
            animator.SetTrigger("jump");
        }
    }

    //CharacterControllerに衝突判定が生じたときの処理
    void OnControllerColliderHit(ControllerColliderHit hit)
        //キャラもの動かすときRigidbody or CharacterControllerのどちらか
        //CharacterController重力自分で実装しなければいけない
        //キャラコン使いうときの衝突判定はOnControllerColliderHit
        //rigidBody はonColligionEnter
        //isTriggerのチェックが入っているときはOnTriggerEnter
    {
        if (IsStun()) { return; }//スタン中にスタンならないように

        if(hit.gameObject.tag == "Robo")
        {
            //ライフを減らして気絶状態に移行
            life--;
            recoverTime = StunDuration;

            //ダメージトリガーを設定
            animator.SetTrigger("damage");

            //ヒットしたオブジェクトは削除
            Destroy(hit.gameObject);
        }
    }
}
