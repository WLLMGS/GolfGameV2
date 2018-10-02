using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayButtonPressed()
	{
		SceneManager.LoadScene(1);		
	}

	public void ResumeButtonPressed()
	{
		SceneManager.LoadScene(1, LoadSceneMode.Single);
	}

	public void QuitButtonPressed()
	{
		Application.Quit();
	}

	public void MainMenuButtonPressed()
	{
		SceneManager.LoadScene(0);
	}
}
