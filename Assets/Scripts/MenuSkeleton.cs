using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSkeleton : MonoBehaviour {

	Animator _animator;

	void Start ()
	{
		_animator = GetComponent<Animator>();

		StartCoroutine(CUpdate());
	}
	
	IEnumerator CUpdate()
	{
		while (true)
		{
			int rand = Random.Range(0, 100);

			if (rand < 10)
				_animator.SetTrigger("Scratch");
			else if (rand < 14)
				_animator.SetTrigger("Laugh");
			else if (rand > 96)
				_animator.SetTrigger("Roar");

			yield return new WaitForSeconds(1.0f);
		}
	}
}
