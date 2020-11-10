//
// IAP_Cust.cs
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
using System.Collections.Generic;
using UnityEngine;

public class RollingCabe : MonoBehaviour
{
    public float rotation_speed = 0.5f;
    public Vector3 RollingAsix;

    private void Start()
    {

    }

    [Obsolete]
    private void Update()
    {
        UpdateSelfRotation();
    }

    [Obsolete]
    private void UpdateSelfRotation()
    {

        transform.RotateAroundLocal(RollingAsix, Time.deltaTime * rotation_speed);
    }
}
