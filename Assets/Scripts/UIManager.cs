using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VehicleBehaviour;

public class UIManager : MonoBehaviour {

	[SerializeField] WheelVehicle _playerCar;
 
	[SerializeField] int _score;
	[SerializeField] Text _textScore;
	[SerializeField] Text _finishText;
	[SerializeField] Text _recordText;

	[SerializeField] Image _boostBar;
	
	void Update ()
	{
		_textScore.text = "Score: " + _score;

		_boostBar.fillAmount = _playerCar.Boost / _playerCar.MaxBoost;
	}

	public void AddScore(int score)
	{
		_score += score;
	}

	public int GetScore()
	{
		return _score;
	}

	public void Finish(float time, int score, bool record)
	{
		_finishText.text = "Finished\n" + Mathf.FloorToInt(time/60f) + "m" + Mathf.FloorToInt(time%60f) + "s\nscore: " + score;

		_finishText.gameObject.SetActive(true);
		_recordText.gameObject.SetActive(record);
	}
}
