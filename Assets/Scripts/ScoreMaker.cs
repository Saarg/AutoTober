using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class ScoreMaker : MonoBehaviour {

	[SerializeField][HideInInspector] ScorePopup scorePopupPrefab;

	[SerializeField] AudioClip impactClip;
	AudioSource source;

	void Start() 
	{
		source = GetComponent<AudioSource>();

		if (source != null)
			source.clip = impactClip;
	}

	void OnCollisionEnter(Collision col) 
	{
		if (source != null) {
			source.Play();
			source.pitch = Random.Range(0.1f, 1.9f);
		}

		if (col.gameObject.CompareTag("Player")) {
			Transform spawnPos = transform;
			ScorePopup sp = GameObject.Instantiate(scorePopupPrefab.gameObject, spawnPos.position, spawnPos.rotation).GetComponent<ScorePopup>();
			sp.SetScore(col.relativeVelocity.sqrMagnitude);

			this.enabled = false;
		}
	}
}
