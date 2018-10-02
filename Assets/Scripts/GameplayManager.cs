using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{


    [SerializeField] private GameObject _ball;

    private static GameplayManager _instance = null;

    private GameObject _currentBall;
    private GameObject _spawnpoint;

    private int _strokes = 0;

    private GameObject _UIStroke;

    // Use this for initialization
    void Awake()
    {
        if (_instance == null) _instance = this;
    }

    void Start()
    {
        _UIStroke = GameObject.Find("StrokesText").gameObject;

#if DEBUG
Assert.IsNotNull(_UIStroke, "could not find ui");
#endif

        //spawn player
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        HandleControls();
    }

    private void HandleControls()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            //respawn player
            RespawnPlayer();
            ResetScore();
        }
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


        Vector3 ballPos = _currentBall.transform.position;

        Vector3 dc = new Vector3(14,9,0);

        Vector3 eulerBall = _currentBall.transform.rotation.eulerAngles;
        Vector3 dcEuler = new Vector3(-13, 30, 0); 



        //set camera position
        Camera.main.gameObject.transform.position = ballPos + dc; 
        Camera.main.gameObject.transform.rotation = Quaternion.Euler(new Vector3(27,-90,0));

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

    public void NotifyStroke()
    {
        ++_strokes;

        //update UI
        _UIStroke.GetComponent<Text>().text = _strokes.ToString();
    }

    public void NotifyReachedFinish()
    {
        //navigate to finish screen
        Debug.Log("FINISHED");
    }

    private void ResetScore()
    {
        _strokes = 0;
        _UIStroke.GetComponent<Text>().text = _strokes.ToString();
    }
}
