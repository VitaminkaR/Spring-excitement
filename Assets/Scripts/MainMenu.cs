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
        // ��������� �������� �������� � ���������� ������ �������� �� ����������
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
        // ����������
        Manager.Save();
        Application.Quit();
    }
}
