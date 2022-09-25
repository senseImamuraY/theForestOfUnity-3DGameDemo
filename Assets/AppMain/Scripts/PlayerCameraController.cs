using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
  [SerializeField] Transform rotationRoot = null;
  [SerializeField] Transform heightRoot = null;
  [SerializeField] Transform mainCamera = null;
  [SerializeField] float lookHeight = 1.0f;
  [SerializeField] float rotationSpeed = 0.01f;
  [SerializeField] float heightSpeed = 0.001f;
  [SerializeField] Vector2 heightLimit_MinMax = new Vector2(-1f, 3f);
  Vector2 cameraStartTouch = Vector2.zero;
  Vector2 cameraTouchInput = Vector2.zero;
  // Start is called before the first frame update
  public void UpdateCameraLook(Transform player)
  {
    var cameraMarker = player.position;
    cameraMarker.y += lookHeight;
    var camLook = (cameraMarker - mainCamera.transform.position).normalized;
    mainCamera.transform.forward = camLook;
  }
  public void FixedUpdateCameraPosition(Transform player)
  {
    this.transform.position = player.position;
  }

  public void UpdateRightTouch(Touch touch)
  {
    if (touch.phase == TouchPhase.Began)
    {
      Debug.Log("右タッチ開始");
      cameraStartTouch = touch.position;
    }
    else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
    {
      Debug.Log("右タッチ中");
      Vector2 position = touch.position;
      cameraTouchInput = position - cameraStartTouch;
      var yRot = new Vector3(0, cameraTouchInput.x * rotationSpeed, 0);
      var rResult = rotationRoot.rotation.eulerAngles + yRot;
      var qua = Quaternion.Euler(rResult);
      rotationRoot.rotation = qua;

      var yHeight = new Vector3(0, -cameraTouchInput.y * heightSpeed, 0);
      var hResult = heightRoot.transform.localPosition + yHeight;
      if (hResult.y > heightLimit_MinMax.y) hResult.y = heightLimit_MinMax.y;
      else if (hResult.y <= heightLimit_MinMax.x) hResult.y = heightLimit_MinMax.x;
      heightRoot.localPosition = hResult;
    }
    else if (touch.phase == TouchPhase.Ended)
    {
      Debug.Log("右タッチ終了");
      cameraTouchInput = Vector2.zero;
    }
  }

  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
