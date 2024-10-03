using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPausePageView : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button returnToMainSceneButton;

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
