using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashboardPageController : MonoBehaviour
{

    public Button btn_page_shop;
    public Button btn_page_reward;
    public Button btn_page_stage;
    public Button btn_page_info;
    public Button btn_page_setting;

    public GameObject dataUtilblock;

    // Start is called before the first frame update
    void Start()
    {
        dataUtilblock = GameObject.Find("Data");


        //do the on-click action
        if (btn_page_shop)
        {
            btn_page_shop.onClick.AddListener(ButtonOnClick);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ButtonOnClick() {
        
        
        if (dataUtilblock) {
            AppDataUtils dataUtils = dataUtilblock.GetComponent<AppDataUtils>();

            dataUtils.LoadShop();
        }
        else
        {
            Debug.Log("Not find GameObject [Data]");
        }
    }


}
