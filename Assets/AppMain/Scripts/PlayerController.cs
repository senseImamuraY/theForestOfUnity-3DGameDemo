using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [SerializeField] GameObject attackHit = null;
  [SerializeField] ColliderCallReceiver footColliderCall = null;
  [SerializeField] GameObject touchMarker = null;
  [SerializeField] float jumpPower = 20f;
  Animator animator = null;
  Rigidbody rigid = null;
  bool isAttack = false;
  bool isGround = false;
  bool isTouch = false;
  float horizontalKeyInput = 0;
  float verticalKeyInput = 0;
  Vector2 leftStartTouch = new Vector2();
  Vector2 leftTouchInput = new Vector2();
  [SerializeField] PlayerCameraController cameraController = null;
  // Start is called before the first frame update
  void Start()
  {
    animator = GetComponent<Animator>();
    rigid = GetComponent<Rigidbody>();
    attackHit.SetActive(false);

    // FootSphereのイベント登録
    footColliderCall.TriggerStayEvent.AddListener(OnFootTriggerStay);
    footColliderCall.TriggerExitEvent.AddListener(OnFootTriggerExit);
  }

  // Update is called once per frame
  void Update()
  {
    Debug.Log(Application.platform);
    Debug.Log(RuntimePlatform.Android);
    cameraController.UpdateCameraLook(this.transform);
    if (Application.platform != RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
    {
      Debug.Log("OSチェック");
      // スマホタッチ操作
      // タッチしている指の数が0より多い
      if (Input.touchCount > 0)
      {
        Debug.Log("タッチしている");
        isTouch = true;
        // タッチ情報をすべて取得
        Touch[] touches = Input.touches;
        foreach (var touch in touches)
        {
          bool isLeftTouch = false;
          bool isRightTouch = false;
          if (touch.position.x > 0 && touch.position.x < Screen.width / 2)
          {
            isLeftTouch = true;
          }
          else if (touch.position.x > Screen.width / 2 && touch.position.x < Screen.width)
          {
            isRightTouch = true;
          }
          if (isLeftTouch == true)
          {
            // タッチ開始
            if (touch.phase == TouchPhase.Began)
            {
              Debug.Log("タッチ開始");
              leftStartTouch = touch.position;
              touchMarker.SetActive(true);
              Vector3 touchPosition = touch.position;
              touchPosition.z = 1f;
              Vector3 markerPosition = Camera.main.ScreenToWorldPoint(touchPosition);
              touchMarker.transform.position = markerPosition;
            }
            // タッチ中
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
              Debug.Log("タッチ中");
              Vector2 position = touch.position;
              leftTouchInput = position - leftStartTouch;
            }
            // タッチ終了
            else if (touch.phase == TouchPhase.Ended)
            {
              Debug.Log("タッチ終了");
              leftTouchInput = Vector2.zero;
              touchMarker.gameObject.SetActive(false);
            }
          }
          if (isRightTouch == true)
          {
            cameraController.UpdateRightTouch(touch);
          }
        }
      }
      else
      {
        isTouch = false;
        // Debug.Log("何かしらが原因でAndroidプラットフォーム判定がされていない");
      }
    }
    else
    {
      Debug.Log("PC操作です");
      // PCキー入力取得
      horizontalKeyInput = Input.GetAxis("Horizontal");
      verticalKeyInput = Input.GetAxis("Vertical");
    }



    // Debug.Log(horizontalKeyInput + "." + verticalKeyInput);
    bool isKeyInput = (horizontalKeyInput != 0 || verticalKeyInput != 0 || leftTouchInput != Vector2.zero);
    if (isKeyInput == true && isAttack == false)
    {
      bool currentIsRun = animator.GetBool("isRun");
      if (currentIsRun == false) animator.SetBool("isRun", true);
      Vector3 dir = rigid.velocity.normalized;
      dir.y = 0;
      this.transform.forward = dir;
    }
    else
    {
      bool currentIsRun = animator.GetBool("isRun");
      if (currentIsRun == true) animator.SetBool("isRun", false);
    }
  }

  void FixedUpdate()
  {
    // カメラの位置をプレイヤーに合わせる
    cameraController.FixedUpdateCameraPosition(this.transform);
    if (isAttack == false)
    {
      Vector3 input = new Vector3();
      Vector3 move = new Vector3();

      if (Application.platform != RuntimePlatform.Android)
      {
        input = new Vector3(leftTouchInput.x, 0, leftTouchInput.y);
        move = input.normalized * 2f;
      }
      else
      {
        input = new Vector3(horizontalKeyInput, 0, verticalKeyInput);
        move = input.normalized * 2f;
      }
      // Vector3 input = new Vector3(horizontalKeyInput, 0, verticalKeyInput);
      // Vector3 move = input.normalized * 2f;
      Vector3 cameraMove = Camera.main.gameObject.transform.rotation * move;
      cameraMove.y = 0;
      Vector3 currentRigidVelocity = rigid.velocity;
      currentRigidVelocity.y = 0;
      rigid.AddForce(cameraMove - currentRigidVelocity, ForceMode.VelocityChange);
    }
  }

  ///<summary>
  ///攻撃ボタンクリックコールバック
  ///</summary>
  public void OnAttackButtonClicked()
  {
    if (isAttack == false)
    {
      animator.SetTrigger("isAttack");
      isAttack = true;
    }

  }

  // <summary>
  // ジャンプボタンクリックコールバック
  // </summary>
  public void OnJumpButtonClicked()
  {
    if (isGround == true)
    {
      rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

    }
  }

  /// <summary>
  /// FootSphereトリガーエンターコール
  /// </summary>
  /// <param name="col">侵入したコライダー</param>
  void OnFootTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "Ground")
    {
      isGround = true;
      // GetComponent<Animation>().SetBool("isGround", true);
      animator.SetBool("isGround", true);
    }
  }
  /// <summary>
  ///FootSphereトリガーステイコール
  /// </summary>
  /// <param name="col">侵入したコライダー</param>
  void OnFootTriggerStay(Collider col)
  {
    if (col.gameObject.tag == "Ground")
    {
      if (isGround == false) isGround = true;
      if (animator.GetBool("isGround") == false) animator.SetBool("isGround", true);
    }
  }

  /// <summary>
  /// FootSphereトリガーイグジットコール
  /// </summary>
  /// <param name="col">侵入したコライダー</param>
  void OnFootTriggerExit(Collider col)
  {
    if (col.gameObject.tag == "Ground")
    {
      isGround = false;
      animator.SetBool("isGround", false);
    }
  }

  /// < summary > 
  /// 攻撃 ボタンクリックコールバック. 
  /// </ summary >
  void Anim_AttackHit()
  {
    Debug.Log("Hit");
    attackHit.SetActive(true);
  }

  void Anim_AttackEnd()
  {
    Debug.Log("End");
    attackHit.SetActive(false);
    isAttack = false;
  }
}
