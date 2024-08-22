using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomScript : MonoBehaviour
{
    [SerializeField] private Animation MushroomAnim = null;

    public void MushroomCollided()
    {
        MushroomAnim.Play();
    }
}
