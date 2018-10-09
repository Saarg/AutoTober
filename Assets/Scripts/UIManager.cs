using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance { get; internal set; }

	[SerializeField] int _score;
	[SerializeField] Text _textScore;

	void Start ()
	{
		instance = this;
	}
	
	void Update ()
	{
		_textScore.text = "Score: " + _score;
	}

	public void AddScore(int score)
	{
		_score += score;
	}
}
