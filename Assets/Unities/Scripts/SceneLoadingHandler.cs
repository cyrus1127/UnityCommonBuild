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

    public bool isAppLaunchPreLoadScene = false;

    private bool shouldStartPreLoadScenes = false;

    private void Awake()
    {
        GameObject targetData = GameObject.Find("Data");
        if (targetData && !isAppLaunchPreLoadScene )
        {
            mainHandler = targetData.GetComponent<SceneHandler>();
            Debug.Log("targetData GameObject Found! App Already Launched ");
        }
        else {
            Debug.Log("targetData GameObject Not Found! is Launch App stage");
        }
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

            //The App is start
            if (targetSceneName == "Scene_login")
            {
                Debug.Log("App is starting -- waiting the FireBase Done");
                AppDataUtils.instance.SetAppLaunchCompletionBlock(CallForContinueLogin);
                StopCoroutine("PreLoadScenes");
            }
            else {
                Debug.Log("App is started --> target Scene Name ? " + targetSceneName);
                StartLoadScene();
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
                progressBar.rectTransform.localScale = new Vector3(prec, 1f, 1f);
            }
            else
            {//style 2
             //progressBar.gameObject.GetComponentInParent<Image>().

            }
        }

        if (shouldStartPreLoadScenes) {
            shouldStartPreLoadScenes = false;
            StartCoroutine("PreLoadScenes");
        }
    }


    /// --------- Scene Loading handling functions ---------- ///

    [System.Obsolete]
    private void StartLoadScene()
    {
        Debug.Log("SceneLoadingHandler::" + "StartLoadScene"  + " called" );
        if (async != null)
        {
            SceneManager.GetSceneByName(targetSceneName);
        }
        else
        {
            Debug.Log("async not ready to open");
            StartCoroutine("PreLoadScenes");
        }
    }

    [System.Obsolete]
    public void CallForContinueLogin()
    {
        //Start Load
        async = null;
        shouldStartPreLoadScenes = true;
        StartLoadScene();
    }

    [System.Obsolete]
    IEnumerator PreLoadScenes()
    {
        if (async == null)
        {
            Debug.Log("PreLoadScenes called ");
            async = SceneManager.LoadSceneAsync(targetSceneName);
            yield return null;
        }
        else
        {

            //while (async.isDone)
            //{
            //    print("async is load done");
            //    if (cameFromSceneName != null)
            //    {
            //        if (!SceneManager.UnloadScene(cameFromSceneName))
            //        {
            //            print("Scene unload failed");
            //        }
            //    }
            //    yield return null;
            //}

            //do in next frame
            while (!async.isDone)
            {
                print("async is not load done");
                yield return null;
            }
        }
    }


    IEnumerator testCorouter()
    {
        print("testCorouter");
        yield return null;
    }
}
