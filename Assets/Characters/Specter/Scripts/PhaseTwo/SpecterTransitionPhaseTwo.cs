using UnityEngine;
//using System.Collections.Generic;
using System.Collections;

public class SpecterTransitionPhaseTwo : MonoBehaviour, IBossAction
{
  public static SpecterTransitionPhaseTwo Instance;
  public bool CanTransition = false;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Debug.LogWarning("Found more than one instance of SpecterTransitionPhaseTwo");
    }
  }

  public void Enter() { }

  public void Update()
  {
    /*if (CanTransition)*/
    /*{*/
    /*  SpecterStateManager.Instance.ChangeAnimation("Phase2__Intro");*/
    /*}*/
  }

  public void DoTransition()
  {
    SpecterStateManager.Instance.ChangeAnimation("Phase2__Intro");
  }

  public void Exit() { }

  private bool hasDoneIt = false;
  public void TP2MoveToPointOne()
  {
    if (!hasDoneIt)
    {
      SpecterStateManager.Instance.Stop();
      transform.position = GameObject.Find("SpecterMovePoints/Phase__2_Transformation/1").transform.position;
      TP2SetSize();
      hasDoneIt = true;
    }
  }
  public void TP2MoveToPointTwo()
  {
    StartCoroutine(MoveTo(GameObject.Find("SpecterMovePoints/Phase__2_Transformation/2").transform.position, 0.5f));
  }
  public void TP2MoveToPointTree()
  {
    StartCoroutine(MoveTo(GameObject.Find("SpecterMovePoints/Phase__2_Transformation/3").transform.position, 0.25f));
  }

  public void TP2SetSize()
  {
    if (SpecterStateManager.Instance.isFacingRight)
    {
      transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);
      StartCoroutine(ScaleBack(3f));
    }
    else
    {
      transform.localScale = new Vector3(-0.18f, 0.18f, 0.18f);
      StartCoroutine(ScaleBack(3f));
    }
  }

  private IEnumerator ScaleBack(float duration)
  {
    Vector3 targetScale;
    if (SpecterStateManager.Instance.isFacingRight)
    {
      targetScale = new Vector3(0.24f, 0.24f, 0);
    }
    else
    {
      targetScale = new Vector3(-0.24f, 0.24f, 0);
    }
    Vector3 currentScale = transform.localScale;
    float elapsed = 0f;
    while (elapsed < duration)
    {
      float t = elapsed / duration;
      Vector3 newScale = Vector3.Lerp(currentScale, targetScale, t);
      elapsed += Time.deltaTime;
      transform.localScale = newScale;
      yield return null;
    }
    transform.localScale = targetScale;
  }

  private IEnumerator MoveTo(Vector3 targetPos, float duration)
  {
    Vector3 currentPos = transform.position;
    float elapsed = 0f;
    while (elapsed < duration)
    {
      float t = elapsed / duration;
      Vector3 newPos = Vector3.Lerp(currentPos, targetPos, t);
      elapsed += Time.deltaTime;
      transform.position = newPos;
      yield return null;
    }
    transform.position = targetPos;
  }
}


