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


    //Ÿ�̸� 
    public int min = 1;
    float sec = 0f;

    bool timerOver = false;


    int playerScore = 0;
    int aiScore = 0;



    void Update()
    {
        // Ÿ�ӿ����� �ƴҶ��� �ð� ������Ʈ 
        if (timerOver == false)
        { timer(); }

    }


    // ���� �ð� ��� 
    void timer()
    {
        if (timerOver != true)
        {

            sec -= Time.deltaTime;

            // ���� 1���� ũ�� �ʰ� 0���� �۾��� ��� ���� �ʷ� ����
            if (min >= 1 && sec <= 0)
            {
                sec = 60f;
                min -= 1;
            }

        }
        // ���� �ð��� 0�� ��� 
        if (min <= 0 && sec <= 0)
        {
            timerOver = true;
            GameManager.Inst.SendMessage("TimeOver");
            GameOver.SetActive(true);

            Debug.Log("UI �ð� ������! ");
        }

        // ���� �ð��� ǥ�� 
        remainingTime.text = string.Format("{0} {1:D2}:{2:D2}", "Remaining Time", min, (int)sec);
    }


    // ���ھ� ������ ���� ǥ�� ���� 
    void UpdateScoreBoard()
    {

        // Playerscore�� UI ��ġ�� temp�� �����ص� 
        Vector3 temp = PlayerScore.transform.position;

        // AI ��ġ�� ���� 
        PlayerScore.transform.position = new Vector3(AIScore.transform.position.x, AIScore.transform.position.y, AIScore.transform.position.z);

        // AI ��ġ�� �����ص� temp ��ġ�� ���� 
        AIScore.transform.position = temp;

    }

    public void UpdatePlayerScore(int newscore)
    {
        // text ���
        Playerscore.text = string.Format("Player Score : {0}", newscore);
        playerScore++;

        // �÷��̾� ���ھ ���� playerscore�� ai score���� �Ʒ��� ���� �� ����
        if (playerScore > aiScore && PlayerScore.transform.position.y < AIScore.transform.position.y)
        { UpdateScoreBoard(); }

    }

    public void UpdateAIScore(int newscore)
    {
        // text ���
        AIscore.text = "AI Score : " + newscore;
        aiScore++;

        //AI ���ھ ����, AI score�� playerscore ���� �Ʒ��� ���� �� ���� 
        if (aiScore > playerScore && AIScore.transform.position.y < PlayerScore.transform.position.y)
        { UpdateScoreBoard(); }
    }
}
