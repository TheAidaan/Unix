using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class UIManager_Base : MonoBehaviour
{
    public List<GameObject> _childPanels = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        foreach (CanvasGroup child in GetComponentsInChildren<CanvasGroup>())
        {

            _childPanels.Add(child.gameObject);

        }
        
        SetUI();
    }
    public virtual void SetUI() {}

    public void ReloadCurrentScene()          // can be used to restart game or play game from title menu
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()          // can be used to restart game or play game from title menu
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetSoundVolume(int volume)
    {
        UXManager.Static_SetSoundVolume(volume);
    }
    public void SetMusicVolume(int volume)
    {
        UXManager.Static_SetMusicVolume(volume);
    }
}
