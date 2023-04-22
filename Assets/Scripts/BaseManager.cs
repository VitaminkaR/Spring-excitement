using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseManager : MonoBehaviour
{
    static public BaseManager Manager;

    public Vector2Int CurrentLevelCoords;
    public Player Player;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _hpPrefab;
    [SerializeField] private GameObject _canvasPrefab;

    // размер частей карты
    public Vector2 MapPartSize;
    // расстояние от края карты, к которому должен приблизиться игрок, для прогрузки следующей части
    public Vector2 MapPartLoadOffset;

    private void Start()
    {
        Application.quitting += ApplicationQuitting;
    }

    private void Update()
    {
        // вычисляет координаты игрока
        if (Player != null)
        {
            CurrentLevelCoords = new Vector2Int((int)(Player.transform.position.x / MapPartSize.x), (int)(Player.transform.position.z / MapPartSize.y));
        }
    }

    static public void Init(GameObject manager)
    {
        Manager = manager.GetComponent<BaseManager>();
        Manager.Player = Instantiate(Manager._playerPrefab).GetComponent<Player>();
        DontDestroyOnLoad(Manager.Player.gameObject);
        Manager.Player.gameObject.SetActive(false);

        DontDestroyOnLoad(Instantiate(Manager._hpPrefab));
        DontDestroyOnLoad(Instantiate(Manager._canvasPrefab));
    }

    // сохраняет данные об игре
    public void Save()
    {
        using (FileStream fs = new FileStream("save.dat", FileMode.OpenOrCreate))
        {
            // data record
            SaveData sv = new SaveData();
            sv.CurrentLevelCoordsX = CurrentLevelCoords.x;
            sv.CurrentLevelCoordsY = CurrentLevelCoords.y;
            sv.PlayerPositionX = Player.transform.position.x;
            sv.PlayerPositionY = Player.transform.position.y;
            sv.PlayerPositionZ = Player.transform.position.z;

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
            CurrentLevelCoords.x = sv.CurrentLevelCoordsX;
            CurrentLevelCoords.y = sv.CurrentLevelCoordsY;
            Player.transform.position = new Vector3(sv.PlayerPositionX, sv.PlayerPositionY, sv.PlayerPositionY);
        }
    }

    // загружает гл меню
    public void LoadMainMenu()
    {
        StopCoroutine(MapLoader());
        SceneManager.LoadScene("MainMenu");
        Player.gameObject.SetActive(false);
    }

    // загружает первые куски карты
    public void PreLoadLevel()
    {
        Player.gameObject.SetActive(true);
        SceneManager.LoadScene("MapPart " + CurrentLevelCoords.x + " " + CurrentLevelCoords.y);
        StartCoroutine(AsyncLoadMapPart(new Vector2Int(CurrentLevelCoords.x + 1, CurrentLevelCoords.y)));
        StartCoroutine(AsyncLoadMapPart(new Vector2Int(CurrentLevelCoords.x - 1, CurrentLevelCoords.y)));
        StartCoroutine(AsyncLoadMapPart(new Vector2Int(CurrentLevelCoords.x, CurrentLevelCoords.y + 1)));
        StartCoroutine(AsyncLoadMapPart(new Vector2Int(CurrentLevelCoords.x, CurrentLevelCoords.y - 1)));

        StartCoroutine(MapLoader());
    }

    // загружает и выгружает куски карты в зависимости от положения игрока
    IEnumerator MapLoader()
    {
        while (true)
        {
            Debug.Log(MapPartSize.x * (CurrentLevelCoords.x + 1) - MapPartLoadOffset.x);
            if (Player.transform.position.x > MapPartSize.x * (CurrentLevelCoords.x + 1) - MapPartLoadOffset.x
            && !SceneManager.GetSceneByName("MapPart " + (CurrentLevelCoords.x + 1) + " " + CurrentLevelCoords.y).isLoaded)
            {
                yield return AsyncLoadMapPart(new Vector2Int(CurrentLevelCoords.x + 1, CurrentLevelCoords.y));
                yield return AsyncUnLoadMapPart(new Vector2(CurrentLevelCoords.x - 1, CurrentLevelCoords.y));
            }
            if (Player.transform.position.x > MapPartSize.x * CurrentLevelCoords.x - MapPartLoadOffset.x
            && !SceneManager.GetSceneByName("MapPart " + (CurrentLevelCoords.x - 1) + " " + CurrentLevelCoords.y).isLoaded)
            {
                yield return AsyncLoadMapPart(new Vector2Int(CurrentLevelCoords.x - 1, CurrentLevelCoords.y));
                yield return AsyncUnLoadMapPart(new Vector2(CurrentLevelCoords.x + 1, CurrentLevelCoords.y));
            }

            if (Player.transform.position.z > MapPartSize.y * (CurrentLevelCoords.y + 1) - MapPartLoadOffset.y
            && !SceneManager.GetSceneByName("MapPart " + CurrentLevelCoords.x + " " + (CurrentLevelCoords.y + 1)).isLoaded)
            {
                yield return AsyncLoadMapPart(new Vector2Int(CurrentLevelCoords.x, CurrentLevelCoords.y + 1));
                yield return AsyncUnLoadMapPart(new Vector2(CurrentLevelCoords.x, CurrentLevelCoords.y - 1));
            }
            if (Player.transform.position.z > MapPartSize.y * CurrentLevelCoords.y - MapPartLoadOffset.y
            && !SceneManager.GetSceneByName("MapPart " + CurrentLevelCoords.x + " " + CurrentLevelCoords.y).isLoaded)
            {
                yield return AsyncLoadMapPart(new Vector2Int(CurrentLevelCoords.x, CurrentLevelCoords.y - 1));
                yield return AsyncUnLoadMapPart(new Vector2(CurrentLevelCoords.x, CurrentLevelCoords.y + 1));
            }

            yield return new WaitForSeconds(1);
        }
    }

    // асинхронно загружает часть карты
    IEnumerator AsyncLoadMapPart(Vector2Int coords)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MapPart " + coords.x + " " + coords.y, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    // асинхронно выгружает часть карты
    IEnumerator AsyncUnLoadMapPart(Vector2 coords)
    {
        if (SceneManager.GetSceneByName("MapPart " + coords.x + " " + coords.y) != null)
        {
            AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync("MapPart " + CurrentLevelCoords.x + " " + CurrentLevelCoords.y);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }

    private void ApplicationQuitting()
    {
        // сохранение
        Save();
    }
}

// класс хранящий сохраняемую инфу
[Serializable]
class SaveData
{
    public int CurrentLevelCoordsX;
    public int CurrentLevelCoordsY;
    public float PlayerPositionX;
    public float PlayerPositionY;
    public float PlayerPositionZ;
}
