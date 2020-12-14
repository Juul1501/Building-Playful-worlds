﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    public CharacterController controller;

    public float interactRange = 2f;

    public float mouseSensitivity = 120;
    public GameObject camera;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    private Vector3 velocity;
    private float angleX;
    private float angleY;

    private bool isGrounded;

    private float time;
    private float duration;
    private float recoil;

    PhotonView pv;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    void Update()
    {
        if (!pv.IsMine)
            return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }
        Move();
        CameraLook();
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        
    }

    void CameraLook()
    {
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        angleX += mouseX * Time.deltaTime * mouseSensitivity;
        angleY += mouseY * Time.deltaTime * mouseSensitivity;

        transform.rotation = Quaternion.Euler(0, angleX, 0);
        angleY = Mathf.Clamp(angleY, -90f, 90f);
        camera.transform.localRotation = Quaternion.Euler(-angleY, 0, 0);

        if(time > 0)
        {
            angleY += recoil * Time.deltaTime / duration;
            time -= Time.deltaTime;
        }
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right* x + transform.forward * z;
        controller.Move(move* speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        if (Input.GetButtonDown("Jump")&& isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Interact()
    {
        Ray r = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        int ignorePlayer = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(r, out hit, interactRange, ignorePlayer))
        {
            IInteractable i = hit.collider.gameObject.GetComponent<IInteractable>();
            if (i != null)
            {
                i.Action();
            }
        }
    }

    public void Recoil(float recoildHeight,float recoilDuration)
    {
        recoil = recoildHeight;
        duration = recoilDuration;
        time = duration;
    }
    public IEnumerator destroy(GameObject g, float t)
    {
        yield return new WaitForSeconds(t);
        photonView.RPC("DestroyObject", RpcTarget.All,g);
    }

    [PunRPC]
    public void DestroyObject(object g)
    {
        var obj = g as GameObject;
        Destroy(obj);
    }
}
