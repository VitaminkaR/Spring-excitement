using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _baseManagerPrefab;
    static public BaseManager Manager;

    private void Start()
    {
        // загружает единожды менеджер и производит первую загрузку из сохранения
        if (Manager == null)
        {
            DontDestroyOnLoad(Instantiate(_baseManagerPrefab));
            Manager = FindObjectOfType<BaseManager>();
            Manager.Load();
        }  
    }

    public void Play()
    {
        Manager.LoadLevel(Manager.CurrentLevel);
    }

    public void Exit()
    {
        // сохранение
        Manager.Save();
        Application.Quit();
    }
}
