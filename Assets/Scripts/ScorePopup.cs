using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopup : MonoBehaviour {

	static new Camera camera;
	[SerializeField] TextMesh scoreText;

	float startTime;
	float lifetime = 1;

	[SerializeField] AnimationCurve xCurve;
	float xmult = 1;
	[SerializeField] AnimationCurve yCurve;
	float ymult = 1;	

	Vector3 startPos;

	[SerializeField] Color posColor = Color.green;
	[SerializeField] Color negColor = Color.red;

	void Start () {
		if (camera == null)
			camera = FindObjectOfType<Camera>();

		startTime = Time.realtimeSinceStartup;

		startPos = transform.position;

		xmult = Random.Range(-1.5f, 1.5f);
		ymult = Random.Range(0.5f, 1.5f);
	}

	public void SetScore(float s) {
		int score = Mathf.FloorToInt(s);

		if (score < 100)
		{
			Destroy(gameObject);
			return;
		}

		scoreText.text = (score > 0 ? "+" : "") + score.ToString();
		scoreText.color = score > 0 ? posColor : negColor;

		transform.localScale = Vector3.one * Mathf.Clamp(0.5f + (Mathf.Abs(s) / 5000f), 0.5f, 1.5f);
	}
	
	void Update () {
		float life = Time.realtimeSinceStartup - startTime;

		Vector3 right = transform.right;
		right.y = 0;
		right.Normalize();
		transform.position = startPos + right * (xCurve.Evaluate(life / lifetime) * xmult) + Vector3.up * (yCurve.Evaluate(life / lifetime) * ymult);

		transform.LookAt(camera.transform.position);

		if (life > lifetime)
			Destroy(gameObject);
	}
}
