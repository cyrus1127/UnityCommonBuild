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

using UnityEngine;

public class AppDataUtils : MonoBehaviour
{
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

    
}
