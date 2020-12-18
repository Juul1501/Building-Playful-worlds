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
    public void OnEnable()
    {
        isReloading = false;
    }
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponentInParent<PlayerController>();
    }
    protected override void Shoot()
    {
        switch (weaponState)
        {
            case ItemState.Equiped:
                if (ammo <= 0)
                    return;

                isReloading = false;
                anim.SetBool("Shooting", true);
                RaycastHit hit;
                Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range);
                if (hit.transform.GetComponent<IDamageable<RaycastHit>>() != null)
                {
                    IDamageable<RaycastHit> hitobj = hit.transform.GetComponent<IDamageable<RaycastHit>>();
                    hitobj.Damage(damage, hit);
                }
                if (hit.collider.tag != "player")
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "impacteffect"), hit.point, Quaternion.LookRotation(hit.normal));

                if (playerController.photonView.IsMine)
                {
                    GameObject flash = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "rifleflare"), flashHolder.position, flashHolder.rotation);
                    flash.transform.parent = flashHolder;
                }

                if (playerController.photonView.IsMine)
                    playerController.photonView.RPC("PlaySound", RpcTarget.All, "Rifle");

                ammo -= 1;
                GenerateRecoil();
                break;
        }
    }

    protected override void Reload()
    {
        if (!isReloading)
            isReloading = true;
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

    protected override void EquipWeapon(GameObject sender)
    {
        base.EquipWeapon(sender);
        playerController = GetComponentInParent<PlayerController>();
    }
}
