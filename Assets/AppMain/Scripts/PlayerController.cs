using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [SerializeField] GameObject attackHit = null;
  [SerializeField] float jumpPower = 20f;
  Animator animator = null;
  Rigidbody rigid = null;
  bool isAttack = false;
  // Start is called before the first frame update
  void Start()
  {
    animator = GetComponent<Animator>();
    rigid = GetComponent<Rigidbody>();
    attackHit.SetActive(false);
  }

  // Update is called once per frame
  void Update()
  {

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
    rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
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
