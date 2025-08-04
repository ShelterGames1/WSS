using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using FishNet.Object;

public class PlayerWeapon : NetworkBehaviour
{
    [SerializeField] private List<APlayerWeapon> weapons = new List<APlayerWeapon>();

    [SerializeField] private APlayerWeapon currentWeapon;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
        {
            enabled = false;
            return;
        }
        InitializeWeapons(transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitializeWeapon(0);
        }
    }

    public void InitializeWeapons(Transform parentOfWeapons)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].transform.SetParent(parentOfWeapons);
        }

        InitializeWeapon(0);
    }
    private void InitializeWeapon(int weaponIndex)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }

        if (weapons.Count > weaponIndex)
        {
            currentWeapon = weapons[weaponIndex];
            currentWeapon.gameObject.SetActive(true);
            }
    }

    private void FireWeapon()
    {
        currentWeapon.Fire();
    }
}
