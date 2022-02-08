using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    const int StageChipSize = 30;

    int currentChipIndex; //生成済みの最大のインデックス（一番前（のインデックス））

    public Transform character;//ネジコのトランスフォーム型
    public GameObject[] stageChips;　//どのステージを登場させるか（ステージの種類が後で増える）　
    public int startChipIndex;　//1が入る
    public int preInstantiate;//inspectorから登録（5)
    public List<GameObject> generatedStageList = new List<GameObject>();//ListはNewしないとつかえないから使えるようにしている
    //左側は変数名あるからNewつけられない

    void Start()
    {
        currentChipIndex = startChipIndex - 1;//最初だけ０
        UpdateStage(preInstantiate);//そのあと更新されていく
    }


    void Update()
    {
        //キャラクターの位置から現在のステージチップのインデックスを計算
        int charaPositionIndex = (int)(character.position.z / StageChipSize);

        //次のステージチップに入ったらステージの更新処理を行う
        if (charaPositionIndex + preInstantiate > currentChipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);//currentIndexより大きい値が引数
        }
    }

    //指定のIndexまでのステージチップを生成して、管理下に置く
    void UpdateStage(int toChipIndex)
    {
        if (toChipIndex <= currentChipIndex)//一度もTrueにならない
        {
            return;
        }

        //指定のステージチップまでを作成
        for (int i = currentChipIndex + 1; i <= toChipIndex; i++)
        {
            GameObject stageObject = GenerateStage(i);

            //生成したステージチップを管理リストに追加
            generatedStageList.Add(stageObject);
        }
        //ステージ保持上限内になるまで古いステージを削除
        while (generatedStageList.Count > preInstantiate + 2) DestroyOldestStage();
        //preInstantiate=5+2=7より大きくなったら

        currentChipIndex = toChipIndex;
    }

    //指定のインデックス位置にStageオブジェクトをランダムに生成
    GameObject GenerateStage(int chipIndex)
    {
        Debug.Log("ok");
        int nextStageChip = Random.Range(0, stageChips.Length);//先々いろんなステージがあるからランダム

        GameObject stageObject = (GameObject)Instantiate(　//ダウンキャスト不要

            stageChips[nextStageChip],//何を
            new Vector3(0, 0, chipIndex * StageChipSize),//どこに
            Quaternion.identity//どの方向で　回転無し
            );

        return stageObject;
    }

    //一番古いステージを削除
    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        //ls.add("John")
        //ls.get(0)
        generatedStageList.RemoveAt(0);//リストから削除（インデックス指定（先頭の要素））
        Destroy(oldStage);//オブジェクト自体はこちらで削除
    }
}