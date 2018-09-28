using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[RequireComponent(typeof(Rigidbody))]
public class BallBehavior : MonoBehaviour
{


    private Rigidbody _rigid;

    private Vector3 _dir = new Vector3();

    // Use this for initialization
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();

#if DEBUG
        Assert.IsNotNull(_rigid, "No rigid body on player ball object");
#endif
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (Input.GetMouseButton(0))
        {
            float xm = Input.GetAxis("Mouse X");
            float ym = Input.GetAxis("Mouse Y");
            //Debug.Log("MOUSE MOVEMENT: [" + xm + "," + ym + "]");

            _dir += new Vector3(xm, 0, ym);
        }

        if (Input.GetMouseButtonUp(0))
        {
			float angle = Mathf.Atan2(_dir.x, _dir.z);
			float camAngle = Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

			// if cam = -90 9+ 90.0f	
			angle += camAngle + Mathf.PI / 2.0f;

			// angle -= Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad / 2.0f;

			Vector3 force = new Vector3(-Mathf.Cos(angle), 0, Mathf.Sin(angle));
			float magnitude = _dir.magnitude;

			_rigid.AddForce(force * magnitude, ForceMode.Impulse);
        }


    }
}
