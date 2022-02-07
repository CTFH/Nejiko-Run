using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero; 
    public float gravity; //自分で重力をつける
    public float speedZ;
    public float speedJump;　
    
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
        //キャラクターコンポーネントはisGroundedというプロパティを持っている
        //足元が接触していたらTrue
        //足が地面に接地しているときのみプレイヤーのキー入力を受け付ける
        if (controller.isGrounded) {
            
            //Input.GetAxis↑に押すと正（１）、下を押すと負（ー１） 
            //キーボードの上下やじるしキーのことをvertical
            if(Input.GetAxis("Vertical") > 0.0f) {

                //上下の入力を前進に切り替え
                moveDirection.z = Input.GetAxis("Vertical") * speedZ;

            } else {
                //押してないとき、接地してないときは止まる
                moveDirection.z = 0;
            }
            //Horizontalは左右キーに対応　右押すと時計方向に回る　左押すと反時計回りに回る
            transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);

            if (Input.GetButton("Jump")) {
                //ジャンプを押されたらy軸方向に力を入れる
                moveDirection.y = speedJump;
                //ジャンプボタンを押したら一回ジャンプをする
                animator.SetTrigger("jump");
            }
        }
        //重力分の力を毎フレーム追加
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

        //Move関数で動く globalDirectionはネジコちゃんが向いている方向を加味したベクトルになっているのでネジコちゃんが向いている方向に進む
        //キャラクターコントローラーのこと　キャラクターコントローラーとはネジコちゃんに最初にアタッチしたネジコちゃんの動きを管理しているコンポーネント
        //Time.deltatimeで1秒間にそのGlobalDirectionだけ移動する
        controller.Move(globalDirection * Time.deltaTime);

        //移動後設置してたらY方向の速度はリセットする
        //上方向に入れているyの値を0にする（しないとyの値が引き続けているので負になっちゃう）
        if (controller.isGrounded) {
            moveDirection.y = 0;
        }
        //速度が0以上なら走っているフラグをTrueにする
        //ネジコちゃんについているAnimatorコンポーネント（見た目の制御）のセットブールメソッド
        //runというパラメータに対してmoveDirection.zに正の値が入っているか（上の矢印が押されているか）
        //押されていたらTrueが入る
        animator.SetBool("run", moveDirection.z > 0f);
        
        //Controllerで空間制御、Animatorで見た目の制御
    }
}
