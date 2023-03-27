using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject selectObject = SelectRaycast().collider.gameObject;
            if (selectObject != null)
            {
                Debug.Log(selectObject.name);
            }
        }
    }

    RaycastHit SelectRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray.origin, ray.direction, out hit, 100);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
        return hit;
    }
}
