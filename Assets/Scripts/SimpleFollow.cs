using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    Vector3 diff;

    public GameObject target; 
    //子要素にすると動きが一緒になるけど今回はしていない

    public float followSpeed;
    void Start()
    {
        //Vector3型でdiff準備　target＝ネジコ
        //ベクトルの引き算　ネジコとカメラの距離
        //原点からネジコ　原点からカメラ　のベクトルの引き算（反対向きの矢印を足す）
        diff = target.transform.position - transform.position;
    }

    void LateUpdate()//他のUpdateが終わった瞬間に走る
        //Update...LateUpdate, Update...LateUpdate
        //UpdateもLateUpdateも1秒間に60回
    {
        //Lerp線形保管　2つのベクトルの第3引数の割合のベクトルを返す
        transform.position = Vector3.Lerp(
            transform.position,　//第一引数A　カメラ
            target.transform.position - diff,//第2引数B　ネジコ
            Time.deltaTime * followSpeed//AとBの中の割合
            //Aから見たBの割合　０はA　Bは1、0.5は中間地点
            );            
    }
}
