using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
	public class UILevelEndView : MonoBehaviour
	{
		[SerializeField]
		private Button returnToMainSceneButton;

		private void OnEnable()
		{
			returnToMainSceneButton.onClick.AddListener(OnQuitButtonClicked);
		}

		private void OnDisable()
		{
			returnToMainSceneButton.onClick.RemoveListener(OnQuitButtonClicked);
		}

		private void OnQuitButtonClicked()
		{
			SceneController.Instance.LoadScene(1);
		}
	}
}
