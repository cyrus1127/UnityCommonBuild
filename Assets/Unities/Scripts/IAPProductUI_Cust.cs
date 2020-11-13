//
// IAPProductUI_Cust.cs
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



#if UNITY_PURCHASING || UNITY_UNIFIED_IAP

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class IAPProductUI_Cust : MonoBehaviour
{
    public Button purchaseButton;
    public Button receiptButton;
    public Text titleText;
    public Text descriptionText;
    public Text priceText;
    public Text statusText;
    public Image imageView;

    private string m_ProductID;
    private Action<string> m_PurchaseCallback;
    private string m_Receipt;

    public void SetProduct(Product p, Action<string> purchaseCallback)
    {
        titleText.text = p.metadata.localizedTitle;
        descriptionText.text = p.metadata.localizedDescription;
        priceText.text = p.metadata.localizedPriceString;

        receiptButton.interactable = p.hasReceipt;
        m_Receipt = p.receipt;

        m_ProductID = p.definition.id;
        m_PurchaseCallback = purchaseCallback;

        statusText.text = p.availableToPurchase ? "Available" : "Unavailable";

        //set Image
        //string spriteName = p.definition.;
        if (m_ProductID.Contains("coins_package_option_a")) {
            string[] parts =  m_ProductID.Split("_".ToCharArray());
            string imageIndex = parts[parts.Length - 1]; //get the last digit
            imageIndex = imageIndex.Substring(1);
            imageView.sprite = Resources.Load<Sprite>("IAP_Shop/" + "IAP_C0"+ imageIndex);
        }else if (m_ProductID.Contains("sub"))
        {//so ads
            imageView.sprite = Resources.Load<Sprite>("IAP_Shop/" + "IAP_C00");
        }
        


    }

    public void SetPendingTime(int secondsRemaining)
    {
        statusText.text = "Pending " + secondsRemaining.ToString();
    }

    public void PurchaseButtonClick()
    {
        if (m_PurchaseCallback != null && !string.IsNullOrEmpty(m_ProductID))
        {
            m_PurchaseCallback(m_ProductID);
        }
        else {
            
           Debug.Log("PurchaseButtonClick : m_PurchaseCallback is Null");
        }
    }

    public void ReceiptButtonClick()
    {
        if (!string.IsNullOrEmpty(m_Receipt))
            Debug.Log("Receipt for " + m_ProductID + ": " + m_Receipt);
    }
}

#endif
