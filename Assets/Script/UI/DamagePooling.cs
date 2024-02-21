using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePooling : MonoBehaviour
{ 
    public GameObject damagePrefab;
    public int poolSize = 5;

    private List<GameObject> damageObjects;

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        damageObjects = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = Instantiate(damagePrefab);
            obj.SetActive(false);
            damageObjects.Add(obj);
        }
    }

    public GameObject GetDamageObject(Vector3 position, int damage)
    {
        foreach (GameObject obj in damageObjects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.GetComponent<DamageDisplay>().damageText.text = damage.ToString();
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }
}
