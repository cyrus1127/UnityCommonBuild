using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginViewController : MonoBehaviour
{
    public enum loginType
    {
        login_registered,
        login_register,
        login_google,
        login_facebook,
        login_apple,
        login_guest
    };

    private AppDataUtils mainData;

    public InputField account_Id;
    public InputField account_pwd;
    public GameObject account_LoginRequest_panel;
    public GameObject account_LoginAlready_panel;
    public GameObject account_LoginError_panel;
    public Text account_curID_text;
    public Text account_curType_text;
    public Text txt_Contents;

    private bool needToShowErrorMsg;
    private bool isLoginPanelNeed = false;

    ///Login
    private loginType curSelectedType;
    private loginType latestSelectedType;
    /// Login - Google config
    public string googleIdToken;
    public string googleAccessToken;

    // Start is called before the first frame update
    void Start()
    {
        CheckUserExist();

        GameObject utilsStorer = GameObject.Find("Data");
        if (mainData == null && utilsStorer) {
            mainData = GameObject.Find("Data").GetComponent<AppDataUtils>();
        }    
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }

    private void FixedUpdate()
    {
        {///Update Panel
            if (account_LoginRequest_panel != null) { account_LoginRequest_panel.SetActive(isLoginPanelNeed); }
            if (account_LoginAlready_panel != null) { account_LoginAlready_panel.SetActive(!isLoginPanelNeed); }
        }

        if (needToShowErrorMsg)
        {
            PopErrorMessageBox();
        }
    }


    ///Functions
    ///
    public void DoLogout()
    {
        AppDataUtils.FirebaseAuthInst.SignOut();
        CheckUserExist();
    }

    public void LoginNormal() { LoginWith(loginType.login_registered); }
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
                else
                {
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
        if (mainData != null)
        {
            if (mainData.IsUserIDExisting())
            {
                Debug.Log("LoginViewController CheckUserExist -  user exist ! ");
                //Set the latest login type
                string id = mainData.LatestUserInfo.ProviderId;
                if (id.Length > 0)
                {
                    if (account_curID_text)
                    {
                        account_curID_text.text = account_curID_text.text.ToString() + id;
                    }
                    isLoginPanelNeed = true;
                }
                else
                {
                    //do nothing
                }
            }
        }

    }

    private void DoLoginProcess()
    {
        //Do show Loading
        if (curSelectedType != latestSelectedType)
        {
            //do switching
        }
        else
        {

        }

        Firebase.Auth.FirebaseUser user = AppDataUtils.FirebaseAuthInst.CurrentUser;
        if (user != null)
        {
            Debug.Log("DoLoginProcess - login ::  user not null ! ");
            //Set the latest login type
            latestSelectedType = curSelectedType;

            mainData.LoadDashboardAfterloginSuccess();
        }
        else
        {
            Debug.Log(" user not exist ! ");
            if (curSelectedType == loginType.login_google)
            {
                Debug.Log("DoLoginProcess - login_google ");
                Firebase.Auth.Credential credential =
    Firebase.Auth.GoogleAuthProvider.GetCredential(googleIdToken, googleAccessToken);
                AppDataUtils.FirebaseAuthInst.SignInWithCredentialAsync(credential).ContinueWith(task =>
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

                    mainData.LoadDashboardAfterloginSuccess();
                });

            }
            else if (curSelectedType == loginType.login_register)
            {
                Debug.Log("DoLoginProcess - login_register");
                AppDataUtils.FirebaseAuthInst.SignInWithEmailAndPasswordAsync(account_Id.text, account_pwd.text).ContinueWith(task =>
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

                    mainData.LoadDashboardAfterloginSuccess();
                });

            }
            else if (curSelectedType == loginType.login_guest)
            {
                Debug.Log("DoLoginProcess - login_guest");
                AppDataUtils.FirebaseAuthInst.SignInAnonymouslyAsync().ContinueWith(task =>
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

                    mainData.LoadDashboardAfterloginSuccess();
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
        else
        {
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
            else
            {

            }
        }
    }

}
