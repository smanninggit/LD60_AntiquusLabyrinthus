using UnityEngine;
using UnityEngine.SceneManagement;

public class PauiseMgmt : MonoBehaviour
{

    [SerializeField] GameObject settingsMenu;

    public void OpenSettings()  { settingsMenu.SetActive(true); }
    public void CloseSettings() { settingsMenu.SetActive(false); }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

        public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}