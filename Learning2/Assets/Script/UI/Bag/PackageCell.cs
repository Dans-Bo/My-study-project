using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class PackageCell: MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Transform UIEquippedIcon; //已装备
    [SerializeField] private Transform UIItemIcon; // 物品图标
                     private Image UIIcon; //图标
    [SerializeField] private Transform UIItemNumText; //物品数量
    [SerializeField] private Transform UISelect; //删除时，选中标识
    [SerializeField] private Transform animator;

    private PackageTableData tableData;
    private PackageLocalTableData localData;
    private PackagePanel UIParent;

    void Awake()
    {
        InitUI();
    }

    private void InitUI()
    {
        UIIcon = UIItemIcon.GetComponent<Image>();
        UISelect.gameObject.SetActive(false);
        UIEquippedIcon.gameObject.SetActive(false);
        animator.gameObject.SetActive(false);
    }

    public void Refresh(PackageLocalTableData localTableData, PackagePanel packagePanel)
    {
        localData = localTableData;
        UIParent = packagePanel;
        tableData = PackageDataManage.Instance.GetPackageItem_ByID(localData.itemID);

        if(tableData == null)
        {
            #if UNITY_EDITOR
            Debug.Log($"获取该id物品失败");
            #endif
        }
        UIIcon.sprite = tableData.itemIcon;
        
        Text numText = UIItemNumText.GetComponent<Text>();
        numText.text = localData.itemCount.ToString();
        UIItemNumText.gameObject.SetActive(localData.itemCount >1); 
    }
/// <summary>
/// 刷新选中状态
/// </summary>
    public void RefreshDelectState()
    {
        if (UIParent.delectUID.Contains(localData.itemUID))
            {
                UISelect.gameObject.SetActive(false);
            }
            //else UISelect.gameObject.SetActive(false);
    }

#region 鼠标事件
    public void OnPointerClick(PointerEventData eventData)
    {
        if(UIParent.currentPanelMode == PanelMode.delect)
        {
            UIParent.AddDelectUID(localData.itemUID);
            if(UISelect.gameObject.activeSelf)
            {
                UISelect.gameObject.SetActive(false);
            }else UISelect.gameObject.SetActive(true);
        }

        /* if(UIParent.ChooseUID == localData.itemUID) return;
        UIParent.ChooseUID = localData.itemUID; */
        #if UNITY_EDITOR
        Debug.Log($"选中该物品");
        #endif
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.gameObject.SetActive(true);
        animator.GetComponent<Animator>().SetTrigger("in");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.gameObject.SetActive(true);
        animator.GetComponent<Animator>().SetTrigger("out");
    }

    #endregion
}
