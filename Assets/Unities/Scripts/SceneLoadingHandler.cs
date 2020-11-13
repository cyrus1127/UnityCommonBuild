using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadingHandler : MonoBehaviour
{
    public string cameFromSceneName;
    public string targetSceneName;

    private SceneHandler mainHandler;

    public Image progressBar;
    private float progressBar_HolderSize;
    AsyncOperation async;


    private void Awake()
    {
        mainHandler = GameObject.Find("Data").GetComponent<SceneHandler>();
    }

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        Debug.Log("scence: SceneLoadingHandler ! Start!" );
        if (mainHandler != null)
        {
            targetSceneName = mainHandler.scene_name_to;
        }

        if (progressBar != null)
        {
            //style 1 

            //style 2
            //progressBar.gameObject.GetComponentInParent<Image>().
        }

        if (targetSceneName == null)
        {
            Debug.Log("No Target Scene");
        }
        else {
            if (async != null && async.isDone)
            {
                SceneManager.GetSceneByName(targetSceneName);
            }
            else
            {
                Debug.Log(async + " not ready to open");
                StartCoroutine(PreLoadScenes());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (async != null)
        {
            float prec = async.progress;

            if (true)
            {//style 1
                Debug.Log( "scence is loading !  -> progress : " + prec );
                progressBar.transform.localScale.Set(prec, 1, 1);
            }
            else
            {//style 2
             //progressBar.gameObject.GetComponentInParent<Image>().

            }
        }
    }

    [System.Obsolete]
    IEnumerator PreLoadScenes()
    {

        async = SceneManager.LoadSceneAsync(targetSceneName);

        while (!async.isDone)
        {
            yield return null;
        }

        if (async.isDone && cameFromSceneName != null) {
            if (!SceneManager.UnloadScene(cameFromSceneName))
            {
                Debug.LogWarning("Scene unload failed");
            }
        }
    }

}
