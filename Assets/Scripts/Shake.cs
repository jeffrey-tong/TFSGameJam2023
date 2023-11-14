using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] Animator camAnim;

    public void CamShake()
    {
        camAnim.SetTrigger("shake");
    }

    public void DimensionShake()
    {
        camAnim.SetTrigger("dimension");
    }

}
