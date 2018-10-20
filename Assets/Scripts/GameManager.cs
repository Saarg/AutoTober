using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-400)]
public class GameManager : MonoBehaviour {

	[SerializeField] int seed;

	void Start () 
	{
		Random.InitState(seed);
	}
	
	void Update () 
	{
		
	}
}
