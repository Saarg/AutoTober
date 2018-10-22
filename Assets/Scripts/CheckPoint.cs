using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VehicleBehaviour;

public class CheckPoint : MonoBehaviour {

	[Serializable]
	public class CheckedEvent : UnityEvent <WheelVehicle>{ }
	[SerializeField] CheckedEvent _checkedEvent;

	SpriteRenderer _sprite;
	[SerializeField] Color _defaultColor = new Color(1.0f, 1.0f, 1.0f, 0.3f);
	[SerializeField] Color _passedColor = new Color(0.0f, 1.0f, 1.0f, 0.3f);

	[SerializeField] bool _isStart;
	[SerializeField] bool _isFinish;

	Dictionary<WheelVehicle, bool> _checked = new Dictionary<WheelVehicle, bool>();
	public bool IsChecked(WheelVehicle v) { return _checked[v]; }

	void Start () 
	{
		_sprite = GetComponentInChildren<SpriteRenderer>();

		_sprite.color = _defaultColor;
	}
	
	void OnTriggerEnter(Collider other)
    {
		WheelVehicle v = other.GetComponentInParent<WheelVehicle>();
		if (v != null && other.gameObject.CompareTag("Player") && !_checked.ContainsKey(v))
		{
        	_sprite.color = _passedColor;

			_checked[v] = true;

			_checkedEvent.Invoke(v);

			if (_isStart)
				GameManager.instance.StartRace(v);
			
			if (_isFinish)
				GameManager.instance.FinishRace(v);
		}
    }
}
