using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningScene : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        SceneManager.LoadScene("WinScene");
    }
}