using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [SerializeField] Menu[] menus;
    public Transform[] cameraAngles;
    public CameraFollow camFollow;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
	public void OpenMenu(string menuName)
	{
		for (int i = 0; i < menus.Length; i++)
		{
			if (menus[i].name == menuName)
			{
				menus[i].Open();
                if(i != 0) MoveCamera(cameraAngles[i]);
			}
			else if (menus[i].open)
			{
				CloseMenu(menus[i]);
			}
		}
	}

	public void OpenMenu(Menu menu)
	{
		for (int i = 0; i < menus.Length; i++)
		{
			if (menus[i].open)
			{
				CloseMenu(menus[i]);
			}

            if (menu == menus[i]) MoveCamera(cameraAngles[i]);
		}
		menu.Open();
	}

	public void CloseMenu(Menu menu)
	{
		menu.Close();
	}

    public void MoveCamera(Transform angle)
    {
        camFollow.target = angle;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
