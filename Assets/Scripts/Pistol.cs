using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public GameObject impactEffect;
    public GameObject muzzleFlash;
    public Transform flashHolder;

    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    public float verticalRecoil;
    public float recoilDuration;

    public Animator anim;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        if (!isEquiped)
            return;

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
        if (!Input.GetButton("Fire1")) anim.SetBool("Shooting", false);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }
    protected override void Shoot()
    {
        if (ammo <= 0)
            return;

        anim.SetBool("Shooting", true);
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

        GameObject flash = Instantiate(muzzleFlash, flashHolder);
        flash.transform.position = flashHolder.position;
        flash.transform.rotation = flashHolder.rotation;

        audioSource.PlayOneShot(shootSound);

        ammo -= 1;
        GenerateRecoil();
    }

    protected override void Reload()
    {
        if (!isReloading)
            StartCoroutine(ReloadWeapon());
    }
    IEnumerator ReloadWeapon()
    {
        audioSource.PlayOneShot(reloadSound);
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammo += magSize - ammo;
        isReloading = false;
    }
    void GenerateRecoil()
    {
        GameManager.instance.playerController.Recoil(verticalRecoil, recoilDuration);
    }
}
