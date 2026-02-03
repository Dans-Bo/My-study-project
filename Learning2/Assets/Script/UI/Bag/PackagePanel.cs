using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public enum PanelMode
{
    normal, delect
}
public class PackagePanel : BasePanel,IPointerClickHandler
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
    private List<PackageCell> cellPool = new List<PackageCell>(); //背包格子对象池
    private Dictionary<string, PackageCell> uidToCell = new();  // uid与背包格子的映射，查找对应物品的格子
    public List<string> DelectUID{get;private set;} = new List<string>(); //记录删除模式下选中的物品
    private Canvas panelCanvas;
    private PlayerEquipmentManager equipmentManager;

    private string _chooseUID;  //选中单个物体
    public string ChooseUID
    {
        get { return _chooseUID; }
        set
        {
            _chooseUID = value;
        }
    } 
    [SerializeField] private GameObject uiItemDetailsPanel; 

    [Serializable]
    public struct MenuButton  
    {
        public Button button;
        public GameObject normalUI;
        public GameObject selectUI;
        public ItemType itemType;

    }

    [SerializeField] private MenuButton[] menuButtons;  //页面切换按钮组


    protected override void Awake()
    {
        currentPanelItemType = ItemType.equipment;
        InitClick();
        InitUI();
        RefreshScrollView();
        uiItemDetailsPanel.SetActive(false);
        if(!UIManager.Instance.UIRoot.TryGetComponent<Canvas>(out panelCanvas)) Debug.LogError("获取canvas失败"); 
    }

    void Start()
    {
        var obj = GameObject.FindWithTag("Player");
        if(! obj.TryGetComponent<PlayerEquipmentManager>( out equipmentManager)) Debug.LogError($"获取装备管理组件失败");
    }

    void OnEnable()
    {
        if(equipmentManager != null)
        {
            equipmentManager.OnEquipmentEquipped += OnEquipmentStateChanged;
            equipmentManager.OnEquipmentUnequipped += OnEquipmentStateChanged;
        }
    }

    void OnDisable()
    {
        if(equipmentManager != null)
        {
            equipmentManager.OnEquipmentEquipped -= OnEquipmentStateChanged;
            equipmentManager.OnEquipmentUnequipped -= OnEquipmentStateChanged;
        }
    }

    void InitUI()
    {
        UIDelectPanel.gameObject.SetActive(false);
    }

    void InitClick()
    {
        UIcloseButton.GetComponent<Button>().onClick.AddListener(OnClickCloseWrapper);
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
    /*void RefreshScrollView()
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
    }*/

    #region 刷新背包
    /// <summary>
    /// 刷新背包中物品(对象池)
    /// </summary>
    public void RefreshScrollView()
    {
         ScrollRect scrollRect = UIScrollView.GetComponent<ScrollRect>();
        if(scrollRect == null)
        {
            Debug.LogError($"UIscrollView无ScrollRect组件");
            return;
        }
        RectTransform scrollContent = scrollRect.content;

        //把所有复用格子放回对象池
        foreach(var cell in cellPool)  
        {
            cell.gameObject.SetActive(false);
        }

        List<PackageLocalTableData> items = PackageDataManage.Instance.CachedPackageData;
        if(items == null || items.Count == 0) return;
        uidToCell.Clear(); //清除uid与背包格子的映射

        int cellIndex = 0; //格子索引
        foreach(var localItem in items)
        {
            PackageTableData data = PackageDataManage.Instance.GetPackageItem_ByID(localItem.itemID);
            if(data == null || data.itemType != currentPanelItemType) continue;

            PackageCell cell;
            //对象池有就复用，没有就实例化
            if(cellIndex < cellPool.Count)
            {
                cell = cellPool[cellIndex];
                cell.gameObject.SetActive(true);
            }
            else
            {
                if(cellPrefab == null)
                {
                    Debug.LogError($"cellPrefab没有赋值");
                    return;
                }
                Transform packageItem = Instantiate(cellPrefab.transform,scrollContent);
                cell = packageItem.GetComponent<PackageCell>();
                cellPool.Add(cell); //加入对象池
            }

            cell.Refresh(localItem,this);

            //添加映射
            if(!string.IsNullOrEmpty(localItem.itemUID))
            {
                uidToCell[localItem.itemUID] = cell;
            }
            cellIndex ++;
        }
    }
    #endregion

    #region 删除模式
/// <summary>
/// 存入选中要删除的UID
/// </summary>
/// <param name="uid"></param>
    public void AddDelectUID(string uid)
    {
        if(!DelectUID.Contains(uid))
        {
            DelectUID.Add(uid);
            Debug.Log($"将物品加入删除列表中，物品uid：{uid}");
        } 
        else
        {
            DelectUID.Remove(uid);
            Debug.Log($"以将物品移除删除列表，物品uid:{uid}");
        }
    }
/// <summary>
/// 退出删除模式刷新格子
/// </summary>
    private void RefreshDelectState()
    {
        if(UIScrollView == null) return;

        RectTransform scroll = UIScrollView.GetComponent<ScrollRect>().content;
        foreach(Transform cell in scroll)
        {
            PackageCell packageCell = cell.GetComponent<PackageCell>();
            if(packageCell == null) continue;

            packageCell.UISelect.gameObject.SetActive(false);//强制关闭选中标识
        }
    }
    #endregion

    private async Task SyncEquipStateToLocalTables()
    {
        if(equipmentManager == null) return;

        List<PackageLocalTableData> localData = PackageLocalTable.Instance.LocalTables;
        if(localData == null || localData.Count == 0) return;

        //重置所有的物品的isequip
        foreach(var item in localData) item.isEquip = false;

        //获取当前所有已装备物品，匹配背包中的物品，并更新isequip
        Dictionary<EquipmentType,PackageLocalTableData> equippedItems = equipmentManager.GetAllEquippedItem();
        foreach(var item in equippedItems)
        {

            PackageLocalTableData equippedItem = item.Value;
            //通过uid查找物品
            PackageLocalTableData targetItem = localData.Find(item => item.itemUID == equippedItem.itemUID);
            if(targetItem != null)
            {

                targetItem.isEquip = equippedItem.isEquip;

                Debug.Log($"同步装备状态：UID={equippedItem.itemUID} 为 {equippedItem.isEquip}");
            }
        }

        //保存总背包数据
        try
        {
            //更新背包缓存
            await PackageLocalTable.Instance.SavePackageData();
            PackageDataManage.Instance.UpdateCachedPackageData();
            Debug.Log("关闭背包：已同步装备状态并保存总背包数据");
        }
        catch(Exception e)
        {
            Debug.LogError($"同步装备状态失败：{e.Message}");
        }

    }

#region 按钮事件
/// <summary>
/// 进入删除模式
/// </summary>
    private void OnClickDelect()
    {
        CloseDetailPanel();
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
        CloseDetailPanel();
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseClick);
        currentPanelMode = PanelMode.normal;
        RefreshDelectState();

        DelectUID.Clear();
        Debug.Log($"清空了删除uid列表，列表中有 {DelectUID.Count}个物品");
        
        UIDelectPanel.gameObject.SetActive(false);
        UIDelectButton.gameObject.SetActive(true);
        
    }
/// <summary>
/// 确认删除
/// </summary>
    private void OnClickConfirmDelect()
    {
        CloseDetailPanel();
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseClick);
        if(DelectUID == null || DelectUID.Count == 0) return;

        PackageDataManage.Instance.DelectItems(DelectUID);

        RefreshScrollView();

        DelectUID.Clear(); //清空选中列表
        RefreshDelectState(); //清空格子选中标识

    }
    /// <summary>
    /// 下一页
    /// </summary>
    private void OnClickRight()
    {
        CloseDetailPanel();
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
        CloseDetailPanel();
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseClick);
        if(typeSwitch.Length == 0) return;

        currentTypeIndex = (currentTypeIndex -1 + typeSwitch.Length)%typeSwitch.Length;
        ItemType itemType = typeSwitch[currentTypeIndex];

        OnClickButtonByType(itemType);
    }
/// <summary>
/// 关闭页面
/// </summary>
    private async Task OnClickClose()
    {
        CloseDetailPanel();
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseClick);
        UIManager.Instance.CloseCurrentActivePanel();
        CloseDetailPanel();
        // 关闭背包更新物品的isequip
        await SyncEquipStateToLocalTables();
    }
/// <summary>
/// 桥接OnClickClose，button仅支持void类型
/// </summary>
/// <returns></returns>
    private async void OnClickCloseWrapper()
    {
        try
        {
            await OnClickClose();
        }
        catch (Exception e)
        {
            Debug.LogError($"关闭背包时发生错误：{e.Message}");
        }
    }
     public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right ) //右键关闭详情面板
        {
            CloseDetailPanel();
        }
    }
    #endregion
/// <summary>
/// 打开物品详情界面
/// </summary>
    public void OpenDetailPanel()
    {
        if(string.IsNullOrEmpty(_chooseUID)) return; 

        uiItemDetailsPanel.SetActive(true);
        
        PackageLocalTableData localTable = PackageDataManage.Instance.GetPackageItem_ByUID(_chooseUID);
        if(localTable == null) return;

        if(!uiItemDetailsPanel.TryGetComponent<ItemDetailsPanal>(out var itemDetails)) return;
        itemDetails.Refresh(localTable,this);

        Vector2 mousePos = Mouse.current.position.ReadValue();

        itemDetails.SetPanelPos(panelCanvas.transform as RectTransform ,mousePos);
     
    }
/// <summary>
/// 关闭物品详情界面
/// </summary>
    public void CloseDetailPanel() => uiItemDetailsPanel.SetActive(false);

#region 装备界面相关
/// <summary>
/// 根据uid查找对应物品格子
/// </summary>
/// <param name="uid"></param>
/// <returns></returns>
    public PackageCell FindCellBy_UID(string uid)
    {
        if(string.IsNullOrEmpty(uid) || !uidToCell.ContainsKey(uid))
        {
            return null;
        }

        return uidToCell[uid];
    }

    /// <summary>
    /// 装备管理器修改isequip之后，再回来修改对应格子的isequip
    /// </summary>
    /// <param name="type"></param>
    /// <param name="item"></param>
    private void OnEquipmentStateChanged(EquipmentType type, PackageLocalTableData item)
    {
        if(item == null || string.IsNullOrEmpty(item.itemUID)) return;

        //根据uid查找对应格子
        PackageCell targetCell = FindCellBy_UID(item.itemUID);
        if(targetCell != null)
        {
            /* //只刷新已装备图标
            targetCell.RefreshEquipState(); */

            //更新已装备图标
            targetCell.RefreshEquipState();
            Debug.Log($"已刷新UID为{item.itemUID}的格子装备状态");
        }
    }
/// <summary>
/// 装备物品，将物品传给装备管理器
/// </summary>
/// <returns></returns>
    public bool EquipItem()
    {
        string itemUID = _chooseUID;
        if(string.IsNullOrEmpty(itemUID))
        {
            Debug.LogWarning("装备失败：物品UID为空");
            return false;
        }

        if(equipmentManager == null)
        {
            Debug.LogError("装备失败：未找到PlayerEquipmentManager");
            return false;
        }

        PackageLocalTableData targetItem = PackageDataManage.Instance.GetPackageItem_ByUID(itemUID);
        equipmentManager.Equip(targetItem);
        return true;

    }
#endregion
   
}
