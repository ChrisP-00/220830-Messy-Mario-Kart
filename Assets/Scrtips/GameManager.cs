using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //--------------------------------------------------------------------
    //Sington
    #region Sington
    static GameManager inst = null;

    public static GameManager Inst    //외부에서 사용하는 변수 
    {
        get
        {
            if (inst == null)
            {
                inst = FindObjectOfType<GameManager>();

                if (inst == null)
                    inst = new GameObject("GameManager").AddComponent<GameManager>();

            }
            return inst;
        }
    }
    #endregion
    //--------------------------------------------------------------------

    int playerScore = 0;
    int aiScore = 0;

    bool isGameOver = false;


    GameObject ghost;


    public void AddPlayerScore(int newscore)
    {
        if (isGameOver == false)
        {
            playerScore += newscore;
            UIManager.Inst.UpdatePlayerScore(playerScore);
        }
    }

    public void AddAIScore(int newscore)
    {
        if (isGameOver == false)
        {
            aiScore += newscore;
            UIManager.Inst.UpdateAIScore(aiScore);
        }
    }



    public void TimeOver()
    {
        Debug.Log("## 게임 매니어 메세지 받았다~! ");

        ghost = GameObject.FindWithTag("AI");
        ghost.SendMessage("GameOver");
        isGameOver = true;
        return;
    }




}
