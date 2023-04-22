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

    // ������ ������ �����
    public Vector2 MapPartSize;
    // ���������� �� ���� �����, � �������� ������ ������������ �����, ��� ��������� ��������� �����
    public Vector2 MapPartLoadOffset;

    private void Start()
    {
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
        StartCoroutine(MapLoader());
    }

    // ��������� � ��������� ����� ����� � ����������� �� ��������� ������
    IEnumerator MapLoader()
    {
        while (true)
        {
            if (Player.transform.position.x > MapPartSize.x * (CurrentLevelCoords.x + 1) - MapPartLoadOffset.x
            && !SceneManager.GetSceneByName("MapPart " + (CurrentLevelCoords.x + 1) + " " + CurrentLevelCoords.y).isLoaded)
            {
                Debug.Log("LOAD" + new Vector2(CurrentLevelCoords.x + 1, CurrentLevelCoords.y).ToString());
                yield return AsyncLoadMapPart(new Vector2(CurrentLevelCoords.x + 1, CurrentLevelCoords.y));
                //yield return AsyncUnLoadMapPart(new Vector2(CurrentLevelCoords.x - 1, CurrentLevelCoords.y));
            }
            if (Player.transform.position.x < MapPartSize.x * CurrentLevelCoords.x + MapPartLoadOffset.x
                && SceneManager.GetSceneByName("MapPart " + (CurrentLevelCoords.x - 1) + " " + CurrentLevelCoords.y) == null)
            {
                Debug.Log("LOAD" + new Vector2(CurrentLevelCoords.x - 1, CurrentLevelCoords.y).ToString());
                yield return AsyncLoadMapPart(new Vector2(CurrentLevelCoords.x - 1, CurrentLevelCoords.y));
                //yield return AsyncUnLoadMapPart(new Vector2(CurrentLevelCoords.x + 1, CurrentLevelCoords.y));
            }

            yield return new WaitForSeconds(1);
        }
    }

    // ���������� ��������� ����� �����
    IEnumerator AsyncLoadMapPart(Vector2 coords)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MapPart " + CurrentLevelCoords.x + " " + CurrentLevelCoords.y);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    // ���������� ��������� ����� �����
    IEnumerator AsyncUnLoadMapPart(Vector2 coords)
    {
        if (SceneManager.GetSceneByName("MapPart " + CurrentLevelCoords.x + " " + CurrentLevelCoords.y) != null)
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
