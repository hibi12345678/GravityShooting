using System;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI kill1 = default;
    public TextMeshProUGUI kill2 = default;
    public TextMeshProUGUI kill3 = default;
    public TextMeshProUGUI kill4 = default;
    public TextMeshProUGUI death1 = default;
    public TextMeshProUGUI death2 = default;
    public TextMeshProUGUI death3 = default;
    public TextMeshProUGUI death4 = default; 
    public TextMeshProUGUI damage1 = default;
    public TextMeshProUGUI damage2 = default;
    public TextMeshProUGUI damage3 = default;
    public TextMeshProUGUI damage4 = default;
    public TextMeshProUGUI name1 = default;
    public TextMeshProUGUI name2 = default;
    public TextMeshProUGUI name3 = default;
    public TextMeshProUGUI name4 = default;
    

    public int team1, team2;
    
    private float elapsedTime;
    public GameObject mainScene;
    ScoreProperty scoreProperty;
    
    void Start()
    {  
        elapsedTime = 0f;
        PhotonView photonView;
        photonView = GetComponent<PhotonView>();


    }

    void Update()
    {
        // まだルームに参加していない場合は更新しない
        if (!PhotonNetwork.InRoom) { return; }

        // 0.1秒毎にテキストを更新する
        elapsedTime += Time.deltaTime;
        mainScene = GameObject.Find("MainScene");
        scoreProperty = mainScene.GetComponent<ScoreProperty>();
        if (elapsedTime > 0.1f)
        {
            elapsedTime = 0f;

            if (gameObject.layer == 7)
            {
 
                 UpdateLabelsFriend(scoreProperty.kill[0], scoreProperty.kill[2], kill1, kill2); 
                 UpdateLabelsEnemy(scoreProperty.kill[1], scoreProperty.kill[3], kill3, kill4);
                 team1 = scoreProperty.kill[0] + scoreProperty.kill[2];
                 team2 = scoreProperty.kill[1] + scoreProperty.kill[3];
                 kill1.text = scoreProperty.kill[0].ToString();
                 kill2.text = scoreProperty.kill[2].ToString();
                 kill3.text = scoreProperty.kill[1].ToString();
                 kill4.text = scoreProperty.kill[3].ToString();

                 death1.text = scoreProperty.death[0].ToString();
                 death2.text = scoreProperty.death[2].ToString();
                 death3.text = scoreProperty.death[1].ToString();
                 death4.text = scoreProperty.death[3].ToString();

                 damage1.text = scoreProperty.damage[0].ToString();
                 damage2.text = scoreProperty.damage[2].ToString();
                 damage3.text = scoreProperty.damage[1].ToString();
                 damage4.text = scoreProperty.damage[3].ToString();

                 name1.text = "Player1";
                 name2.text = "Player3";
                 name3.text = "Player2";
                 name4.text = "Player4";


            }

            else if (gameObject.layer == 8)
            {

                UpdateLabelsFriend(scoreProperty.kill[1], scoreProperty.kill[3], kill1, kill2);
                UpdateLabelsEnemy(scoreProperty.kill[0], scoreProperty.kill[2], kill3, kill4);

                kill3.text = scoreProperty.kill[0].ToString();
                kill4.text = scoreProperty.kill[2].ToString();
                kill1.text = scoreProperty.kill[1].ToString();
                kill2.text = scoreProperty.kill[3].ToString();

                death3.text = scoreProperty.death[0].ToString();
                death4.text = scoreProperty.death[2].ToString();
                death1.text = scoreProperty.death[1].ToString();
                death2.text = scoreProperty.death[3].ToString();

                damage3.text = scoreProperty.damage[0].ToString();
                damage4.text = scoreProperty.damage[2].ToString();
                damage1.text = scoreProperty.damage[1].ToString();
                damage2.text = scoreProperty.damage[3].ToString();

                name3.text = "Player1";
                name4.text = "Player3";
                name1.text = "Player2";
                name2.text = "Player4";

            }



        }
    }

    void UpdateLabelsFriend(int score1, int score2, TextMeshProUGUI labelA, TextMeshProUGUI labelB)
    {

        if (score1 >= score2)
        {
            labelA.rectTransform.anchoredPosition = new Vector3(-25, 25, 0);
            labelB.rectTransform.anchoredPosition = new Vector3(-25, -25, 0);
        }
        else
        {
            labelA.rectTransform.anchoredPosition = new Vector3(-25, -25, 0);
            labelB.rectTransform.anchoredPosition = new Vector3(-25, 25, 0);
        }
    }
    void UpdateLabelsEnemy(int score1, int score2, TextMeshProUGUI labelA, TextMeshProUGUI labelB)
    {

        if (score1 >= score2)
        {
            labelA.rectTransform.anchoredPosition = new Vector3(-25, -75, 0);
            labelB.rectTransform.anchoredPosition = new Vector3(-25, -125, 0);
        }
        else
        {
            labelA.rectTransform.anchoredPosition = new Vector3(-25, -125, 0);
            labelB.rectTransform.anchoredPosition = new Vector3(-25, -75, 0);
        }
    }
}