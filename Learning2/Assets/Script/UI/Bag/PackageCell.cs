using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using JetBrains.Annotations;
using System.Collections;

public class PackageCell: MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Transform UIEquippedIcon; //已装备
    [SerializeField] private Transform UIItemIcon; // 物品图标
                     private Image UIIcon; //图标
    [SerializeField] private Transform UIItemNumText; //物品数量
                     public Transform UISelect; //删除时，选中标识
    [SerializeField] private Transform animator;
    
    private PackageTableData tableData;
    private PackageLocalTableData localData;
    public PackageLocalTableData LocalData => localData;
    private PackagePanel UIParent;
    

    void Awake()
    {
        InitUI();
    }

    private void InitUI()
    {
        UIIcon = UIItemIcon.GetComponent<Image>();
        
        animator.gameObject.SetActive(false);
    }
/// <summary>
/// 刷新物品格子
/// </summary>
/// <param name="localTableData"></param>
/// <param name="packagePanel"></param>
    public void Refresh(PackageLocalTableData localTableData, PackagePanel packagePanel)
    {
        localData = localTableData;
        UIParent = packagePanel;
        tableData = PackageDataManage.Instance.GetPackageTableData_ByID(localData.itemID);

        if(tableData == null)
        {
            #if UNITY_EDITOR
            Debug.Log($"获取该id物品失败");
            #endif
            return;
        }
        UIIcon.sprite = tableData.itemIcon;
        UISelect.gameObject.SetActive(false);
        UIEquippedIcon.gameObject.SetActive(localData.isEquip);
        
        Text numText = UIItemNumText.GetComponent<Text>();
        numText.text = localData.itemCount.ToString();
        UIItemNumText.gameObject.SetActive(localData.itemCount >1); 
    }
/// <summary>
/// 刷新已装备图标
/// </summary>
    public void RefreshEquipState()
    {
        if(UIEquippedIcon != null && localData != null)
        {
            UIEquippedIcon.gameObject.SetActive(localData.isEquip);
        }
    }
/// <summary>
/// 刷新选中状态
/// </summary>
    public void RefreshDelectState()
    {
        if (UIParent.DelectUID.Contains(localData.itemUID))
            {
                UISelect.gameObject.SetActive(false);
            }
            //else UISelect.gameObject.SetActive(false);
    }

#region 鼠标事件
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseClick);

        if(eventData.button == PointerEventData.InputButton.Right ) //右键关闭详情面板
        {
            //isOpenItemDetailsPanel = false;
            UIParent.CloseDetailPanel();
            return;
        }

        if(UIParent.currentPanelMode == PanelMode.delect) //删除模式不打开详情面板
        {
            UIParent.AddDelectUID(localData.itemUID);

            UISelect.gameObject.SetActive(!UISelect.gameObject.activeSelf);
            return;
            /* if(UISelect.gameObject.activeSelf)
            {
                UISelect.gameObject.SetActive(false);
            }else UISelect.gameObject.SetActive(true); */
        }

        // if(UIParent.ChooseUID == localData.itemUID) return;
        UIParent.ChooseUID = localData.itemUID; 

        UIParent.OpenDetailPanel(); //打开详情界面
        //isOpenItemDetailsPanel = true;

        #if UNITY_EDITOR
        Debug.Log($"选中该物品");
        #endif
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.gameObject.SetActive(true);
        animator.GetComponent<Animator>().SetTrigger("in");
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseSlide);
        StartCoroutine(DisableAnimatorObj(0.5f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.gameObject.SetActive(true);
        animator.GetComponent<Animator>().SetTrigger("out");
        StartCoroutine(DisableAnimatorObj(0.5f));
    }

    #endregion

    /// <summary>
    /// 协程延迟关闭动画gameobject  
    /// </summary>
    /// <param name="delay">延迟的时间</param>
    /// <returns></returns>
    private IEnumerator DisableAnimatorObj(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.gameObject.SetActive(false);
    }
}
