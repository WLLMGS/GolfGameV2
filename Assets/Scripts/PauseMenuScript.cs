using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{



    public static bool IsPaused = false;

    [SerializeField] private GameObject _PauseMenu;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleControls();
    }


    void HandleControls()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                IsPaused = false;
                Time.timeScale = 1;
                //hide menu
                _PauseMenu.SetActive(false);
            }
            else
            {
                IsPaused = true;
                Time.timeScale = 0;
                //show menu
                _PauseMenu.SetActive(true);
            }
        }
    }

    public void OnResume()
    {
        IsPaused = false;
        Time.timeScale = 1;
        //hide menu
        _PauseMenu.SetActive(false);
    }

    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        IsPaused = false;
    }

    public void OnQuit()
    {
        Application.Quit();
        Time.timeScale = 1;
        IsPaused = false;
    }
}
