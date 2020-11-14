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
using System.Threading.Tasks;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

public class AppDataUtils : MonoBehaviour
{

    public enum loginType {
        login_registered,
        login_register,
        login_google,
        login_facebook,
        login_apple,
        login_guest
    };

    public InputField account_Id;
    public InputField account_pwd;
    public GameObject account_LoginRequest_panel;
    public GameObject account_LoginAlready_panel;
    public GameObject account_LoginError_panel;
    public Text account_curID_text;
    public Text account_curType_text;
    public Text txt_Contents;

    private bool needToShowErrorMsg;

    ///Login
    private loginType curSelectedType;
    private loginType latestSelectedType;
    /// Login - Firebase
    private Firebase.Auth.FirebaseAuth auth;
    private static Firebase.FirebaseApp app;
    /// Login - Google config
    public string googleIdToken;
    public string googleAccessToken;
    

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
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
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
        Init_FireBase();
    }

    // Update is called once per frame
    void Update()
    {
        //do check user
        CheckUserExist();
        //Show Error if need
        PopErrorMessageBox();
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


    public void LoginNormal(){LoginWith(loginType.login_registered); }
    public void LoginRegister() { LoginWith(loginType.login_register); }
    public void LoginGoogle() { LoginWith(loginType.login_google); }
    public void LoginFacebook() { LoginWith(loginType.login_facebook); }
    public void LoginApple() { LoginWith(loginType.login_apple); }
    public void LoginGuest() { LoginWith(loginType.login_guest); }

    private void LoginWith(loginType type)
    {
        curSelectedType = type;
        switch (curSelectedType)
        {
            case loginType.login_register:
                //Do Registeration flow
                if (account_Id.text.Length == 0 || account_pwd.text.Length == 0)
                {
                    Debug.Log("Account ID/Password can not be empty !");
                }
                else {
                    //Do auto fill and open another dialog for next step
                    DoLoginProcess();
                }
                break;
            case loginType.login_registered:
                DoLoginProcess();
                break;
            case loginType.login_google:
                //Call google Login flow
                DoLoginProcess();
                break;
            case loginType.login_facebook:
                //Call facebook Login flow
                if (account_LoginRequest_panel != null) { account_LoginRequest_panel.SetActive(false); }
                if (account_LoginAlready_panel != null) { account_LoginAlready_panel.SetActive(true); }
                break;
            case loginType.login_apple:
                //Call apple Login flow
                break;
            case loginType.login_guest:
                //Call apple Login flow
                DoLoginProcess();
                break;
        }

    }

    
    private void CheckUserExist()
    {
        if (auth != null) {
            Firebase.Auth.FirebaseUser user = auth.CurrentUser;
            bool userExist = false;
            if (user != null)
            {
                Debug.Log("CheckUserExist -  user exist ! ");
                //Set the latest login type
                string id = user.ProviderId;
                if (account_curID_text)
                {
                    account_curID_text.text = account_curID_text.text.ToString() + id;
                }
                userExist = true;
            }

            if (account_LoginRequest_panel != null) { account_LoginRequest_panel.SetActive(!userExist); }
            if (account_LoginAlready_panel != null) { account_LoginAlready_panel.SetActive(userExist); }
        }
        
    }

    public void DoLogout()
    {
        auth.SignOut();
        CheckUserExist();
    }

    public void DoLoginProcess()
    {
        //Do show Loading
        if (curSelectedType != latestSelectedType)
        {
            //do switching
        }
        else {

        }

        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            Debug.Log("DoLoginProcess - login ::  user not null ! ");
            //Set the latest login type
            latestSelectedType = curSelectedType;

            LoadDashboardAfterloginSuccess();
        }
        else
        {
            Debug.Log(" user not exist ! ");
            if (curSelectedType == loginType.login_google)
            {
                Debug.Log("DoLoginProcess - login_google ");
                Firebase.Auth.Credential credential =
    Firebase.Auth.GoogleAuthProvider.GetCredential(googleIdToken, googleAccessToken);
                auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SignInWithCredentialAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                        SetErrorMessageBox(task.Exception.ToString());
                        return;
                    }

                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    Debug.LogFormat("User signed in successfully: {0} ({1})",
                        newUser.DisplayName, newUser.UserId);

                    //Set the latest login type
                    latestSelectedType = curSelectedType;

                    LoadDashboardAfterloginSuccess();
                });

            }
            else if (curSelectedType == loginType.login_register)
            {
                Debug.Log("DoLoginProcess - login_register");
                auth.SignInWithEmailAndPasswordAsync(account_Id.text, account_pwd.text).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                        SetErrorMessageBox(task.Exception.ToString());
                        return;
                    }

                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    Debug.LogFormat("EmailReguester User signed in successfully: {0} ({1})",
                        newUser.DisplayName, newUser.UserId);

                    //Set the latest login type
                    latestSelectedType = curSelectedType;

                    LoadDashboardAfterloginSuccess();
                });
                    
            }
            else if (curSelectedType == loginType.login_guest)
            {
                Debug.Log("DoLoginProcess - login_guest");
                auth.SignInAnonymouslyAsync().ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SignInAnonymouslyAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                        SetErrorMessageBox(task.Exception.ToString());
                        return;
                    }

                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    Debug.LogFormat("Guest User signed in successfully: {0} ({1})",
                        newUser.DisplayName, newUser.UserId);

                    //Set the latest login type
                    latestSelectedType = curSelectedType;

                    LoadDashboardAfterloginSuccess();
                });
            }
        }

        ////Cyrus : do simulate real connection
        //yield return new WaitForSeconds(0.5f);
    }

    private void SetErrorMessageBox(string msg)
    {
        if (account_LoginError_panel != null)
        {
            Debug.Log("SetErrorMessageBox Msg set !  \n msg : " + msg);
            account_LoginError_panel.SetActive(true);
            txt_Contents.text = "" + msg;
            needToShowErrorMsg = true;
        }
        else {
            Debug.Log("SetErrorMessageBox no Msg set ! ");
        }
    }

    private void PopErrorMessageBox()
    {
        if (account_LoginError_panel != null && txt_Contents != null)
        {
            if (!account_LoginError_panel.activeInHierarchy && txt_Contents.text.Length > 0 && needToShowErrorMsg)
            {
                account_LoginError_panel.SetActive(true);
                needToShowErrorMsg = false;
            }
            else {

            }
        }
    }

    //Scene handling
    void LoadDashboardAfterloginSuccess()
    {
        CheckUserExist();
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
