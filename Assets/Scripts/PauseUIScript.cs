using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIScript : MonoBehaviour
{
    // UI
    private GameObject _pauseUI;

    void Start()
    {
        _pauseUI = transform.GetChild(1).gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseUI.SetActive(!_pauseUI.activeSelf);
            Time.timeScale = _pauseUI.active ? 0 : 1;
        }
    }

    public void ExitMainMenu()
    {
        Time.timeScale = 1;
        BaseManager.Manager.LoadMainMenu();
    }
}
