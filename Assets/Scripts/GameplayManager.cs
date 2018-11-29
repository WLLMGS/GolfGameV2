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

    [SerializeField] private GameObject _tableParent;
    [SerializeField] private GameObject _tableLabelPrefab;
    [SerializeField] private GameObject _tableTextPrefab;


    private List<Text> _tableElements = new List<Text>();



    // Use this for initialization
    void Awake()
    {
        if (_instance == null) _instance = this;

        //SpawnPlayer();
        RespawnPlayer();
        SetCameraParams();
    }


    void Start()
    {
        _UIStroke = GameObject.Find("StrokesText").gameObject;

        InitScoreboard();
    }

    public static GameplayManager GetInstance()
    {
        return _instance;
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

        if (Input.GetKeyDown(KeyCode.Tab)) ToggleTable();

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
        if (_currentBall != null) Destroy(_currentBall);

        _currentBall = Instantiate(_ball, _spawnPoints[_currentHoleIndex].transform.position, Quaternion.identity);

        Camera.main.gameObject.GetComponent<CameraMovement>().SetObjectToFollow(_currentBall); //-> set object to follow
    }

    public void NotifyStroke()
    {
        ++_strokes;


        //update UI
        UpdateScoreUI();
    }

    public void NotifyReachedFinish()
    {

        //set score table text to amount of stroke for the hole
        //reset the score -> strokes to zero & changed in UI
        UpdateScoreUI();
        ResetScore();

        //increment current hole index
        //respawn player at the next hole
        //if there is a next hole
        if (_currentHoleIndex + 1 < _spawnPoints.Count)
        {
            ++_currentHoleIndex;
            RespawnPlayer();
        }
    }

    private void ResetScore()
    {
        _strokes = 0;
        _UIStroke.GetComponent<Text>().text = _strokes.ToString();

    }


    private void UpdateScoreUI()
    {
        _tableElements[_currentHoleIndex].text = _strokes.ToString();
        _UIStroke.GetComponent<Text>().text = _strokes.ToString();
    }
    private void InitScoreboard()
    {
        /*
            this piece of code sets up the table in the canvas
            it uses a parent thats already in the canvas
            it consists of a table label and a tabel text
            table label shows which hole it is (hole 1, hole 2)
            table text show how many strokes the player has on this hole
            the labels get added in a for each and use a y counter to determine the height
         */


        int ycounter = 0;

        foreach (GameObject spawnpoint in _spawnPoints)
        {
            GameObject lbl = Instantiate(_tableLabelPrefab, _tableParent.transform);
            RectTransform rt = lbl.GetComponent<RectTransform>();
            Text txtComp = lbl.GetComponent<Text>();

            rt.position += new Vector3(0, -ycounter * rt.sizeDelta.y, 0);

            txtComp.text = "Hole " + (ycounter + 1).ToString() + " : ";

            GameObject txt = Instantiate(_tableTextPrefab, _tableParent.transform);
            rt = txt.GetComponent<RectTransform>();
            txtComp = txt.GetComponent<Text>();

            rt.position += new Vector3(rt.sizeDelta.x, -ycounter * rt.sizeDelta.y, 0);
            txtComp.text = "0";

            ++ycounter;

            _tableElements.Add(txtComp);
        }

        _tableParent.SetActive(false);
    }

    private void ToggleTable()
    {
        bool isactive = !_tableParent.activeSelf;
        _tableParent.SetActive(isactive);
    }
}
