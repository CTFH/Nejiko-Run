using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;　//書かないとテキスト型が認識できない
using UnityEngine.SceneManagement; //名前空間インポート

public class GameController : MonoBehaviour
{
    public NejikoController nejiko; //Component
    //Transform型 nejiko
    //GameObject型 nejiko
    public Text scoreText; //Component
    public LifePanel lifePanel; //Component

    public void Update()
    {
        //スコアを更新
        int score = CalcScore();
        scoreText.text = $"Score:{score}m";
        //"Score : " + score + "m";
        //ScoreTextはコンポーネントで　.textはプロパティ

        //ライフパネルを更新
        lifePanel.UpdateLife(nejiko.Life());
        //ネジコがトランスフォーム型のとき
        //nejiko.gameObject.GetComponent<NejikoController>().Life();

        //GameObject型の時
        //nejiko.GetComponent<NejikoController>().Life();

        //.LifeはNejikoControllerのライフメソッド

        //ネジコのライフが0になったらゲームオーバー
        if(nejiko.Life() <= 0)
        {
            //これ以降のUpdateは止める
            enabled = false; //EnableはGameControllerのチェックを外す
                             //（GameObjectの有効はSetActiveで設定）
                             //通常GetComponent<>().enabledだけど、今回は自分自身なのでそのままenabled
                             //setActiveはメソッド（なので（）が必要）、enableはプロパティ

            //ハイスコアを更新
            //データの保存はデータベース化ファイルに保存するかのどちらか
            //ちょっとしたデータならローカルにファイルを保持
            //PlayerPrefsがそれ（データの保存）に該当　もともと用意されているクラス
            if (PlayerPrefs.GetInt("HighScore")<score)
            {
                PlayerPrefs.SetInt("HighScore", score);
                //setAttributeに似てる（"ラベル", 値）　
            }

            //2秒後にReturnToTitleを呼び出す
            Invoke("ReturnToTitle", 2.0f);
            //第一引数の関数を第二引数の時間数後に呼び出す
        }
    }

    int CalcScore()
    {
        //ねじ子の走行距離をスコアとする
        return (int)nejiko.transform.position.z;
        //nejiko.=ネジココントローラー（型）
        //正式には　NejikoController.gameObject.GetComponent<transform>(); ()の後は.propertyやメソッドなどがくる

        //transform型
        //nejiko.position.z;

        //GameObject型の時
        //nejiko.transform.position.z;
    }

    void ReturnToTitle()
    {
        //タイトルシーンに切り替え
        SceneManager.LoadScene("Title");
    }
}
