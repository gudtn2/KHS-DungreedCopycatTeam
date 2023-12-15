using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

// YS: Scene관리및 이동  
public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance
    {
        get
        {
            return Instance;
        }
    }

    public static MySceneManager instance;

    [SerializeField]
    private float           fadeDuration;
    [SerializeField]
    private CanvasGroup     fadeImg;

    private void Start()
    {
        if(instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        { 
            ChagngeScene();

        }

    }
    public void ChagngeScene()
    {
        fadeImg.DOFade(1, fadeDuration)
            .OnStart(() => { fadeImg.blocksRaycasts = true; })
            .OnComplete(() => { });
    }
}
