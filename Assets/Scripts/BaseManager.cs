using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseManager : MonoBehaviour
{
    public string CurrentLevel;

    // сохраняет данные об игре
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

    // загружает данные об игре
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

    // загружает уровень с сохранением данных об уровне (использовать эту функцию, а не unity-вскую)
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
        if(name != "MainMenu")
            CurrentLevel = name;
    }

    // пока дебаг
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            LoadLevel("MainMenu");
    }
}

// класс хранящий сохраняемую инфу
[Serializable]
class SaveData
{
    public string CurrentLevel;
}
