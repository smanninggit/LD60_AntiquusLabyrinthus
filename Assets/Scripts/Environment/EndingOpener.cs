using System.Collections;
using UnityEngine;

public class EndingOpener : MonoBehaviour
{
    [SerializeField] float targetAngle = -80f;
    [SerializeField] Vector3 hingeAxis = Vector3.up;
    [SerializeField] float duration = 4f;

    public void Rotate()
    {
        StopAllCoroutines();
        StartCoroutine(DoRotate());
    }

    IEnumerator DoRotate()
    {
        Quaternion start = transform.localRotation;                            
        Quaternion end = Quaternion.AngleAxis(targetAngle, hingeAxis) * start;  

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            transform.localRotation = Quaternion.Slerp(start, end, t / duration);
            yield return null;
        }
        transform.localRotation = end;
    }
}