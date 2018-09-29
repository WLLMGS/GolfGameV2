using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private GameObject _ball;
   [SerializeField] private float _rotationSpeed = 2.0f;

    private Vector3 _previousBallPosition;

    // Use this for initialization
    void Start()
    {
#if DEBUG
        Assert.IsNotNull(_ball, "camera does not have ball obj");
#endif

        _previousBallPosition = _ball.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
    }


    void LateUpdate()
    {
        HandleMovement();
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float xm = Input.GetAxis("Mouse X");
			float ym = Input.GetAxis("Mouse Y");

			transform.RotateAround(_ball.transform.position, new Vector3(0,1,0), xm * _rotationSpeed);
			transform.RotateAround(_ball.transform.position, transform.right, ym);
        }

    }

    void HandleMovement()
    {
        Vector3 displacement = _ball.transform.position - _previousBallPosition;
        _previousBallPosition = _ball.transform.position;

        transform.position += displacement;
    }
}
