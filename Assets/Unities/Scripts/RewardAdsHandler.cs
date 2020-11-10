//
// EmptyInterface.cs
//
// Author:
//       cyruslam <sluggishchildcreategroup>
//
// Copyright (c) 2020 SluggishChildCreateGroup
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardAdsHandler : MonoBehaviour , IUnityAdsListener
{
    string gameId_AOS = "3872649";
    string gameId_IOS = "3872648";
    const string placementId_reward = "rewardedVideo";
    const string placementId_levelCom = "LevelComplete";

    bool testMode = true;
    
    public Button btn_Reward;
    public Text txt_coins;


    public void Awake()
    {

    }


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
        Advertisement.AddListener(this);


        if (btn_Reward != null)
        {
            btn_Reward.interactable = Advertisement.IsReady(placementId_reward);
            btn_Reward.onClick.AddListener(ShowRewardedVideo);
        }
    }

    public void ShowInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }

    public void ShowRewardedVideo()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(placementId_reward))
        {
            Advertisement.Show(placementId_reward);
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            if (placementId == placementId_reward) {
                int num = int.Parse(txt_coins.text);
                txt_coins.text = (num + 100).ToString();
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
        switch (placementId)
        {
            case placementId_reward:
                btn_Reward.interactable = true;
                break;
            default:
                //do nothing
                break;
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}
