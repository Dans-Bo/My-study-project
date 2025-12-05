using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PanelMode
{
    normal, delect
}
public class PackagePanel : BasePanel
{
    [SerializeField] private Transform UIScrollView;
    [SerializeField] private Transform UIcloseButton;
    [SerializeField] private Transform UItypeName;
    [SerializeField] private Transform UIleftButton;
    [SerializeField] private Transform UIrightButton;
    [SerializeField] private Transform UIDelectButton;
    [SerializeField] private Transform UIDelectPanel;
    [SerializeField] private Transform UIBackButton;
    [SerializeField] private Transform UIConfirmDelectButton;
    public PanelMode currentPanelMode{ get ; private set;} = PanelMode.normal; //当为删除模式时，点击物品则存储其uid，确认删除则直接remove
    private ItemType[] typeSwitch = {ItemType.equipment,ItemType.food,ItemType.potion,ItemType.material}; //页面切换，用于上一页，下一页
    private int currentTypeIndex; //当前背包物品类型的index，用于上下页切换
    private ItemType currentPanelItemType; //当前背包物品类型，
    [SerializeField]private GameObject cellPrefab; 

    public List<string> delectUID{get;private set;} //记录删除模式下选中的物品

    private string _chooseUID;  //选中单个物体
    public string ChooseUID
    {
        get { return _chooseUID; }
        set
        {
            _chooseUID = value;
        }
    } 

    [Serializable]
    public struct MenuButton  
    {
        public Button button;
        public GameObject normalUI;
        public GameObject selectUI;
        public ItemType itemType;

    }

    [SerializeField] private MenuButton[] menuButtons;  //页面切换管理

    void Awake()
    {
        InitClick();
        InitUI();
        RefreshScrollView();
    }

    void Start()
    {
        currentPanelItemType = ItemType.equipment;
    }
    void InitUI()
    {
        UIDelectPanel.gameObject.SetActive(false);
    }

    void InitClick()
    {
        UIcloseButton.GetComponent<Button>().onClick.AddListener(OnClickClose);
        UIleftButton.GetComponent<Button>().onClick.AddListener(OnClickLeft);
        UIrightButton.GetComponent<Button>().onClick.AddListener (OnClickRight);
        UIDelectButton.GetComponent<Button>().onClick.AddListener(OnClickDelect);
        UIBackButton.GetComponent<Button>().onClick.AddListener(OnClickBack);
        UIConfirmDelectButton.GetComponent<Button>().onClick.AddListener(OnClickConfirmDelect);

        foreach(var button in menuButtons)
        {
            if(button.button == null)
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"有空按钮");
                #endif
                continue;
            }

            ItemType type = button.itemType;
            button.button.onClick.AddListener(() =>
            {
                currentTypeIndex = GetTypeIndex(type);
                OnClickButtonByType(type);
            });
        }
    }
#region UI菜单按钮
    private void OnClickButtonByType(ItemType type)
    {

        currentPanelItemType = type;

        UItypeName.GetComponent<Text>().text = GetTypeName(type);
        
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseClick);

        //刷新背包格子
        RefreshScrollView();
        //更新按钮选中状态
        UpdataButtonState(type);
    }

    private void UpdataButtonState(ItemType type)
    {
        if(menuButtons == null || menuButtons.Length == 0) return;

        foreach(var button in menuButtons)
        {
            if(button.normalUI == null || button.selectUI == null) continue;

            bool isSelect = button.itemType == type;
            button.selectUI.SetActive(isSelect);
            button.normalUI.SetActive(!isSelect);
            button.button.interactable = true;
        }
    }
/// <summary>
/// 显示背包类型名称
/// </summary>
/// <param name="type"></param>
/// <returns></returns>
    private string GetTypeName(ItemType type)
    {
        return type switch
        {
            ItemType.equipment => "装备",
            ItemType.food => "食物",
            ItemType.potion => "药水",
            ItemType.material =>"材料",
            _ => "杂货",
        };
    }
/// <summary>
/// 获得页面切换数组index
/// </summary>
/// <param name="type"></param>
/// <returns></returns>
/// <exception cref="NotImplementedException"></exception>
    private int GetTypeIndex(ItemType type)
    {
        for(int i = 0; i< typeSwitch.Length; ++i)
        {
            if(typeSwitch[i] == type) return i;
        }
        return 0;
    }
#endregion
    
    /// <summary>
    /// 刷新背包中物品
    /// </summary>
    void RefreshScrollView()
    {
        RectTransform scrollCountent = UIScrollView.GetComponent<ScrollRect>().content;
        //删除容器中原本的物品
        for(int i = 0; i < scrollCountent.childCount; i++)
        {
            Destroy(scrollCountent.GetChild(i).gameObject);
        }

        List<PackageLocalTableData> items = PackageDataManage.Instance.CachedPackageData;
        
        foreach(var localItem in items)
        {
            PackageTableData data = PackageDataManage.Instance.GetPackageItem_ByID(localItem.itemID);
            if(data == null) continue;

            if(data.itemType == currentPanelItemType)
            {
                Transform packageItem = Instantiate(cellPrefab.transform, scrollCountent);
                PackageCell cell = packageItem.GetComponent<PackageCell>();

                // 背包格子更新
                cell.Refresh(localItem,this);
            }
        }
    }

    #region 删除模式
/// <summary>
/// 存入选中要删除的UID
/// </summary>
/// <param name="uid"></param>
    public void AddDelectUID(string uid)
    {
        delectUID ??= new List<string>();

        if(!delectUID.Contains(uid))
        {
            delectUID.Add(uid);
            Debug.Log($"将物品加入删除列表中，物品uid：{uid}");
        } 
        else
        {
            delectUID.Remove(uid);
            Debug.Log($"以将物品移除删除列表，物品uid:{uid}");
        }
    }
/// <summary>
/// 退出删除模式刷新格子
/// </summary>
    private void RefreshDelectState()
    {
        RectTransform scroll = UIScrollView.GetComponent<ScrollRect>().content;
        foreach(Transform cell in scroll)
        {
            PackageCell packageCell = cell.GetComponent<PackageCell>();
            packageCell.RefreshDelectState();
        }
    }


    #endregion

#region 按钮事件
/// <summary>
/// 进入删除模式
/// </summary>
    private void OnClickDelect()
    {
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseClick);
        currentPanelMode = PanelMode.delect;
        UIDelectButton.gameObject.SetActive(false);
        UIDelectPanel.gameObject.SetActive(true);
    }
    /// <summary>
    /// 推出删除模式
    /// </summary>
    private void OnClickBack()
    {
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseClick);
        currentPanelMode = PanelMode.normal;
        RefreshDelectState();

        delectUID.Clear();
        Debug.Log($"清空了删除uid列表，列表中有 {delectUID.Count}个物品");
        
        UIDelectPanel.gameObject.SetActive(false);
        UIDelectButton.gameObject.SetActive(true);
        
    }
/// <summary>
/// 确认删除
/// </summary>
    private void OnClickConfirmDelect()
    {
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseClick);
        if(delectUID == null || delectUID.Count == 0) return;

        PackageDataManage.Instance.DelectItems(delectUID);

        RefreshScrollView();
    }
    /// <summary>
    /// 下一页
    /// </summary>
    private void OnClickRight()
    {
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseClick);
        if(typeSwitch.Length == 0) return;

        currentTypeIndex = (currentTypeIndex +1 + typeSwitch.Length)%typeSwitch.Length;
        ItemType itemType = typeSwitch[currentTypeIndex];

        OnClickButtonByType(itemType);
    }
/// <summary>
/// 上一页
/// </summary>
    private void OnClickLeft()
    {
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseClick);
        if(typeSwitch.Length == 0) return;

        currentTypeIndex = (currentTypeIndex -1 + typeSwitch.Length)%typeSwitch.Length;
        ItemType itemType = typeSwitch[currentTypeIndex];

        OnClickButtonByType(itemType);
    }
/// <summary>
/// 关闭页面
/// </summary>
    private void OnClickClose()
    {
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseClick);
        ClosePanel();
    }
    #endregion
}
