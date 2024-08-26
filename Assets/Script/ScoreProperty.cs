using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Collections;
public class ScoreProperty : MonoBehaviourPun, IPunObservable
{
    [PunRPC]
    public int[] damage = new int[] { 0, 0, 0, 0 };
    [PunRPC]
    public int[] kill = new int[] { 0, 0, 0, 0 };
    [PunRPC]
    public int[] death = new int[] { 0, 0, 0, 0 };
    
    int[] count = new int[] { 0, 0, 0 ,0};
    public string prefabName;
    
    public Transform parentTransform;  // UIを生成する親オブジェクト
    public float displayTime = 6f;  // UIが表示される時間（秒）
    public Vector3 spawnPosition;  // UIを生成する位置（親オブジェクトの相対位置）
    // ネットワークデータの同期
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 変数の値を他のクライアントに送信
            stream.SendNext(damage);
            stream.SendNext(kill);
            stream.SendNext(death);
        }
        else
        {
            // 他のクライアントから変数の値を受信
            damage = (int[])stream.ReceiveNext();
            kill = (int[])stream.ReceiveNext();
            death = (int[])stream.ReceiveNext();
        }
    }

    // スコアを追加する RPC メソッド

    public void ScoreAdd(int dam,int num)
    {
        damage[num - 1] += dam;
    }


    public void KDAdd(int deathNum,int killNum)
    {
        
        death[deathNum - 1] += 1;
        kill[killNum - 1] += 1;
        prefabName = "Log" + killNum + "-" + deathNum;
        GameObject uiPrefab = Resources.Load<GameObject>(prefabName);
        ShowUI(uiPrefab);
    }
        
    

    [PunRPC]
    // スコアを更新するために RPC を呼び出すメソッド
    public void UpdateScore(int num, int deathNum, int killNum)
    {
        photonView.RPC("ScoreAdd", RpcTarget.All, num);
        photonView.RPC("KDAdd", RpcTarget.All, deathNum, killNum);
    }

    void ShowUI(GameObject uiPrefab)
    {
        int i = 0;
        bool flag = false; 
        for (i = 3; i >= 1; i--)
        {
            if (count[i - 1] == 1 && flag == true)
            {
                spawnPosition = new Vector3(750+ 960, (i*-60)+400 +540,0);
                GameObject uiInstance = Instantiate(uiPrefab, spawnPosition, Quaternion.identity, parentTransform);

                // UIを削除するコルーチンを開始
                StartCoroutine(HideUIAfterTime(uiInstance, displayTime, i+1));
                flag = false;
            }
            else if (i ==1 && flag == false)
            {
                spawnPosition = new Vector3(750 +960, 400+ 540, 0);
                GameObject uiInstance = Instantiate(uiPrefab, spawnPosition, Quaternion.identity, parentTransform);

                // UIを削除するコルーチンを開始
                StartCoroutine(HideUIAfterTime(uiInstance, displayTime, i));
            }
        }


         
    }

    // 指定した時間後にUIを削除するコルーチン
    private IEnumerator HideUIAfterTime(GameObject uiInstance, float time ,int i)
    {
        count[i-1] = 1;
        yield return new WaitForSeconds(time);
        count[i - 1] = 0;
        // UIインスタンスを削除
        Destroy(uiInstance);
    }
}
