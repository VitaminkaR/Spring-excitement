using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseManager : MonoBehaviour
{
    public string CurrentLevel;

    // ��������� ������ �� ����
    public void Save()
    {
        using (FileStream fs = new FileStream("save.dat", FileMode.OpenOrCreate))
        {
            // data record
            SaveData sv = new SaveData();
            sv.CurrentLevel = CurrentLevel;

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, sv);
        }
    }

    // ��������� ������ �� ����
    public void Load()
    {
        using (FileStream fs = new FileStream("save.dat", FileMode.OpenOrCreate))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SaveData sv = (SaveData)formatter.Deserialize(fs);

            // data read
            CurrentLevel = sv.CurrentLevel;
        }
    }

    // ��������� ������� � ����������� ������ �� ������ (������������ ��� �������, � �� unity-�����)
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
        if(name != "MainMenu")
            CurrentLevel = name;
    }

    // ���� �����
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            LoadLevel("MainMenu");
    }
}

// ����� �������� ����������� ����
[Serializable]
class SaveData
{
    public string CurrentLevel;
}
