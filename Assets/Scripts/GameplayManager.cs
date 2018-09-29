using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameplayManager : MonoBehaviour
{


    [SerializeField] private GameObject _ball;

    private static GameplayManager _instance = null;

    private GameObject _currentBall;
	private GameObject _spawnpoint;

    // Use this for initialization
    void Awake()
    {
        if (_instance == null) _instance = this;
    }

    void Start()
    {
        //spawn player
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static GameplayManager GetInstance()
    {
        return _instance;
    }

    private void SpawnPlayer()
    {
        //find spawnpoint
        _spawnpoint = GameObject.Find("SpawnPoint");

#if DEBUG
        Assert.IsNotNull(_spawnpoint, "DEPENDENCY ERROR (in GameplayManager): spawnpoint in world not found");
#endif

        _currentBall = Instantiate(_ball, _spawnpoint.transform.position, Quaternion.identity); //spawn player ball
		
		Camera.main.gameObject.GetComponent<CameraMovement>().SetObjectToFollow(_currentBall); //-> set object to follow

    }

	public void NotifyPlayerDead()
	{
		RespawnPlayer();
	}

	private void RespawnPlayer()
	{
		Destroy(_currentBall);

		_currentBall = Instantiate(_ball, _spawnpoint.transform.position, Quaternion.identity);

		Camera.main.gameObject.GetComponent<CameraMovement>().SetObjectToFollow(_currentBall); //-> set object to follow

	}
}
