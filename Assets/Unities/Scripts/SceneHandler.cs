using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    const string scene_load = "Scene_loading";
    public string scene_name_to;
    public string scene_name_from;

    private AsyncOperation asyncOperation_IAPView;

    

    // Start is called before the first frame update
    void Start()
    {
        scene_name_from = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetToScene(string scene_name_to)
    {
        this.scene_name_to = scene_name_to;
        if (this.scene_name_to != null)
        {
            OpenScene();
        }
    }

    public void OpenScene()
    {
        if (asyncOperation_IAPView != null && asyncOperation_IAPView.isDone)
        {
            Debug.Log(scene_name_to + " ready to open");
            //SceneManager.GetSceneByName(scene_name_to);
            StartCoroutine(PreLoadScenes());
        }
        else
        {
            Debug.Log(scene_name_to + " not ready to open");
            StartCoroutine(PreLoadScenes());
        }
    }

    IEnumerator PreLoadScenes()
    {
        asyncOperation_IAPView = SceneManager.LoadSceneAsync(scene_load);

        while (!asyncOperation_IAPView.isDone)
        {
            yield return null;
        }

        if (asyncOperation_IAPView.isDone)
        {
            scene_name_from = SceneManager.GetActiveScene().name;
            Debug.Log("Scene load done");
        }
    }
}
