using TMPro;
using UnityEngine;

enum UnitType
{
    Snowdrop,
    Cloud,
}

// �������� �� ����� � ����� ������ � �����������, � ����� ������ � ���� "������"
public class Spawner : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _essenceText;
    // �������� �� ������� ���������� �����
    private int _essence;
    public int Essence 
    { 
        get { return _essence; } 
        set { 
            _essenceText.text = "Essence: " + value.ToString(); 
            _essence = value; 
        } 
    }

    private void Start()
    {
        Essence = 0;
    }

    // � ����������� �� ����, ������ ������������� ����� �� ������������ ����
    GameObject CreateUnit(Field field, UnitType type)
    {
        return null;
    }
}
