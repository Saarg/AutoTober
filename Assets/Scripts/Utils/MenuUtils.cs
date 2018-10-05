﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUtils : MonoBehaviour {

	public void LoadScene (string name) 
	{
		SceneManager.LoadScene(name, LoadSceneMode.Single);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
