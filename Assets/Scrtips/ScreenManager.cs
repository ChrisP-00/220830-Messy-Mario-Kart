using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{


    public void OnClick_NextScene()
    {
        SceneManager.LoadScene("MainScene");
        //SceneManager.UnloadScene("StartScene");
    }


}
