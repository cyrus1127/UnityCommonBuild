//
// AppDataUtils.cs
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

using System;
using System.Collections;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;


public class AppDataUtils : MonoBehaviour
{

    public delegate void FirebaseReadyDelegate();
    public FirebaseReadyDelegate methodHolder;

    ///Login
    private string account_Id;

    /// Login - Firebase
    private static Firebase.Auth.FirebaseAuth auth;
    private static Firebase.FirebaseApp app;
    private Firebase.Auth.FirebaseUser user;
    private bool isUserExist = false;
    private bool isCheckedUserOnce = false;
    /// Login - Google config
    public string googleIdToken;
    public string googleAccessToken;


    private int coins;
    private int cashCoins;
    public string lastLogoutTime;
    static string key_coins = "key_coins";
    static string key_cashCoins = "key_cash";
    static string key_logoutTime = "key_logoutTime";


    private static AppDataUtils _instance = null;
    public static AppDataUtils instance
    {
        get { return _instance ?? (_instance = AppDataUtils.Create()); }
    }

    public void Init()
    { }

    public void Init_FireBase()
    {
        //Initial itself with nothing

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {

            if (task.IsCompleted){
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    app = Firebase.FirebaseApp.DefaultInstance;

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                    Debug.Log("Firebase is ON now");

                    //Init Authentication handler
                    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

                    //Check
                    CheckUserExist();
                }
                else
                {
                    UnityEngine.Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.

                    //Do Call the method to end the Loading start Screen
                    if (methodHolder != null)
                    {
                        methodHolder();
                    }
                }
            }

            if (task.IsFaulted){
                UnityEngine.Debug.LogError(System.String.Format(
                    " Firebase Init Exception : {0}", task.Exception));
            }
        });
    }

    private static AppDataUtils Create() {

        string key = "_AppDataUtil_";
        GameObject myUtils = GameObject.Find(key);
        if (myUtils == null) {
            myUtils = new GameObject(key);
            myUtils.AddComponent<AppDataUtils>();
        }

        _instance = myUtils.GetComponent<AppDataUtils>();

#if !UNITY_EDITOR
#if UNITY_ANDROID

#endif  //UNITY_ANDROID
#endif  //UNITY_EDITOR

        return _instance;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //do check user
        
    }

    static public bool IsAOS()
    {
        if (Application.platform == RuntimePlatform.Android )
        {
            return true;
        }
        else
        {
            Debug.LogWarning(Application.platform.ToString() +
                " is not a supported platform for the Codeless IAP restore button");
        }
            
        return false;
    }

    static public bool IsIOS()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
                    Application.platform == RuntimePlatform.OSXPlayer ||
                    Application.platform == RuntimePlatform.tvOS)
        {
            return true;
        }
        else
        {
            Debug.LogWarning(Application.platform.ToString() +
                " is not a supported platform for the Codeless IAP restore button");
        }

        return false;
    }

    ///////  -------- Account --------- /////// 

    private void CheckUserExist()
    {
        if (auth != null && user == null && !isCheckedUserOnce)
        {
            Debug.Log("Firebase Auth Inst is on ! do CheckUserExist");
            user = auth.CurrentUser;
            if (auth.CurrentUser != null)
            {
                Debug.Log("CheckUserExist -  user exist ! [" + user.ProviderId + "]");
                //Set the latest login type

                isUserExist = true;
            }
            else {
                Debug.Log("CheckUserExist -  user not exist ! ");
            }

            isCheckedUserOnce = true;

            //End the process , Call the delegate method to continue the loading process
            if (methodHolder != null)
            {
                methodHolder();
            }
            else {
                Debug.Log("methodHolder is in null! Please check");
            }
        }
    }

    public static Firebase.Auth.FirebaseAuth FirebaseAuthInst => auth;
    public static Firebase.FirebaseApp FirebaseAppInst => app;
    public Firebase.Auth.FirebaseUser LatestUserInfo => user;
    public bool IsUserIDExisting()
    {
        return isUserExist;
    }


    ///////  -------- Local Data Store --------- /////// 

    private bool IsKeyExisting(string in_key)
    {
        return PlayerPrefs.HasKey(in_key);
    }
    //----- get functions -----
    /// <summary>
    /// Get Integer Value 
    /// </summary>
    /// <param name="in_key"></param>
    /// <returns></returns>
    private int GetIntValByKey(string in_key)
    {
        if (in_key.Length != 0)
        {
            if (!IsKeyExisting(in_key))
            {
                //initial the value
                SetIntValByKey(in_key, 0);
            }
            else
            {
                //Direct return the stored value
                return PlayerPrefs.GetInt(in_key);
            }
        }
        return 0;
    }

    /// <summary>
    /// Get Float Value 
    /// </summary>
    /// <param name="in_key"></param>
    /// <returns></returns>
    private float GetFloatValByKey(string in_key)
    {
        if (in_key.Length != 0)
        {
            if (!IsKeyExisting(in_key))
            {
                //initial the value
                SetFloatValByKey(in_key, 0f);
            }
            else
            {
                //Direct return the stored value
                return PlayerPrefs.GetFloat(in_key);
            }
        }
        return 0f;
    }

    /// <summary>
    /// Get String Value 
    /// </summary>
    /// <param name="in_key"></param>
    /// <returns></returns>
    private string GetStringValByKey(string in_key)
    {
        if (in_key.Length != 0)
        {
            if (!IsKeyExisting(in_key))
            {
                //initial the value
                SetStringValByKey(in_key, "");
            }
            else
            {
                //Direct return the stored value
                return PlayerPrefs.GetString(in_key);
            }
        }
        return "";
    }

    /// <summary>
    /// Get total number of coins player is holding
    /// </summary>
    /// <returns>Current Player is holding number of game coins</returns>
    public int GetCurPlayerCoins()
    {
        return coins = GetIntValByKey(key_coins);
    }

    /// <summary>
    /// Get total number of cashCoins player is holding.
    /// This coin is not genarated in the game. Only earl by parchasing system.
    /// </summary>
    /// <returns>Current Player is holding number of cash coins.</returns>
    public int GetCurPlayerCashCoins()
    {
        return cashCoins = GetIntValByKey(key_cashCoins);
    }

    //----- set functions -----
    /// <summary>
    /// Set Integer Value 
    /// </summary>
    /// <param name="in_key"></param>
    /// <param name="in_val"></param>
    private void SetIntValByKey(string in_key, int in_val)
    {
        if (in_key.Length != 0)
        {
            PlayerPrefs.SetInt(in_key, in_val);
        }
    }

    /// <summary>
    /// Set Float Value 
    /// </summary>
    /// <param name="in_key"></param>
    /// <param name="in_val"></param>
    private void SetFloatValByKey(string in_key, float in_val)
    {
        if (in_key.Length != 0)
        {
            PlayerPrefs.SetFloat(in_key, in_val);
        }
    }
    /// <summary>
    /// Set String Value 
    /// </summary>
    /// <param name="in_key"></param>
    /// <param name=stringal"></param>
    private void SetStringValByKey(string in_key,string in_val)
    {
        if (in_key.Length != 0)
        {
            PlayerPrefs.SetString(in_key, in_val);
        }
    }

    /// <summary>
    /// Update Local Coin
    /// </summary>
    /// <param name="in_coin"></param>
    public void UpdateLocalCoin(int in_coin)
    {
        //Should do server request for update the in_coin
        if (in_coin >= 0 ) {
            SetIntValByKey(key_coins, in_coin);
        }
    }

    /// <summary>
    /// Update Local Game Coin
    /// </summary>
    /// <param name="in_coin"></param>
    public void UpdateLocalGameCoin(int in_coin)
    {
        //Should do server request for update the in_coin
        if (in_coin >= 0)
        {
            SetIntValByKey(key_cashCoins, in_coin);
        }
    }


    /////// -------- Scene handling --------- /////// 


    public void SetAppLaunchCompletionBlock( FirebaseReadyDelegate in_method  )
    {
        if (in_method != null)
        {
            if (methodHolder != in_method)
            {
                methodHolder = in_method;

                //Cyrus : Delegate set , then do start Initial Firebase process
                Init_FireBase();
            }
        }
        
    }

    public void LoadDashboardAfterloginSuccess()
    {
        LoadDashboard();
    }

    public void LoadShop()
    {
        Debug.Log("function::LoadShop called");
        GetComponent<SceneHandler>().SetToScene("Scene_InAppPurchase");
    }
    public void LoadDashboard()
    {
        Debug.Log("function::LoadShop called");
        GetComponent<SceneHandler>().SetToScene("Scene_Main");
    }

}
