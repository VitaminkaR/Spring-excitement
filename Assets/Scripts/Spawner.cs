using TMPro;
using UnityEngine;

enum Unit
{
    Snowdrop,
    Cloud,
}

// отвечает за выбор и спавн юнитов и противников, а также хранит в себе "валюту"
public class Spawner : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _essenceText;
    // эссенция за которую покупаются юниты
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

    // в зависимости от типа, создаёт определенного юнита на определенном поле
    GameObject CreateUnit(Field field, Unit type)
    {
        return null;
    }
}
