using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[RequireComponent(typeof(Rigidbody))]
public class BallBehavior : MonoBehaviour
{

    [SerializeField] private float _moveForce = 3.0f;
    [SerializeField] private float _maxMoveMagnitude = 15.0f;


    private Rigidbody _rigid;

    private bool _isReadyToMove = false;

    private Vector3 _dir = new Vector3();

    private GameObject _directionArrow;
    private GameObject _directionPlane;


    // Use this for initialization
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _directionArrow = transform.Find("directionArrow").gameObject;
        _directionPlane = _directionArrow.transform.Find("plane").gameObject;

#if DEBUG
        Assert.IsNotNull(_rigid, "No rigid body on player ball object");
        Assert.IsNotNull(_directionArrow, "No direction arrow on player ball object");
        Assert.IsNotNull(_directionPlane, "No direction plane on player ball object");
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenuScript.IsPaused)
        {
            IsReadyToMove();
            HandleMovement();
        }
    }
    void IsReadyToMove()
    {
        if (_rigid.velocity.magnitude <= 1.0f) _isReadyToMove = true;
    }
    void HandleMovement()
    {
        if (_isReadyToMove)
        {
            if (Input.GetMouseButton(0))
            {
                //set the direction arrow active
                if (!_directionArrow.activeSelf) _directionArrow.SetActive(true);

                //get mouse movement
                float xm = Input.GetAxis("Mouse X");
                float ym = Input.GetAxis("Mouse Y");
                //Debug.Log("MOUSE MOVEMENT: [" + xm + "," + ym + "]");

                //add the mouse movement to the direction
                _dir += new Vector3(xm, 0, ym);

                //clamp direction magnitude
                ClampDirection();

                //calculate the angle for the direction arrow
                float angle = Mathf.Atan2(_dir.x, _dir.z);
                float camAngle = Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
                angle += camAngle + Mathf.PI / 2.0f;

                //set the direction arrow rotation to cam rotatation + direction angle
                _directionArrow.transform.rotation = Quaternion.Euler(new Vector3(0, angle * Mathf.Rad2Deg, 0));

                //set the scale to the magnitude of the direction
                _directionPlane.transform.localScale = new Vector3(_dir.magnitude, 8.0f, 1.0f);

            }

            if (Input.GetMouseButtonUp(0))
            {
                //disable the direction arrow
                _directionArrow.SetActive(false);


                //calculate angle
                float angle = Mathf.Atan2(_dir.x, _dir.z);
                float camAngle = Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

                // if cam = -90 + 90.0f = 0	
                angle += camAngle + Mathf.PI / 2.0f;


                //calculate force in the angle direction
                Vector3 force = new Vector3(-Mathf.Cos(angle), 0, Mathf.Sin(angle));
                float magnitude = _dir.magnitude;

                //multiply the force by the magnitude of the direction
                force *= magnitude;

                //add the force to the rigid body
                _rigid.AddForce(force * _moveForce, ForceMode.Impulse);

                //set direction back to zero
                _dir = Vector3.zero;

                //notify game manager
                GameplayManager.GetInstance().NotifyStroke();

                //set ready to move to false
                _isReadyToMove = false;
            }

        }


    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            //notify level end
            GameplayManager.GetInstance().NotifyReachedFinish();
        }
        else if (other.gameObject.tag == "Killbox")
        {
            GameplayManager.GetInstance().NotifyPlayerDead();
        }
    }


    private void ClampDirection()
    {
        if (_dir.magnitude > _maxMoveMagnitude)
        {
            _dir = _dir.normalized * _maxMoveMagnitude;
        }
    }

}
