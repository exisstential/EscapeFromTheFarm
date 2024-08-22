using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  [SerializeField] private GameObject PlayerGameObj = null;
  private float xOffset = 7f;
  private void Start() 
  {
    PlayerGameObj = GameObject.FindGameObjectWithTag("Player");
  }

  private void LateUpdate() 
  {
    transform.position = new Vector3(PlayerGameObj.transform.position.x + xOffset, 0, -10);
  }
}
