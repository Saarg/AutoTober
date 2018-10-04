using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollController : MonoBehaviour {

	Animator _animator;
	Rigidbody[] _ragdollRigibodies = new Rigidbody[0];
	Collider[] _ragdollColliders = new Collider[0];

	Collider _collider;

	[SerializeField] bool _ragdollMode = false;
	[SerializeField] MonoBehaviour[] _scriptsToDisable;

	// Use this for initialization
	void Start ()
	{
		if (_animator == null)
		{
			_animator = GetComponentInChildren<Animator>();
		}

		if (_ragdollRigibodies.Length == 0)
		{
			_ragdollRigibodies = _animator.GetComponentsInChildren<Rigidbody>();
		}

		if (_ragdollColliders.Length == 0)
		{
			_ragdollColliders = _animator.GetComponentsInChildren<Collider>();
		}

		_animator.enabled = true;
		foreach(Rigidbody r in _ragdollRigibodies)
		{
			r.isKinematic = true;
			r.collisionDetectionMode = CollisionDetectionMode.Discrete;
		}
		
		foreach(Collider c in _ragdollColliders)
		{
			c.enabled = false;
		}

		if (_collider == null)
		{
			_collider = GetComponent<Collider>();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_collider.enabled == _ragdollMode)
		{
			_animator.enabled = !_ragdollMode;
			_collider.enabled = !_ragdollMode;

			foreach(Rigidbody r in _ragdollRigibodies)
			{
				r.isKinematic = !_ragdollMode;
				r.collisionDetectionMode = _ragdollMode ? CollisionDetectionMode.ContinuousDynamic : CollisionDetectionMode.Discrete;
			}
			
			foreach(Collider c in _ragdollColliders)
			{
				c.enabled = _ragdollMode;
			}

			foreach(MonoBehaviour b in _scriptsToDisable)
			{
				b.enabled = !_ragdollMode;
			}
		}
	}
}
