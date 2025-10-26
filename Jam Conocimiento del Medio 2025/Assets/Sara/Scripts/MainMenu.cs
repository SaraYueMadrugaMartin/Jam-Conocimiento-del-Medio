using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject panelCredits;

    private void Start()
    {
        panelCredits.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("01_Level01");
    }

    public void Credits()
    {
        panelCredits.SetActive(true);
    }

    public void BackHome()
    {
        SceneManager.LoadScene("00_Main");
    }

    public void ClosePanelCredits()
    {
        panelCredits.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Has salido del juego.");
        Application.Quit();
    }
}
