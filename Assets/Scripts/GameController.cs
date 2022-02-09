using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;　//書かないとテキスト型が認識できない

public class GameController : MonoBehaviour
{
    public NejikoController nejiko;
    public Text scoreText;
    public LifePanel lifePanel;

    public void Update()
    {
        //スコアを更新
        int score = CalcScore();
        scoreText.text = $"Score:{score}m";
        //"Score : " + score + "m";

        //ライフパネルを更新
        lifePanel.UpdateLife(nejiko.Life());
    }

    int CalcScore()
    {
        //ねじ子の走行距離をスコアとする
        return (int)nejiko.transform.position.z;
        //nejiko.=ネジココントローラー（型）
        //正式には　NejikoController.GameObject.GetComponent<transform>();
    }
}
