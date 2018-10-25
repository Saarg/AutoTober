using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VehicleBehaviour;

[DefaultExecutionOrder(-400)]
public class GameManager : MonoBehaviour {

	static public GameManager instance { get; internal set;}

	[SerializeField] ScorePopup scorePopupPrefab;

	[SerializeField] int _seed;

	[SerializeField] PlayerHolder[] _players;

	void Start () 
	{
		if (instance != null)
		{
			Debug.LogError("More than one gamemanager instance found!");
			Destroy(this);
		}

		instance = this;

		Random.InitState(_seed);

		StartCoroutine(LoadAsyncScene());
	}

	[Header("Scene")]
	[SerializeField] string _scene;
	[SerializeField] GameObject _loadingObject;

	IEnumerator LoadAsyncScene()
    {
		Debug.Log("Starting to load " + _scene);
		_loadingObject.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_scene, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

		
		Debug.Log("Loading done " + _scene);
		_loadingObject.SetActive(false);

		_checkPoints = FindObjectsOfType<CheckPoint>();
		StartCoroutine(Countdown());
    }

	[Header("CountDown")]
	[SerializeField] Text _countdownText;
	float _countdownLength = 3.0f;
	float _countdownStart;
	
	IEnumerator Countdown()
	{
		_countdownText.gameObject.SetActive(true);
		_countdownText.text = "" + Mathf.CeilToInt(_countdownLength);
		_countdownStart = Time.realtimeSinceStartup;

		foreach(PlayerHolder ph in _players)
		{
			// ph.Vehicle.Handbrake = true;
			ph.Vehicle.GetComponent<Rigidbody>().isKinematic = true;
		}

		while (Time.realtimeSinceStartup - _countdownStart < _countdownLength)
		{
			_countdownText.text = "" + Mathf.CeilToInt(_countdownLength - (Time.realtimeSinceStartup - _countdownStart));
			_countdownText.fontSize = 200 + (int)((Time.realtimeSinceStartup - _countdownStart)%1f * 100f);

			yield return null;
		}

		_countdownText.gameObject.SetActive(false);
		foreach(PlayerHolder ph in _players)
		{
			// ph.Vehicle.Handbrake = false;
			ph.Vehicle.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	[Header("RaceManager")]
	[SerializeField] int expectedTime;
	[SerializeField] CheckPoint[] _checkPoints;

	float _startTime;
	float _endTime;

	[SerializeField] Ghost _ghost;
	Dictionary<WheelVehicle, GhostRecorder> _recorders = new  Dictionary<WheelVehicle, GhostRecorder>();
	
	public void StartRace(WheelVehicle player)
	{
		if (player != null)
		{
			Debug.Log("Race start");
			_startTime = Time.realtimeSinceStartup;

			_recorders[player] = new GhostRecorder(expectedTime, 10, ref player);

			StartCoroutine(_recorders[player].RecordCoroutine());
			_ghost.run = true;
			_ghost.gameObject.SetActive(true);
		}
	}

	public void FinishRace(WheelVehicle player)
	{
		bool finished = true;
		foreach (CheckPoint cp in _checkPoints)
		{
			if (!cp.IsChecked(player))
				finished = false;
		}

		if (finished)
		{
			if (player != null)
			{
				PlayerHolder ph = player.GetComponentInParent<PlayerHolder>();

				_endTime = Time.realtimeSinceStartup - _startTime;
				Debug.Log("Finish time: " + _endTime);

				ph.Ui.AddScore(Mathf.FloorToInt(expectedTime - _endTime) * 1000);

				Transform spawnPos = player.gameObject.transform;
				ScorePopup sp = GameObject.Instantiate(scorePopupPrefab.gameObject, spawnPos.position, spawnPos.rotation).GetComponent<ScorePopup>();
				sp.SetScore(Mathf.FloorToInt(expectedTime - _endTime) * 1000);

				int score = ph.Ui.GetScore();

				_recorders[player].Stop();
				if (!_ghost.exist || score > _ghost.score)
				{
					_ghost.score = score;
					_recorders[player].Save(_scene + "-BestTime");

					ph.Ui.Finish(_endTime, score, true);
				}
				else
				{
					ph.Ui.Finish(_endTime, score, false);
				}
			}
		}
		else
		{
			Debug.Log("You missed a checkpoint!");
		}
	}
}
