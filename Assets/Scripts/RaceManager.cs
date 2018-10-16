using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

public class RaceManager : MonoBehaviour {

	[SerializeField] int expectedTime;
	[SerializeField] CheckPoint[] _checkPoints;

	float _startTime;
	float _endTime;

	[SerializeField] WheelVehicle _vehicle;
	[SerializeField] Ghost _ghost;
	GhostRecorder _recorder = null;
	
	public void StartRace()
	{
		Debug.Log("Race start");
		_startTime = Time.realtimeSinceStartup;

		_recorder = new GhostRecorder(expectedTime, 10, ref _vehicle);
		StartCoroutine(_recorder.RecordCoroutine());
		_ghost.run = true;
		_ghost.gameObject.SetActive(true);
	}

	public void FinishRace()
	{
		bool finished = true;
		foreach (CheckPoint cp in _checkPoints)
		{
			if (!cp.IsChecked())
				finished = false;
		}

		if (finished)
		{
			_endTime = Time.realtimeSinceStartup - _startTime;
			Debug.Log("Finish time: " + _endTime);

			UIManager.instance.AddScore(Mathf.FloorToInt(expectedTime - _endTime) * 1000);

			_recorder.Stop();
			if (!_ghost.exist || _ghost.duration > _recorder.duration)
				_recorder.Save();
		}
		else
		{
			Debug.Log("You missed a checkpoint!");
		}
	}
}
