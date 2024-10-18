using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
	public class UIPausePageView : MonoBehaviour
	{
		[SerializeField]
		private Button resumeButton;
		[SerializeField]
		private Button returnToMainSceneButton;

		private void OnEnable()
		{
			resumeButton.onClick.AddListener(OnResumeButtonClicked);
			returnToMainSceneButton.onClick.AddListener(OnQuitButtonClicked);
		}

		private void OnDisable()
		{
			resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
			returnToMainSceneButton.onClick.RemoveListener(OnQuitButtonClicked);
		}

		private void OnQuitButtonClicked()
		{
			SceneController.Instance.LoadScene(1);
		}

		private void OnResumeButtonClicked()
		{
			Time.timeScale = 1;
			gameObject.SetActive(false);
		}
	}
}
