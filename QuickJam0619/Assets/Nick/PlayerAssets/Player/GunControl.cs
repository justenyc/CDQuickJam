﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{

    private ShootingBase[] guns;

    private AmmoControl ammoScript;

    public float FireRateImprovementPercentage = 0;
    public float MaxFireRateImprovementPercentage = 0;

    int activeGun;


    // Start is called before the first frame update
    void Start()
    {
        guns = gameObject.GetComponents<ShootingBase>();
        ammoScript = gameObject.GetComponent<AmmoControl>();

        RerollGun();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RerollGun()
    {
        activeGun = Random.Range(0, guns.Length);

        for (int i = 0; i < guns.Length; i++)
        {
            Debug.Log(guns[i]);

            if (i == activeGun)
            {
                Debug.Log("Gun activated");
                guns[i].enabled = true;

                try
                {
                    AmmoUI.instance.ChangeAmmoType(i);
                }
                catch
                {
                    AmmoUI.instance.InitializeAmmoUI();
                    AmmoUI.instance.ChangeAmmoType(i);
                }

            }
            else
            {
                guns[i].enabled = false;
            }
        }

        Debug.Log("Switched guns!");
        Debug.Log(activeGun);

        ammoScript.SetAmmoRemaining(guns[activeGun].GetStartingAmmoCount());
        AmmoUI.instance.SetAmmo(guns[activeGun].GetStartingAmmoCount());

    }

    public void ImproveFireRate(float ImprovementPercentage)
    {
        FireRateImprovementPercentage = Mathf.Clamp(FireRateImprovementPercentage + ImprovementPercentage, 0, MaxFireRateImprovementPercentage);
    }
}
