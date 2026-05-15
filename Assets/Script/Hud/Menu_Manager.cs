using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu_Manager : MonoBehaviour
{

    public TMP_Text panelName; 
    public void LoadScene(string sceneName)
    {
       SceneManager.LoadScene(sceneName); 
    }

    public void ExitApp ()
    {
        Application.Quit();
    }

    public void setPanelName(string name)
    {
        panelName.text = name;
    }
}
