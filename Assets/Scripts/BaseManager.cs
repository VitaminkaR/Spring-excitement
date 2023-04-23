using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
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

    // ������ ������ �����
    public Vector2 MapPartSize;
    // ���������� �� ���� �����, � �������� ������ ������������ �����, ��� ��������� ��������� �����
    public Vector2 MapPartLoadOffset;
    // ������ ��������� ������������� ������ ����
    public Dictionary<Vector2Int, bool> MapPartIsLoad;

    private void Start()
    {
        MapPartIsLoad = new Dictionary<Vector2Int, bool>();
        Application.quitting += ApplicationQuitting;
    }

    private void Update()
    {
        // ��������� ���������� ������
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

    // ��������� ������ �� ����
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

    // ��������� ������ �� ����
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

    // ��������� �� ����
    public void LoadMainMenu()
    {
        StopCoroutine(MapLoader());
        SceneManager.LoadScene("MainMenu");
        Player.gameObject.SetActive(false);
    }

    // ��������� ������ ����� �����
    public void PreLoadLevel()
    {
        Player.gameObject.SetActive(true);
        SceneManager.LoadScene("MapPart " + CurrentLevelCoords.x + " " + CurrentLevelCoords.y);
        MapPartIsLoad.Add(CurrentLevelCoords, true);
        StartCoroutine(AsyncLoadMapArea());
        StartCoroutine(MapLoader());
    }

    // ��������� � ��������� ����� ����� � ����������� �� ��������� ������
    IEnumerator MapLoader()
    {
        Vector2Int pastCoords = CurrentLevelCoords;
        while (true)
        {
            if (pastCoords != CurrentLevelCoords)
                yield return AsyncLoadMapArea();
            pastCoords = CurrentLevelCoords;

            yield return new WaitForSeconds(1);
        }
    }

    // ���������� ��������� ������� ���� ������ ������� map ���������
    // ���������� ��� ����������� �����
    // ���������� ��������� ��������� �� ������� �����
    IEnumerator AsyncLoadMapArea()
    {
        List<Scene> allScenes = SceneManager.GetAllScenes().ToList();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector2Int currentCoords = new Vector2Int(CurrentLevelCoords.x + i, CurrentLevelCoords.y + j);
                allScenes.Remove(allScenes.Find(scene => scene.name == $"MapPart {currentCoords.x} {currentCoords.y}"));
                yield return AsyncLoadMapPart(currentCoords);
            }
        }

        // ���������� ��� ����� ������� �������� ����� �������� (��� ����������, �� � ����� ��������� �� �����)
        for (int i = 0; i < allScenes.Count; i++)
        {
            string[] s = allScenes[i].name.Split();
            yield return AsyncUnLoadMapPart(new Vector2Int(int.Parse(s[1]), int.Parse(s[2])));
        }
    }
    // ���������� ��������� ����� �����
    IEnumerator AsyncLoadMapPart(Vector2Int coords)
    {   
        if (!ExistScene(coords))
            yield break;

        Debug.Log(coords);

        if (!MapPartIsLoad.ContainsKey(coords))
        {
            MapPartIsLoad.Add(coords, true);
        }
        else if (!MapPartIsLoad[coords])
        {
            MapPartIsLoad[coords] = true;
        }
        else
        {
            yield break;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MapPart " + coords.x + " " + coords.y, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    // ���������� ��������� ����� �����
    IEnumerator AsyncUnLoadMapPart(Vector2Int coords)
    {
        if (!ExistScene(coords))
            yield break;

        if (!MapPartIsLoad.ContainsKey(coords))
        {
            yield break;
        }
        else if (MapPartIsLoad[coords])
        {
            MapPartIsLoad[coords] = false;
        }

        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync("MapPart " + coords.x + " " + coords.y);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    // ��������� ���������� �� ����� ���������
    private bool ExistScene(Vector2Int coords) =>
        EditorBuildSettings.scenes.Any(scene => scene.path.Contains("Assets/Scenes/MapPart " + coords.x + " " + coords.y + ".unity"));


    private void ApplicationQuitting()
    {
        // ����������
        Save();
    }
}

// ����� �������� ����������� ����
[Serializable]
class SaveData
{
    public int CurrentLevelCoordsX;
    public int CurrentLevelCoordsY;
    public float PlayerPositionX;
    public float PlayerPositionY;
    public float PlayerPositionZ;
}
