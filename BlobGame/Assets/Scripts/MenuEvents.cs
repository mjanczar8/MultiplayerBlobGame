using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEvents : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject highScorePanel;
    public GameObject saveDataPanel;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            menuPanel.SetActive(true);
        }
    }
    public void MainMenu()
    {
        menuPanel.SetActive(true);
        highScorePanel.SetActive(false);
        saveDataPanel.SetActive(false);
    }
    public void SetName()
    {
        menuPanel.SetActive(false);
        highScorePanel.SetActive(true);
        saveDataPanel.SetActive(false);
    }
    public void LeaderBoard()
    {
        menuPanel.SetActive(false);
        highScorePanel.SetActive(false);
        saveDataPanel.SetActive(true);
    }
    public void escape()
    {
        menuPanel.SetActive(false);
        highScorePanel.SetActive(false);
        saveDataPanel.SetActive(false);
    }
}
