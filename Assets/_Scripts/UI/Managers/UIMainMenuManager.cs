using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenuManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private List<GameObject> pages = new List<GameObject>();

    private void OnEnable()
    {
        startButton.onClick.AddListener(StartGame);
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => HandlePageButtonClicked(button));
        }
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(StartGame);
        foreach (Button button in buttons)
        {
            button.onClick.RemoveListener(() => HandlePageButtonClicked(button));
        }
    }

    private void StartGame()
    {
        SceneController.Instance.LoadScene(2);
    }

    private void HandlePageButtonClicked(Button button)
    {
        CloseAllPages();
        pages[buttons.IndexOf(button)].SetActive(true);
    }

    private void CloseAllPages()
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
    }
}
