using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void EnterShop()
    {
        SceneManager.LoadScene("ShopScene");
    }
    public void ExitShop()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void EnterTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void ExitTutorial()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
