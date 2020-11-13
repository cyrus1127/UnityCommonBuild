using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsHandler : MonoBehaviour
{
    string gameId_AOS = "3872649";
    string gameId_IOS = "3872648";
    
    bool testMode = true;

    public string banner_placement = "MainPageBanner";
    public float bannerRefreshTime = 0.5f;
    public BannerPosition onShowPostion;

    // Start is called before the first frame update
    void Start()
    {
        string target_id = gameId_AOS;
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
                       Application.platform == RuntimePlatform.OSXPlayer ||
                       Application.platform == RuntimePlatform.tvOS)
        {
            target_id = gameId_IOS;
        }
        
        Advertisement.Initialize(target_id, testMode);
        Advertisement.Banner.SetPosition(onShowPostion);
        Advertisement.Banner.Load();
        
        StartCoroutine(LoadBannerAd());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadBannerAd()
    {
        while (!Advertisement.Banner.isLoaded)
        {
            yield return new WaitForSeconds(bannerRefreshTime);
        }

        Advertisement.Banner.Show(banner_placement);
    }
}
