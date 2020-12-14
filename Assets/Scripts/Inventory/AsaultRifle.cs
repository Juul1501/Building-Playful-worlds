using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class AsaultRifle : Weapon
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
    private PlayerController playerController;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    protected override void Shoot()
    {
        if (ammo <= 0)
            return;

        isReloading = false;
        anim.SetBool("Shooting", true);
        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range);
        if (hit.transform.GetComponent<IDamageable<RaycastHit>>() != null)
        {
            IDamageable<RaycastHit> hitObj = hit.transform.GetComponent<IDamageable<RaycastHit>>();
            hitObj.Damage(damage, hit);
        }
        //GameObject obj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        GameObject obj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "impacteffect"), hit.point, Quaternion.LookRotation(hit.normal));
        //StartCoroutine(player.destroy(obj,2f));

        //GameObject flash = Instantiate(muzzleFlash, flashHolder);
        GameObject flash = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "rifleflare"), flashHolder.position, flashHolder.rotation);
        flash.transform.parent = flashHolder;
        //StartCoroutine(player.destroy(flash, 2f));
        audioSource.PlayOneShot(shootSound);
        ammo -= 1;
        GenerateRecoil();


    }

    protected override void Reload()
    {
        if(!isReloading)
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

    protected override void Update()
    {
        if (!isEquiped)
            return;

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
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

    void GenerateRecoil()
    {

        player.Recoil(verticalRecoil,recoilDuration);
    }


}
