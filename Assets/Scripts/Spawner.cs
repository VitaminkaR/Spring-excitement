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

// отвечает за выбор и спавн юнитов и противников, а также хранит в себе "валюту"
public class Spawner : MonoBehaviour
{
    // объекты Unit-ов
    public List<GameObject> UnitObjects;
    // цена Unit-ов
    public List<int> UnitCost;

    // выбранный тип юнита
    [SerializeField] private UnitType _currentType;

    [SerializeField] private TextMeshProUGUI _essenceText;
    // эссенция за которую покупаются юниты
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

    // в зависимости от типа, создаёт определенного юнита на определенном поле
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
