using Managers;
using UnityEngine;
using UnityEngine.UI;

public class UILevelEndView : MonoBehaviour
{
    [SerializeField] Button returnToMainSceneButton;

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
