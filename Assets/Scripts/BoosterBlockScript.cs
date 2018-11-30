using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterBlockScript : MonoBehaviour {


	[SerializeField] private float _multiplier = 100.0f;

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			Rigidbody rb = other.GetComponent<Rigidbody>();
			if(rb == null) return;

			//boost player when entered
			Vector3 currVelocity = rb.velocity;
			currVelocity.Normalize();
			currVelocity *= _multiplier;

			rb.velocity = currVelocity;
		}
	}
}
