using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class AsaultRifle : Weapon
{
    public GameObject impactEffect;
    public ParticleSystem muzzleFlash;

    protected override void Shoot()
    {
        if (ammo <= 0)
            return;

        isReloading = false;
        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range);
        if (hit.transform.GetComponent<IDamageable<RaycastHit>>() != null)
        {
            IDamageable<RaycastHit> hitObj = hit.transform.GetComponent<IDamageable<RaycastHit>>();
            hitObj.Damage(damage, hit);

        }
        GameObject obj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(obj, 2f);
        muzzleFlash.Play();
        ammo -= 1;
    }

    protected override void Reload()
    {
        if(!isReloading)
        StartCoroutine(ReloadWeapon());
    }
    IEnumerator ReloadWeapon()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammo += magSize - ammo;
        isReloading = false;
    }
}
