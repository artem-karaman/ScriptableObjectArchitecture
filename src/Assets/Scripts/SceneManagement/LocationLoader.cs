using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// This class manages the scenes loading and unloading
/// </summary>
public class LocationLoader : MonoBehaviour
{
	[Header("Initialization Scene")]
	[SerializeField]
	private GameSceneSO _initializationScene;

	[Header("Load on Start")]
	[SerializeField]
	private GameSceneSO[] _mainMenuScenes;

	[Header("Loading Screen")]
	[SerializeField] private GameObject _loadingInterface;
	[SerializeField] private Image _loagingProgressBar;

	[Header("Load Event")]
	[SerializeField]
	private LoadEventChannelSO _loadEventChannel; //The load event we are listening to

	private IList<AsyncOperation> _scenesToLoadAsyncOperation = new List<AsyncOperation>();
	private IList<Scene> _scenesToUnload = new List<Scene>();
	private GameSceneSO _activeScene;

	private void OnEnable() => _loadEventChannel.OnLoadingRequested += LoadScenes;

	private void OnDisable() => _loadEventChannel.OnLoadingRequested -= LoadScenes;


    private void Start()
    {
		if(SceneManager.GetActiveScene().name == _initializationScene.sceneName)
		{
			LoadMainMenu();
		}
    }

    private void LoadMainMenu() => LoadScenes(_mainMenuScenes, false);

    private void LoadScenes(GameSceneSO[] locationsToLoad, bool showLoadingScreen)
    {
		AddScenesToUnload();

		_activeScene = locationsToLoad[0];

		for(int i = 0; i < locationsToLoad.Length; ++i)
		{
			var currentSceneName = locationsToLoad[i].sceneName;
			if (!CheckLoadState(currentSceneName))
			{
				_scenesToLoadAsyncOperation.Add(SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive));
			}
		}

		_scenesToLoadAsyncOperation[0].completed += SetActiveScene;

		if (showLoadingScreen)
		{
			_loadingInterface.SetActive(true);
			StartCoroutine(TrackLoadingProgress());
		}
		else
		{
			_scenesToLoadAsyncOperation.Clear();
		}

		UnloadScenes();
    }

    private void UnloadScenes()
    {
        if(_scenesToUnload != null)
		{
			for(int i = 0; i < _scenesToUnload.Count; ++i)
			{
				SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
			}
		}

		_scenesToUnload.Clear();
    }

	private IEnumerator TrackLoadingProgress()
    {
        float totalProgress = 0;

        while (totalProgress <= 0.9f)
        {
            totalProgress = 0;
            for (int i = 0; i < _scenesToLoadAsyncOperation.Count; ++i)
            {
                Debug.Log($"Scene {i} : {_scenesToLoadAsyncOperation[i].isDone} " +
                    $"progress = {_scenesToLoadAsyncOperation[i].progress}");
                totalProgress += _scenesToLoadAsyncOperation[i].progress;
            }

            _loagingProgressBar.fillAmount = totalProgress / _scenesToLoadAsyncOperation.Count;
            Debug.Log($"progress bar {_loagingProgressBar.fillAmount} and value = " +
                $"{totalProgress / _scenesToLoadAsyncOperation.Count}");

            yield return null;
        }

        ClearScenesToLoad();

        HideLoadingInterface();
    }

    private void ClearScenesToLoad() => _scenesToLoadAsyncOperation.Clear();

    private void HideLoadingInterface() => _loadingInterface.SetActive(false);

    private void SetActiveScene(AsyncOperation asyncOperation) =>
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(_activeScene.sceneName));

    private bool CheckLoadState(string sceneName)
    {
        for(int i = 0; i < SceneManager.sceneCount; ++i)
		{
			var scene = SceneManager.GetSceneAt(i);
			if(scene.name == sceneName)
			{
				return true;
			}
        }
		return false;
    }

    private void AddScenesToUnload()
    {
        for(int i = 0; i < SceneManager.sceneCount; ++i)
		{
			var scene = SceneManager.GetSceneAt(i);
			if(scene.name != _initializationScene.sceneName)
			{
				Debug.Log($"Added scene to unload = {scene.name}");
				_scenesToUnload.Add(scene);
			}
		}
    }

	private void ExitGame()
	{
		Application.Quit();
		Debug.Log("Exit!!!");
	}
}

