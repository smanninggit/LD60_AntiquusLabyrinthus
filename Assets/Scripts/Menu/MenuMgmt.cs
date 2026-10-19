using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMgmt : MonoBehaviour
{

    [SerializeField] GameObject settingsMenu;
    

    public void OpenSettings()  { settingsMenu.SetActive(true); }
    public void CloseSettings() { settingsMenu.SetActive(false); }

    public void LoadMainScene()
    {
        AudioManager.Instance.FadeOutAndDestroy();
        SceneManager.LoadScene("MainDungeon");
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