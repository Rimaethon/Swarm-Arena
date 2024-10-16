using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Managers
{
	public class SceneController : PersistentSingleton<SceneController>
	{
		private Canvas loadingCanvas;

		protected override void Awake()
		{
			base.Awake();
			loadingCanvas = GetComponentInChildren<Canvas>();

			if (SceneManager.GetActiveScene().buildIndex == 0)
			{
				LoadScene(1);
			}
		}

		private IEnumerator LoadSceneAsync(int sceneIndex = 1)
		{
			loadingCanvas.enabled = true;
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

			while (!asyncLoad.isDone)
			{
				yield return null;
			}

			loadingCanvas.enabled = false;
		}

		public void LoadScene(int sceneIndex)
		{
			StartCoroutine(LoadSceneAsync(sceneIndex));
		}
	}
}
