using System.Collections;
using UnityEngine;

public class MoveGate : MonoBehaviour
{
    [SerializeField] float duration = 2f;
 
    public void Move()
    {
        StartCoroutine(DoMove());
    }

    IEnumerator DoMove()
    {

        Vector3 start = transform.localPosition;
        Vector3 end = start + Vector3.up * 3;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(start, end, t / duration);
            yield return null;
        }
        transform.localPosition = end;
    }
}