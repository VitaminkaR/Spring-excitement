using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _baseManagerPrefab;

    private void Start()
    {
        // загружает единожды менеджер и производит первую загрузку из сохранения
        if (BaseManager.Manager == null)
        {
            GameObject _object = Instantiate(_baseManagerPrefab);
            DontDestroyOnLoad(_object);
            BaseManager.Init(_object);
            BaseManager.Manager = FindObjectOfType<BaseManager>();
            BaseManager.Manager.Load();
        }
    }

    public void Play()
    {
        BaseManager.Manager.PreLoadLevel();
    }

    public void Exit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
