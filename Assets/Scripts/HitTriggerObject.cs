using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTriggerObject : MonoBehaviour {

	[SerializeField] Vector3 offset = new Vector3(0f, 0.5f, 0f);
	Rigidbody _parentRb;

	void Start () 
	{
		_parentRb = GetComponentInParent<Rigidbody>();
	}
	
	void Update () 
	{
		Vector3 pos = offset;
		if (_parentRb)
		{
			Vector3 speed = transform.InverseTransformDirection(_parentRb.velocity);
			speed.Normalize();

			pos += speed * 1.3f;
		}
		transform.localPosition = pos;
	}
}
