using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPageController : MonoBehaviour
{
    public Button btn_page_close;

    public GameObject dataUtilblock;

    // Start is called before the first frame update
    void Start()
    {
        dataUtilblock = GameObject.Find("Data");

        //do the on-click action
        if (btn_page_close)
        {
            btn_page_close.onClick.AddListener(ButtonOnClick);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ButtonOnClick()
    {
        if (dataUtilblock)
        {
            AppDataUtils dataUtils = dataUtilblock.GetComponent<AppDataUtils>();

            dataUtils.LoadDashboard();
        }
        else
        {
            Debug.Log("Not find GameObject [Data]");
        }
    }
}
