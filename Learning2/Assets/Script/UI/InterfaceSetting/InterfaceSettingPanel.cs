using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceSettingPanel : BasePanel
{
    [SerializeField] private Toggle UIToggle2560x1440;
    [SerializeField] private Toggle UIToggle1920x1080;
    [SerializeField] private Toggle UIToggle1280x720;
    [SerializeField] private Toggle UIToggleFullScreen;
    [SerializeField] private Button UIApplyButton;
    [SerializeField] private Button UIReturnButton;
    [SerializeField] private Button UICloseButton;

    //初始状态，打开面板时的实际设置
    private int initWidth;
    private int initHeight;
    private bool initFullScreen;

    //临时状态，更改但未应用
    private int tempWidth;
    private int tempHeight;
    private bool tempFullScreen;

    public override void OpenPanel(string UIname)
    {
        base.OpenPanel(UIname);

        //记录当前实际的分辨率和全屏状态
        initWidth = Screen.width;
        initHeight = Screen.height; 
        initFullScreen = Screen.fullScreen;

        //初始化临时状态为吃初始状态，修改时修改的是临时状态
        tempHeight = initHeight;
        tempWidth = initWidth;
        tempFullScreen = initFullScreen;

        //根据初始状态显示对应的Toggle
        SyncToggleToState(initWidth, initHeight, initFullScreen);
    }

    

    protected override void Awake()
    {
        InitClick();
    }

    private void InitClick()
    {
        UIToggle1280x720.onValueChanged.AddListener(delegate { OnResolutionChange(1280,720);});
        UIToggle1920x1080.onValueChanged.AddListener(delegate { OnResolutionChange(1920,1080);});
        UIToggle2560x1440.onValueChanged.AddListener(delegate { OnResolutionChange(2560,1440);});
        
        UIApplyButton.onClick.AddListener(OnApply);
        UIReturnButton.onClick.AddListener(OnReturn);
        UICloseButton.onClick.AddListener(OnClose);

        UIToggleFullScreen.onValueChanged.AddListener(OnFullScreen);
    }

    private void OnFullScreen(bool isFullScreen)
    {
        tempFullScreen = isFullScreen;
    }

    private void OnApply()
    {
        Screen.SetResolution(tempWidth,tempHeight,tempFullScreen);
        Debug.Log($"分辨率已调整为{tempWidth} x {tempHeight}"); 
    }

    private void OnReturn()
    {
        IsChange();
        UIManager.Instance.CloseCurrentActivePanel();
        UIManager.Instance.OpenPanel(ConstUIName.homePanel);
    }

    private void OnClose()
    {
        IsChange();
        UIManager.Instance.CloseCurrentActivePanel();
    }
    
    private void OnResolutionChange(int width, int height)
    {
        if(UIToggle1280x720.isOn || UIToggle1920x1080.isOn || UIToggle2560x1440.isOn)
        {
            tempWidth = width;
            tempHeight = height;
            Debug.Log($"临时修改分辨率为{width} x {height}");
            /* Screen.SetResolution(v1,v2,Screen.fullScreen);
            Debug.Log($"分辨率已调整为{v1} x {v2}"); */
        }
    }

    private void SyncToggleToState(int width, int height, bool isFullScreen)
    {
        //同步分辨率Toggle
        UIToggle2560x1440.isOn =  width == 2560 && height == 1440;
        UIToggle1920x1080.isOn =  width == 1920 && height == 1080;
        UIToggle1280x720.isOn =  width == 1280 && height == 720;

        // 同步全屏Toggle
        UIToggleFullScreen.isOn = isFullScreen;
    }

    private void IsChange()
    {
        bool isChange = tempWidth != initWidth
                        || tempHeight != initHeight
                        || tempFullScreen != initFullScreen;

        if(isChange)
        {
            tempWidth = initWidth;
            tempHeight = initHeight;
            tempFullScreen = initFullScreen;

            SyncToggleToState(initWidth,initHeight,initFullScreen);
            Debug.Log("恢复到之前的设置");
        } 
    }
}
