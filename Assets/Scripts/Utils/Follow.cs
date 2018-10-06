using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
	public class Follow : MonoBehaviour {
		[SerializeField] bool _follow = false;
		[SerializeField] Transform _target;
		[SerializeField] Vector3 _offset;
		[Range(0, 10)]
		[SerializeField] float _lerpPositionMultiplier = 1f;
		[Range(0, 10)]		
		[SerializeField] float _lerpRotationMultiplier = 1f;

		Vector3 _startPos;
		Quaternion _startRot;

		Rigidbody _rb;

		void Start () 
		{
			_startPos = transform.position;
			_startRot = transform.rotation;

			_rb = GetComponent<Rigidbody>();
		}

		void FixedUpdate() 
		{
			if (!_follow) return;

			this._rb.velocity.Normalize();

			Quaternion curRot = transform.rotation;

			Rigidbody _rb = _target.GetComponent<Rigidbody>();
			if (_rb == null)
				transform.LookAt(_target);
			else {
				transform.LookAt(_target.position + _target.forward * _rb.velocity.sqrMagnitude);				
			}
			
			Vector3 tPos = _target.position + _target.TransformDirection(_offset);
			if (tPos.y < _target.position.y) {
				tPos.y = _target.position.y;
			}

			transform.position = Vector3.Lerp(transform.position, tPos, Time.fixedDeltaTime * _lerpPositionMultiplier);
			transform.rotation = Quaternion.Lerp(curRot, transform.rotation, Time.fixedDeltaTime * _lerpRotationMultiplier);

			if (transform.position.y < 0.5f) {
				transform.position = new Vector3(transform.position.x , 0.5f, transform.position.z);
			}
		}
	}
}
