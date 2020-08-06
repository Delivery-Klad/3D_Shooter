using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineWeaponHolder : MonoBehaviour
{
    public int WeaponID = 0;
    public GameObject Weapon = null;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void AddWeaponToHolder(int index)
    {
        if(WeaponID != index)
        {
            WeaponID = index;
            Destroy(Weapon);
            Weapon = Instantiate(GameManager.instance.WeaponModels[index], gameObject.transform);
        }
        else
        {
            Weapon.SetActive(true);
        }
    }
}
