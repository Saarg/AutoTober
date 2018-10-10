using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VehicleBehaviour;

public class UIManager : MonoBehaviour {

	public static UIManager instance { get; internal set; }

	[SerializeField] WheelVehicle _playerCar;
 
	[SerializeField] int _score;
	[SerializeField] Text _textScore;

	[SerializeField] Image _boostBar;

	void Start ()
	{
		instance = this;
	}
	
	void Update ()
	{
		_textScore.text = "Score: " + _score;

		_boostBar.fillAmount = _playerCar.Boost / _playerCar.MaxBoost;
	}

	public void AddScore(int score)
	{
		_score += score;
	}
}
