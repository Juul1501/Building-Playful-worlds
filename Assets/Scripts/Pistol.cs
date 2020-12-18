using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
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
    private PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }
    public void OnEnable()
    {
        isReloading = false;
    }

    protected override void Update()
    {
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
        switch (weaponState)
        {
            case ItemState.Equiped:
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
                if (hit.collider.tag != "player")
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "pistolimpact"), hit.point, Quaternion.LookRotation(hit.normal));

                if (playerController.photonView.IsMine)
                {
                    GameObject flash = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "pistolmuzzle"), flashHolder.position, flashHolder.rotation);
                    flash.transform.parent = flashHolder;
                }

                if (playerController.photonView.IsMine)
                    playerController.photonView.RPC("PlaySound", RpcTarget.All, "Pistol");

                ammo -= 1;
                GenerateRecoil();
                break;
        }
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
        player.Recoil(verticalRecoil, recoilDuration);
    }
}
