using System.Collections;
using UnityEngine;

public class MoveSpike : MonoBehaviour
{
    [SerializeField] float distance = 0.5f;
    [SerializeField] float upTime = 0.2f;
    [SerializeField] float holdTime = 0f;   
    [SerializeField] float downTime = 1f;

    Vector3 startPos;
    Coroutine running;

    void Awake() => startPos = transform.localPosition;

    public void Move()
    {
        if (running != null) StopCoroutine(running);
        transform.localPosition = startPos;   
        running = StartCoroutine(DoMove());
    }

    IEnumerator DoMove()
    {
        Vector3 top = startPos + Vector3.up * distance;

        for (float t = 0f; t < upTime; t += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(startPos, top, t / upTime);
            yield return null;
        }
        transform.localPosition = top;

        if (holdTime > 0f) yield return new WaitForSeconds(holdTime);

        for (float t = 0f; t < downTime; t += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(top, startPos, t / downTime);
            yield return null;
        }
        transform.localPosition = startPos;
        running = null;
    }
}