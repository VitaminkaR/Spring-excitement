using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

enum UnitType
{
    Snowdrop,
    Cloud,
}

// �������� �� ����� � ����� ������ � �����������, � ����� ������ � ���� "������"
public class Spawner : MonoBehaviour
{
    // ������� Unit-��
    public List<GameObject> UnitObjects;
    // ���� Unit-��
    public List<int> UnitCost;

    // ��������� ��� �����
    [SerializeField] private UnitType _currentType;

    [SerializeField] private TextMeshProUGUI _essenceText;
    // �������� �� ������� ���������� �����
    private int _essence;
    public int Essence
    {
        get { return _essence; }
        set
        {
            _essenceText.text = "Essence: " + value.ToString();
            _essence = value;
        }
    }

    [SerializeField] private int _essenceGain;

    private void Start()
    {
        Essence = 0;
        StartCoroutine(EssenceGain());
    }

    // � ����������� �� ����, ������ ������������� ����� �� ������������ ����
    public GameObject CreateUnit(Field field)
    {
        int type = (int)_currentType;
        GameObject go = null;
        if (Essence >= UnitCost[type])
        {
            Essence -= UnitCost[type];
            go = Instantiate(UnitObjects[type]);
            go.transform.position = field.transform.position - new Vector3(0, -0.5f, 0);
        }
        return go;
    }

    public IEnumerator EssenceGain()
    {
        while (true)
        {
            Essence += _essenceGain;
            yield return new WaitForSeconds(1);
        }
    }
}
