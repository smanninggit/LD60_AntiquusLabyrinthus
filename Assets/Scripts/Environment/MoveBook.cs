using System.Collections;
using UnityEngine;

public class MoveBook : MonoBehaviour
{
    [SerializeField] float duration = 2f;
    [SerializeField] bool isRight;  
    public void Move()
    {
        StartCoroutine(DoMove());
    }

    IEnumerator DoMove()
    {
        float distance = isRight ? 2f : -2f;

        Vector3 start = transform.localPosition;
        Vector3 end = start + Vector3.forward * distance;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(start, end, t / duration);
            yield return null;
        }
        transform.localPosition = end;
    }
}