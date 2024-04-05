using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;

    public void OnClickNewGame()
    {
        canvas.SetActive(true);
        SceneManager.LoadScene("Scene(Yuseop)");
    }

    public void OnclickOption()
    {

    }

    public void OnClickExit()
    {
        canvas.SetActive(false);
        SceneManager.LoadScene("StartScene");
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
