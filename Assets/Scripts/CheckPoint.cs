using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour {

	[SerializeField] UnityEvent _checkedEvent;

	SpriteRenderer _sprite;
	[SerializeField] Color _defaultColor = new Color(1.0f, 1.0f, 1.0f, 0.3f);
	[SerializeField] Color _passedColor = new Color(0.0f, 1.0f, 1.0f, 0.3f);

	bool _checked = false;
	public bool IsChecked() { return _checked; }

	void Start () 
	{
		_sprite = GetComponentInChildren<SpriteRenderer>();

		_sprite.color = _defaultColor;
	}
	
	void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.CompareTag("Player") && !_checked)
		{
        	_sprite.color = _passedColor;

			_checked = true;

			_checkedEvent.Invoke();
		}
    }
}
