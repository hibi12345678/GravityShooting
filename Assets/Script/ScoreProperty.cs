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
    
    public Transform parentTransform;  // UI�𐶐�����e�I�u�W�F�N�g
    public float displayTime = 6f;  // UI���\������鎞�ԁi�b�j
    public Vector3 spawnPosition;  // UI�𐶐�����ʒu�i�e�I�u�W�F�N�g�̑��Έʒu�j
    // �l�b�g���[�N�f�[�^�̓���
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �ϐ��̒l�𑼂̃N���C�A���g�ɑ��M
            stream.SendNext(damage);
            stream.SendNext(kill);
            stream.SendNext(death);
        }
        else
        {
            // ���̃N���C�A���g����ϐ��̒l����M
            damage = (int[])stream.ReceiveNext();
            kill = (int[])stream.ReceiveNext();
            death = (int[])stream.ReceiveNext();
        }
    }

    // �X�R�A��ǉ����� RPC ���\�b�h

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
    // �X�R�A���X�V���邽�߂� RPC ���Ăяo�����\�b�h
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

                // UI���폜����R���[�`�����J�n
                StartCoroutine(HideUIAfterTime(uiInstance, displayTime, i+1));
                flag = false;
            }
            else if (i ==1 && flag == false)
            {
                spawnPosition = new Vector3(750 +960, 400+ 540, 0);
                GameObject uiInstance = Instantiate(uiPrefab, spawnPosition, Quaternion.identity, parentTransform);

                // UI���폜����R���[�`�����J�n
                StartCoroutine(HideUIAfterTime(uiInstance, displayTime, i));
            }
        }


         
    }

    // �w�肵�����Ԍ��UI���폜����R���[�`��
    private IEnumerator HideUIAfterTime(GameObject uiInstance, float time ,int i)
    {
        count[i-1] = 1;
        yield return new WaitForSeconds(time);
        count[i - 1] = 0;
        // UI�C���X�^���X���폜
        Destroy(uiInstance);
    }
}
