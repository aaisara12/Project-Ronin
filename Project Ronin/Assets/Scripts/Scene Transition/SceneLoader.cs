using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]

public class SceneLoader : MonoBehaviour
{

	// Current instance of SceneLoader, if one exists
	public static SceneLoader instance {get; private set;}

	// Whether this SceneLoader is currently loading a scene
	private bool loading = false;

	[Header("Settings")]
		[Tooltip("Name of the next scene to be loaded.")]
		[SerializeField] private string nextScene;

		[Tooltip("Minimum duration of the scene transition, in seconds.")]
		[SerializeField] private float duration = 1.0f;

	[Header("Events")]
		[Tooltip("Event called when the next scene starts loading.")]
		public UnityEvent<float> onSceneLoad = new UnityEvent<float>();

	// Update the current instance at the start of a scene
	void Awake() => instance = this;

	// Loads the next scene asynchronously, taking at least minDuration seconds
	private static IEnumerator LoadSceneAsync(string sceneName, float minDuration)
	{
		if (sceneName == null)
		{
			Debug.LogError("The name of the scene to load was not provided.");
			yield break;
		}
		else if (minDuration < 0)
		{
			Debug.LogWarning("Minimum duration of scene loading should be non-negative.");
		}

		// Wait for minDuration seconds
		float startTime = Time.time;
		while (Time.time - startTime < minDuration)
		{
			yield return null;
		}

		// Start loading the next scene asynchronously
		AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName);
		if (sceneLoad == null)
		{
			Debug.LogError("Scene loading operation failed.");
			yield break;
		}
	}

	// Wrapper function which invokes the onSceneLoad event and loads the sceneName scene
	public void LoadScene(string sceneName)
	{
		if (!loading)
		{
			loading = true;
			onSceneLoad.Invoke(duration);
			StartCoroutine(LoadSceneAsync(sceneName, duration));
		}
	}

	// Wrapper function which loads the nextScene
	public void LoadScene() => LoadScene(nextScene);
}
