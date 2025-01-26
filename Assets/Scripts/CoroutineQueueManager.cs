using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoroutineQueueManager : MonoBehaviour {
  private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
  private bool isRunning = false;

  // Enqueue a coroutine to be run sequentially
  public void EnqueueCoroutine(IEnumerator coroutine) {
    coroutineQueue.Enqueue(coroutine);
    if (!isRunning)
      StartCoroutine(RunQueue());
  }

  private IEnumerator RunQueue() {
    isRunning = true;
    while (coroutineQueue.Count > 0) {
      yield return StartCoroutine(coroutineQueue.Dequeue());
    }
    isRunning = false;
  }
}
