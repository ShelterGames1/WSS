using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[HideInInspector]public enum Sight
{
    yes,
    no
}

public class InitialRecoil : MonoBehaviour
{ public Animator animator;
    public Camera cam;
    [HideInInspector] public GameObject sight;

    public int fieldOfViewSightOn;
    public int fieldOfViewSightOff;

    [HideInInspector]public Sight sightEnum;
    private void Update()
    {
        if(Input.GetMouseButton(1))
        {
            if (gameObject.GetComponent<GunScript>().isReloading == false)
            {
                animator.SetBool("IsPricel", true);
                if(sight.activeSelf)
                {
                    cam.fieldOfView = fieldOfViewSightOn;
                }
                else
                {
                    cam.fieldOfView = fieldOfViewSightOff;
                }
            }
        }
        else
        {
            animator.SetBool("IsPricel", false);
        }
    }
}
