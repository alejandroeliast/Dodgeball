using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    Canvas canvas;
    Controller controller;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void SetActive(Controller controller)
    {
        canvas.enabled = true;
        this.controller = controller;
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
        controller.SetEnabled();
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void CancelButton()
    {
        canvas.enabled = false;
        controller.SetEnabled();
    }

}
