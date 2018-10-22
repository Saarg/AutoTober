using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuUtils : MonoBehaviour {

	bool _loading = false;
	[SerializeField] UnityEvent _startLoading;

	public void LoadScene (string name) 
	{
		StartCoroutine(LoadAsyncScene(name));
	}

	public void Quit()
	{
		Application.Quit();
	}

	IEnumerator LoadAsyncScene(string name)
    {
		if (!_loading)
		{
			Debug.Log("Starting to load " + name);
			_loading = true;

			_startLoading.Invoke();

			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

			// Wait until the asynchronous scene fully loads
			while (!asyncLoad.isDone)
			{
				yield return null;
			}

			
			Debug.Log("Loading done " + name);
			_loading = false;
		}
    }

	public void Fullscreen(bool enable)
	{
		Screen.fullScreen = enable;
	}

	public void VSync(bool enable)
	{
		QualitySettings.vSyncCount = enable ? 1 : 0;
	}

	public void Quality(int quality)
	{
		QualitySettings.SetQualityLevel(quality, true);
	}
}
