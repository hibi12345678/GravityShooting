using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationUpper: MonoBehaviour
{

    public GameObject bottom;  // 回転を取得するオブジェクト
    public GameObject target;  // 回転を適用するオブジェクト

    void Update()
    {
        // bottom のローカルのローテーションを取得
        Quaternion localRotation = bottom.transform.localRotation;

        // ローカルのローテーションをオイラー角で取得
        Vector3 localEulerAngles = localRotation.eulerAngles;

        // x軸の回転角度を1/2にする
        localEulerAngles.x =localEulerAngles.x;

        // 変更したオイラー角をクォータニオンに戻す
        Quaternion adjustedRotation = Quaternion.Euler(localEulerAngles);

        // target に適用
        target.transform.localRotation = adjustedRotation;
    }
}
