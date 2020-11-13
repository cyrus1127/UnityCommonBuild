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

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AppDataUtils : MonoBehaviour
{

    public enum loginType {
        login_register,
        login_registered,
        login_google,
        login_facebook,
        login_apple
    };

    public InputField account_Id;
    public InputField account_pwd;


    private static AppDataUtils _instance = null;
    public static AppDataUtils instance
    {
        get { return _instance ?? (_instance = AppDataUtils.Create()); }

    }

    public void Init()
    {
        //Initial itself with nothing
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

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        
    }

    // Update is called once per frame
    void Update()
    {

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

    private void LoginWith(loginType type)
    {
        switch (type)
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
                //Do normal Login flow
                if (account_Id.text.Length == 0 || account_pwd.text.Length == 0)
                {
                    Debug.Log("Account ID/Password can not be empty !");
                }
                else {
                    //Do check server responce
                    StartCoroutine(DoLoginProcess());
                }
                break;
            case loginType.login_google:
                //Call google Login flow
                break;
            case loginType.login_facebook:
                //Call facebook Login flow
                break;
            case loginType.login_apple:
                //Call apple Login flow
                break;
        }

    }

    
    IEnumerator DoLoginProcess()
    {
        //Do show Loading
        if(true){
            //coding here.....
        }

        //Cyrus : do simulate real connection
        yield return new WaitForSeconds(0.5f);

        LoadDashboardAfterloginSuccess();
    }


    //Scene
    private void LoadDashboardAfterloginSuccess()
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
