using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePanel : MonoBehaviour
{
    public GameObject[] icons;　//3つのアイコンを登録

    //ライフに応じてスプライトを出しわける
    public void UpdateLife(int life)　//life＝3
    {
        for (int i=0; i<icons.Length; i++)
        {
            if (i < life)
            {
                //SetActive関数はゲームオブジェクトの有効、無効の設定を切り替える関数
                //そのゲームオブジェクトが有効か無効かを決めるメソッド
                //超重要
                icons[i].SetActive(true);
            }
            else
            {
                icons[i].SetActive(false);
            }
        }
    }
    
}
