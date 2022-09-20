using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderCallReceiver : MonoBehaviour
{
  public class TriggerEvent : UnityEvent<Collider> { }
  public TriggerEvent TriggerEnterEvent = new TriggerEvent();
  public TriggerEvent TriggerStayEvent = new TriggerEvent();
  public TriggerEvent TriggerExitEvent = new TriggerEvent();
  // Start is called before the first frame update
  void Start()
  {

  }

  /// <summary>
  /// トリガーエンターコールバック
  /// </summary>
  /// <param name="other">接触したコライダー</param>
  void OnTriggerEnter(Collider other)
  {
    TriggerEnterEvent?.Invoke(other);
  }

  /// <summary>
  /// トリガーステイコールバック
  ///</summary>
  /// <param name="other">接触したコライダー</param>
  void OnTriggerStay(Collider other)
  {
    TriggerStayEvent?.Invoke(other);
  }

  ///<summary>
  /// トリガーイグジットコールバック
  ///</summary>
  /// <param name="other">接触したコライダー</param>

  void OnTriggerExit(Collider other)
  {
    TriggerExitEvent?.Invoke(other);
  }


  // Update is called once per frame
  void Update()
  {

  }
}
