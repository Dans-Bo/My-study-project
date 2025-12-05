using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 常量UI面板名
/// </summary>
public class ConstUIName
{
    public const string packagePanel = "PackagePanel";
    public const string audioPanel = "AudioPanel";
}

public class UIManager
{
    private Dictionary<string,string> prefabPathDic;   //预制体路径
    private Dictionary<string, GameObject> panelPrefabDic; //预制体缓存
    public Dictionary<string, BasePanel> openPanelsDic; //已打开面板

    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }
    private Transform _uiRoot;
    public Transform UIRoot
    {
        get
        {
            if( _uiRoot == null)
            {
                if(GameObject.Find("Canvas")) _uiRoot = GameObject.Find("Canvas").transform;
                else CreatCanvas();
                
            }
            return _uiRoot;
        }
    }
/// <summary>
/// 自动创建Canvas
/// </summary>
    private void CreatCanvas()
    {
        GameObject canvasObj = new GameObject("Canvas");

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920,1080);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

        _uiRoot = canvasObj.transform;
    }

    private UIManager()
    {
        UIInstance();
    }
/// <summary>
/// 初始化
/// </summary>
    private void UIInstance()
    {
        panelPrefabDic = new Dictionary<string, GameObject>();
        openPanelsDic = new Dictionary<string, BasePanel>();
        prefabPathDic = new Dictionary<string, string>()
        {
            {ConstUIName.packagePanel, "Package/PackagePanel"},
            {ConstUIName.audioPanel,"AudioSetting/AudioPanel"}
        };
    }
/// <summary>
/// 打开UI面板
/// </summary>
/// <param name="panelName">面板名</param>
/// <returns></returns>
    public BasePanel OpenPanel(string panelName)
    {
        //如果在已打开面板字典中，直接返回
        BasePanel basePanel;
        if(openPanelsDic.TryGetValue(panelName, out basePanel))
        {
            #if UNITY_EDITOR
            Debug.Log($"{panelName}界面已打开");
            #endif
            return basePanel;
        }
        //检查面板预制件路径是否正确
        string path;
        if(!prefabPathDic.TryGetValue(panelName, out path))
        {
            #if UNITY_EDITOR
            Debug.Log($"{panelName}界面名称错误或{path}路径不存在，请检查");
            #endif
            return null;
        }
        //如果预制件缓存中没有，则缓存
        GameObject panel;
        if(!panelPrefabDic.TryGetValue(panelName, out panel))
        {
            string panelPath = "Prefabs/UI/" + path;
            panel = Resources.Load<GameObject>(panelPath);
            if (panel == null) // 增加预制体加载失败判断
            {
                Debug.LogError($"预制体加载失败！路径：{panelPath}");
                return null;
            }
            panelPrefabDic.Add(panelName, panel);
        }

        //打开面板
        GameObject panelObj = GameObject.Instantiate(panel,UIRoot,false);
        basePanel = panelObj.GetComponent<BasePanel>();
        openPanelsDic.Add(panelName, basePanel);
        
        basePanel.OpenPanel(panelName);
        return basePanel;
    }
/// <summary>
/// 关闭ui面板
/// </summary>
/// <param name="panelName">面板名</param>
/// <returns></returns>
    public bool ClosePanel(string panelName)
    {
        BasePanel basePanel;
        if(!openPanelsDic.TryGetValue(panelName, out basePanel))
        {
            #if UNITY_EDITOR
            Debug.Log($"{panelName}界面未打开");
            #endif
            return false;
        }

        basePanel.ClosePanel();
        return true;
    }

    public bool ClearnOpenPanelDict()
    {
        openPanelsDic.Clear();
        return true;
    }
}
