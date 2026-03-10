using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePanel : BasePanel
{
    [SerializeField] private Button UICloseButton;
    [SerializeField] private Button UIAudioButton;
    [SerializeField] private Button UIInterfaceButton;
    [SerializeField] private Button UISaveButton;
    [SerializeField] private Button UIReturnStartButton;

    protected override void Awake()
    {
        InitClick();
    }

    void InitClick()
    {
        UICloseButton.GetComponent<Button>().onClick.AddListener(OnClose);
        UIAudioButton.GetComponent<Button>().onClick.AddListener(OnAudio);
        UIInterfaceButton.GetComponent<Button>().onClick.AddListener(OnInterface);
        UISaveButton.GetComponent<Button>().onClick.AddListener(OnSave);
        UIReturnStartButton.GetComponent<Button>().onClick.AddListener(OnReturnStart);
    }

    private void OnClose()
    {
        UIManager.Instance.CloseCurrentActivePanel();
    }
/// <summary>
/// 音量设置
/// </summary>
    private void OnAudio()
    {
        UIManager.Instance.CloseCurrentActivePanel();
        UIManager.Instance.OpenPanel(ConstUIName.AUDIOPANEL);
    }
/// <summary>
/// 界面设置
/// </summary>
    private void OnInterface()
    {
        UIManager.Instance.CloseCurrentActivePanel();
        UIManager.Instance.OpenPanel(ConstUIName.INTERFACESETTINGPANEL);
    }
/// <summary>
/// 保存游戏
/// </summary>
/// <exception cref="NotImplementedException"></exception>
    private void OnSave()
    {
        UIManager.Instance.CloseCurrentActivePanel();
        //TODO
        Debug.Log("保存游戏");
        //游戏保存图标和提示
    }
/// <summary>
/// 返回开始界面
/// </summary>
/// <exception cref="NotImplementedException"></exception>
    private void OnReturnStart()
    {
        UIManager.Instance.CloseCurrentActivePanel();
        //TODO
        Debug.Log("返回开始界面");
    }
}
