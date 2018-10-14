using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelSpin : MonoBehaviour {

	Image _img;

	void Start () {
		_img = GetComponent<Image>();

		if (_img == null)
			Destroy(this);
	}
	
	void Update () {
		float delta = Time.deltaTime / 2.0f;

		if (_img.fillClockwise)
		{
			if (_img.fillAmount + delta > 1) {
				_img.fillClockwise = false;

				_img.fillAmount = 1.0f;
			}
			else
				_img.fillAmount = (_img.fillAmount + delta) % 1;
		}
		else
		{
			if (_img.fillAmount - delta < 0) {
				_img.fillClockwise = true;

				_img.fillAmount = .0f;
			}
			else
				_img.fillAmount = _img.fillAmount - delta;
		}

		transform.Rotate(transform.forward, -180 * delta);
	}
}
