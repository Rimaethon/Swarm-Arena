using System.Collections;
using Rimaethon.Scripts.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class SceneController : PersistentSingleton<SceneController>
	{
		private Canvas loadingCanvas;


		protected override void Awake()
		{
			base.Awake();
			loadingCanvas = GetComponentInChildren<Canvas>();
			LoadScene(1);
		}

		private IEnumerator  LoadSceneAsync(int sceneIndex = 1)
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
