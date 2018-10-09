using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour {

	[SerializeField] float expectedTime;
	[SerializeField] CheckPoint[] _checkPoints;

	float _startTime;
	float _endTime;
	
	public void StartRace()
	{
		Debug.Log("Race start");
		_startTime = Time.realtimeSinceStartup;
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
		}
		else
		{
			Debug.Log("You missed a checkpoint!");
		}
	}
}
