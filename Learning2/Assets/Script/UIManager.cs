using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 常量UI面板名
/// </summary>
public class ConstUIName
{
    public const string packagePanel = "PackagePanel";
    public const string audioPanel = "AudioPanel";
    public const string interfaceSettingPanel = "InterfaceSettingPanel";
    public const string homePanel = "HomePanel";
    public const string lootsBoxPanel = "LootsBoxPanel";
    public const string equipmentPanel = "EquipmentPanel";
}

public class UIManager
{
    private Dictionary<string,string> prefabPathDic;   //预制体路径
    private Dictionary<string, GameObject> panelPrefabDic; //预制体缓存
    public Dictionary<string, BasePanel> openPanelsDic; //已打开面板

    public string CurrentActivePanel {get ; private set;} //当前已打开面板

    private PlayerInput _playerInput;
    private PlayerInput PlayerInput
    {
        get
        {
            if(_playerInput == null)
            {
                //通过标签查找
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if(playerObj != null)
                {
                    _playerInput = playerObj.GetComponent<PlayerInput>();
                }
                if(_playerInput == null)
                {
                    Debug.LogWarning("未找到PlayerInput组件");
                }
            }
            return _playerInput;
        }
    }

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

        canvasObj.AddComponent<GraphicRaycaster>();

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
        CurrentActivePanel = string.Empty;
        prefabPathDic = new Dictionary<string, string>()
        {
            {ConstUIName.packagePanel, "Package/PackagePanel"},
            {ConstUIName.audioPanel,"AudioSetting/AudioPanel"},
            {ConstUIName.interfaceSettingPanel,"InterfaceSetting/InterfaceSettingPanel"},
            {ConstUIName.homePanel,"HomePanel"},
            {ConstUIName.lootsBoxPanel, "LootBox/BoxPanel"},
            {ConstUIName.equipmentPanel,"Equipment/EquipmentPanel"},
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

        //实例化面板
        GameObject panelObj = GameObject.Instantiate(panel,UIRoot,false);
        basePanel = panelObj.GetComponent<BasePanel>();
        openPanelsDic.Add(panelName, basePanel);
        CurrentActivePanel = panelName;

        SwitchUIActionMap();
        
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

        /* if(CurrentActivePanel == panelName)
        {
            CurrentActivePanel = string.Empty;
            Debug.Log($"已关闭当前面板：{panelName}");
        } */

        basePanel.ClosePanel();
        Debug.Log($"已关闭面板{panelName}, 剩余打开面板数量：{openPanelsDic.Count}");
        
        CurrentActivePanel = openPanelsDic.Count > 0 ? openPanelsDic.Keys.Last() : string.Empty;  //获取最后一个打开的面板
        if(openPanelsDic.Count == 0)
        {
            SwitchGameActionMap();    
        }
        else SwitchUIActionMap();
           
        return true;
    }
/// <summary>
/// 关闭当前面板
/// </summary>
/// <returns></returns>
    public bool CloseCurrentActivePanel()
    {
        if(string.IsNullOrEmpty(CurrentActivePanel))
        {
            Debug.Log("当前无已激活面板");
            return false;
        }

        return ClosePanel (CurrentActivePanel);
    }
/// <summary>
/// 清除打开面板缓存
/// </summary>
/// <returns></returns>
    public bool ClearnOpenPanelDict()
    {
        openPanelsDic.Clear();
        return true;
    }
/// <summary>
/// 切换到UI控制
/// </summary>
    private void SwitchUIActionMap()
    {
        if(PlayerInput != null)
        {
            PlayerInput.EnableUIActionMap();
             Debug.Log("切换到UI控制");
        }
    }
/// <summary>
/// 切换到Game控制
/// </summary>
    private void SwitchGameActionMap()
    {
        if(PlayerInput != null)
        {
            PlayerInput.EnableGameplayerInput();
             Debug.Log("切换到GAME控制");
        }
    }
}
