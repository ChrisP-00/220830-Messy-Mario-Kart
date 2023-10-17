using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    //------------------------------------------------------------
    //Sington 
    #region Sington
    static UIManager inst;
    public static UIManager Inst
    {
        get
        {
            if (inst == null)
            {
                inst = FindObjectOfType<UIManager>();

                if (inst == null)
                    inst = new GameObject("UIManager").AddComponent<UIManager>();
            }
            return inst;
        }
    }

    #endregion
    //------------------------------------------------------------

    [Header("UI Info")]
    [SerializeField] Text Playerscore;
    [SerializeField] Text AIscore;
    [SerializeField] Text remainingTime;

    [SerializeField] GameObject GameOver;


    [SerializeField] GameObject PlayerScore;
    [SerializeField] GameObject AIScore;


    //타이머 
    public int min = 1;
    float sec = 0f;

    bool timerOver = false;


    int playerScore = 0;
    int aiScore = 0;



    void Update()
    {
        // 타임오버가 아닐때만 시간 업데이트 
        if (timerOver == false)
        { timer(); }

    }


    // 제한 시간 계산 
    void timer()
    {
        if (timerOver != true)
        {

            sec -= Time.deltaTime;

            // 분이 1보다 크고 초가 0보다 작아질 경우 분을 초로 변경
            if (min >= 1 && sec <= 0)
            {
                sec = 60f;
                min -= 1;
            }

        }
        // 남은 시간이 0일 경우 
        if (min <= 0 && sec <= 0)
        {
            timerOver = true;
            GameManager.Inst.SendMessage("TimeOver");
            GameOver.SetActive(true);

            Debug.Log("UI 시간 지났다! ");
        }

        // 남은 시간을 표시 
        remainingTime.text = string.Format("{0} {1:D2}:{2:D2}", "Remaining Time", min, (int)sec);
    }


    // 스코어 보드의 순위 표시 변경 
    void UpdateScoreBoard()
    {

        // Playerscore의 UI 위치를 temp에 저장해둠 
        Vector3 temp = PlayerScore.transform.position;

        // AI 위치로 변경 
        PlayerScore.transform.position = new Vector3(AIScore.transform.position.x, AIScore.transform.position.y, AIScore.transform.position.z);

        // AI 위치는 저장해둔 temp 위치로 변경 
        AIScore.transform.position = temp;

    }

    public void UpdatePlayerScore(int newscore)
    {
        // text 출력
        Playerscore.text = string.Format("Player Score : {0}", newscore);
        playerScore++;

        // 플레이어 스코어가 높고 playerscore가 ai score보다 아래에 있을 때 실행
        if (playerScore > aiScore && PlayerScore.transform.position.y < AIScore.transform.position.y)
        { UpdateScoreBoard(); }

    }

    public void UpdateAIScore(int newscore)
    {
        // text 출력
        AIscore.text = "AI Score : " + newscore;
        aiScore++;

        //AI 스코어가 높고, AI score가 playerscore 보다 아래에 있을 때 실행 
        if (aiScore > playerScore && AIScore.transform.position.y < PlayerScore.transform.position.y)
        { UpdateScoreBoard(); }
    }
}
