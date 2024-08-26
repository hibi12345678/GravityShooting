using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Pun;
public class ScoreDisplay : MonoBehaviour
{
    public GameObject defeat;
    public GameObject victory;
    public GameObject draw;
    GameObject timer;
    // 自分自身のタグを取得
    string myTag;
    float timeRem;
    private CountdownTimer countdownTimer;
    private Leaderboard leaderboard;
    private ScoreProperty scoreProperty;
    int team1;
    int team2;
    public Text uiText1;
    public Text uiText2;
    public GameObject leaderBoardPanel;
    public GameObject escapeUI;
    public GameObject rotPoint;
    public GameObject mainScene;
    private MouseLock mouseLock;
    public TextMeshProUGUI myName = default;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = GameObject.Find("Timer"); 
        mainScene = GameObject.Find("MainScene");
        myTag = gameObject.tag;
        mouseLock = rotPoint.GetComponent<MouseLock>();
        PhotonView photonView;
        photonView = GetComponent<PhotonView>();
       

        if (photonView.CreatorActorNr == 1)
        {
            myName.text = "Player1";
        }
        if (photonView.CreatorActorNr == 2)
        {
            myName.text = "Player2";

        }
        if (photonView.CreatorActorNr == 3)
        {
            myName.text = "Player3";

        }
        if (photonView.CreatorActorNr == 4)
        {
            myName.text = "Player4";

        }
    }

    // Update is called once per frame
    void Update()
    {

        countdownTimer = timer.GetComponent<CountdownTimer>();
        leaderboard = leaderBoardPanel.GetComponent<Leaderboard>();
        timeRem = countdownTimer.currentTime;
        scoreProperty = mainScene.GetComponent<ScoreProperty>();
        

        if (gameObject.layer == 7)
        {
            team1 = scoreProperty.kill[0] + scoreProperty.kill[2];
            team2 = scoreProperty.kill[1] + scoreProperty.kill[3];
            uiText1.text = team1.ToString();
            uiText2.text = team2.ToString();
            if (timeRem == 0)
            {
                


                if (team1 > team2)
                {
                    victory.gameObject.SetActive(true);
                    Invoke("FinishUI", 3.0f);
                }
                else if(team1 < team2)
                {
                    defeat.gameObject.SetActive(true);
                    Invoke("FinishUI", 3.0f);
                }
                else
                {
                    draw.gameObject.SetActive(true);
                    Invoke("FinishUI", 3.0f);
                }
            }
        }
        else if(gameObject.layer == 8)
        {
            team2 = scoreProperty.kill[0] + scoreProperty.kill[2];
            team1 = scoreProperty.kill[1] + scoreProperty.kill[3];
            uiText1.text = team1.ToString();
            uiText2.text = team2.ToString();
            if (timeRem == 0)
            {
                
                if (team1 > team2)
                {
                    victory.gameObject.SetActive(true);
                    StartCoroutine(SlowTimeForThreeSeconds());
                    Invoke("FinishUI", 3.0f);
                }
                else if (team1 < team2)
                {
                    
                    defeat.gameObject.SetActive(true);
                    StartCoroutine(SlowTimeForThreeSeconds());
                    Invoke("FinishUI", 3.0f);
                }
                else
                {
                    draw.gameObject.SetActive(true);
                    StartCoroutine(SlowTimeForThreeSeconds());
                    Invoke("FinishUI", 3.0f);
                    
                }
            }
        }
        if (Input.GetKey(KeyCode.Tab))
        {
            leaderBoardPanel.SetActive(true);
        }
        else
        {
            leaderBoardPanel.SetActive(false);
        }
    }

    void FinishUI()
    {

        // カーソルロックを解除:
        mouseLock.enabled = false;
        escapeUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(escapeUI);  
        
    }

    IEnumerator SlowTimeForThreeSeconds()
    {
        // 時間を半分に遅くする
        Time.timeScale = 0.5f;

        // 3秒間待つが、実際には6秒かかる
        yield return new WaitForSeconds(2.0f);

        // 時間を元に戻す
        Time.timeScale = 1.0f;
    }
}
