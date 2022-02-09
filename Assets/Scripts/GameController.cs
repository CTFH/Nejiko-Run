using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;　//書かないとテキスト型が認識できない

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
}
