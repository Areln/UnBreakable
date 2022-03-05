using UnityEngine;
using System.Collections;
using System;

public class CameraFollow : MonoBehaviour
{
  public Transform target;
  public float distanceCameraFromGround = 40;
  public float camOffset = 1.5f;
  public float camRotationSpeed = 1;
  public bool InvertCamRotation;

  float camHeight = 100f;
  float camX;
  float camZ;
  float targetXLast;
  float targetZLast;
  float targetDistanceFromTarget;

  void Start()
  {
    if(InvertCamRotation)
    {
      camRotationSpeed = -camRotationSpeed;
    }
    camX = target.transform.position.x;
    camZ = target.transform.position.z - camHeight / camOffset;
    targetXLast = target.position.x;
    targetZLast = target.position.z;
  }

  void LateUpdate()
  {
    if (target)
    {
      camHeight = GetCamHeight();
      targetDistanceFromTarget = camHeight / camOffset;
      camX += target.position.x - targetXLast;
      camZ += target.position.z - targetZLast;
      transform.position = new Vector3(camX, camHeight, camZ);

      var targetsPositionFromCamera = new Vector3(target.position.x, camHeight, target.position.z);
      if (Vector3.Distance(transform.position, targetsPositionFromCamera) > targetDistanceFromTarget)
      {
        transform.position = Vector3.MoveTowards(transform.position, targetsPositionFromCamera, 10 * Time.deltaTime);
        camX = transform.position.x;
        camZ = transform.position.z;
      }
      //else if (Vector3.Distance(transform.position, targetsPositionFromCamera) < targetDistanceFromTarget)
      //{
      //  transform.position = Vector3.MoveTowards(transform.position, -transform.forward, camOffset * Time.deltaTime);
      //  camX = transform.position.x;
      //  camZ = transform.position.z;
      //}

      if (Input.GetKey(KeyCode.LeftControl))
      {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {//rotate camera
          transform.RotateAround(target.position, Vector3.up, -camRotationSpeed);
          camX = transform.position.x;
          camZ = transform.position.z;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {//rotate camera
          transform.RotateAround(target.position, Vector3.up, camRotationSpeed);
          camX = transform.position.x;
          camZ = transform.position.z;
        }
      }
      targetXLast = target.position.x;
      targetZLast = target.position.z;
    }

  }

  private float GetCamHeight()
  {
    var camH = camHeight;
    Ray r = new Ray(transform.position, Vector3.down);
    RaycastHit hit;
    if (Physics.Raycast(r, out hit, 1000, LayerMask.GetMask("Ground")))
    {
      camH = hit.point.y + distanceCameraFromGround;
    }


    return camH;
  }
}
