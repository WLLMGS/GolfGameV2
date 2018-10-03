using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {



	private float _angle = 0.0f;
	[SerializeField] private float _offset = 0.0f;
	[SerializeField] private float _amplitude = 10.0f;

	[SerializeField] private float _frequency = 2.0f;


	[SerializeField] private bool _xDirection = false;
	[SerializeField] private bool _zDirection = false;
	

	private Vector3 _originalPos;
	void Awake()
	{
		_originalPos = transform.position;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		HandleMovement();
	}

	void HandleMovement()
	{
		_angle ++;

		Vector3 currentPosition = transform.position;
		
		if(_xDirection) currentPosition.x = _originalPos.x + _amplitude * Mathf.Sin( _frequency * ((_angle + _offset) * Mathf.Deg2Rad) );
		if(_zDirection) currentPosition.z = _originalPos.z + _amplitude * Mathf.Sin( _frequency * ((_angle + _offset) * Mathf.Deg2Rad) );
		transform.position = currentPosition;

	}
}
