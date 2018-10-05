using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameObject _ball;

    [SerializeField] private List<GameObject> _spawnPoints = new List<GameObject>();

    private int _currentHoleIndex = 0;

    private static GameplayManager _instance = null;

    private GameObject _currentBall;
    private GameObject _spawnpoint;

    private int _strokes = 0;

    private GameObject _UIStroke;

    // Use this for initialization
    void Awake()
    {
        if (_instance == null) _instance = this;

        SpawnPlayer();
        SetCameraParams();
    }


    void Start()
    {
        _UIStroke = GameObject.Find("StrokesText").gameObject;

#if DEBUG
        Assert.IsNotNull(_UIStroke, "could not find ui");
#endif

        //spawn player



    }



    // Update is called once per frame
    void Update()
    {
        HandleControls();
    }

    private void HandleControls()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //respawn player
            RespawnPlayer();
            ResetScore();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            SetCameraParams();
        }

        else if(Input.GetKeyDown(KeyCode.N))
        {
            NotifyReachedFinish();
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

        _currentBall = Instantiate(_ball, _spawnPoints[_currentHoleIndex].transform.position, Quaternion.identity); //spawn player ball

        Camera.main.gameObject.GetComponent<CameraMovement>().SetObjectToFollow(_currentBall); //-> set object to follow
        Debug.Log("DONE");

    }
    private void SetCameraParams()
    {
        Vector3 ballPos = _currentBall.transform.position;

        Debug.Log(ballPos);

        Vector3 dc = new Vector3(19, 15, 0);

        //set camera position
        Camera.main.gameObject.transform.position = ballPos + dc;
        Camera.main.gameObject.transform.rotation = Quaternion.Euler(new Vector3(27, -90, 0));
    }
    public void NotifyPlayerDead()
    {
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        Destroy(_currentBall);

        _currentBall = Instantiate(_ball, _spawnPoints[_currentHoleIndex].transform.position, Quaternion.identity);

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

        if (_currentHoleIndex + 1 < _spawnPoints.Count)
        {
            ++_currentHoleIndex;
            RespawnPlayer();
        }
        else Debug.Log("No More Holes");
    }

    private void ResetScore()
    {
        _strokes = 0;
        _UIStroke.GetComponent<Text>().text = _strokes.ToString();
    }
}
