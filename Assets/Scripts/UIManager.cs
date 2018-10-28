using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using VehicleBehaviour;

using MOSC;

public class UIManager : MonoBehaviour {

	[SerializeField] WheelVehicle _playerCar;
 
	[SerializeField] int _score;
	[SerializeField] Text _textScore;
	[SerializeField] Text _finishText;
	[SerializeField] Text _recordText;

	[SerializeField] Image _boostBar;

	[SerializeField] StandaloneInputModule[] _eventSystems;

	[SerializeField] GameObject _pauseMenu;

	void Start()
	{
		foreach (StandaloneInputModule es in _eventSystems)
		{
			es.enabled = false;
		}

		_finishText.gameObject.SetActive(false);
		_recordText.gameObject.SetActive(false);

		_pauseMenu.SetActive(false);
	}
	
	void Update ()
	{
		_textScore.text = "Score: " + _score;

		_boostBar.fillAmount = _playerCar.Boost / _playerCar.MaxBoost;

		if (MultiOSControls.GetValue("Pause") != 0)
		{
			Pause();
		}
	}

	void OnApplicationFocus(bool hasFocus)
    {
        Pause();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        Pause();
    }

	public void Pause()
	{
		_pauseMenu.SetActive(true);

		Time.timeScale = 0f;
	}

	public void Resume()
	{
		_pauseMenu.SetActive(false);

		Time.timeScale = 1f;
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

		StartCoroutine(CEnableEventSystems());
	}

	IEnumerator CEnableEventSystems()
	{
		yield return new WaitForSeconds(1f);
		foreach (StandaloneInputModule es in _eventSystems)
		{
			es.enabled = true;
		}
	}
}
