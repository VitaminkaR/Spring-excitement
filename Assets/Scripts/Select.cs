using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    [HideInInspector] public GameObject SelectField;

    [SerializeField] private Color _startColor;
    [SerializeField] private Color _selectColor;

    void Update()
    {
        GameObject selectObject = SelectRaycast().collider?.gameObject;
        if (selectObject != null && selectObject.GetComponent<Field>() != null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log(selectObject.name);
            }
            if(SelectField != null)
                SelectField.GetComponent<Renderer>().material.color = _startColor;
            SelectField = selectObject;
            SelectField.GetComponent<Renderer>().material.color = _selectColor;
        }
        else
        {
            if (SelectField != null)
                SelectField.GetComponent<Renderer>().material.color = _startColor;
            SelectField = null;
        }
    }

    RaycastHit SelectRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray.origin, ray.direction, out hit, 1000);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
        return hit;
    }
}
