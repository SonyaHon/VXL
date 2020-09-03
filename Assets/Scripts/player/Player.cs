using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

  public float rotationSpeed = 10.0f;
  public float moveSpeed = 1.0f;
  void Start()
  {
    VXL.instance.SetPlayer(this);
  }

  void Update()
  {
    if (Input.GetAxis("Horizontal") != 0)
    {
      Debug.Log("Hor change");
      transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, 0, Space.World);
    }

    if (Input.GetAxis("Vertical") != 0)
    {
      transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.Self);
    }
  }
}
