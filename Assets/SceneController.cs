using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static class selectedStage
    {
        public static string stage = "1";
    }


    

    public void SwitchTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void SwitchSelectScene()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void SwitchGameScene()
    {
        
        SceneManager.LoadScene("GameScene");
        selectedStage.stage = this.gameObject.name;
        Debug.Log(selectedStage.stage);
    }

    public void SwitchRresultScene()
    {
        SceneManager.LoadScene("ResultScene");
    }
}
