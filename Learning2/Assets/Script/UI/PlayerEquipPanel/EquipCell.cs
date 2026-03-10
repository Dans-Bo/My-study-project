using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipCell : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private Image uiEquipIcon;
    [SerializeField] private RectTransform uiIsEquipped;
    [SerializeField] private RectTransform animator;
    private PackageLocalTableData equipLocalData;
    private PackageTableData equipTableData;
    private EquipmentPanel uiParent;

    void Awake()
    {
        InitUI();
    }

    private void InitUI()
    {
        animator.gameObject.SetActive(false);
        uiIsEquipped.gameObject.SetActive(false);
    }
    public void Refresh(PackageLocalTableData equip, EquipmentPanel equipmentPanel)
    {
        equipLocalData = equip;
        uiParent = equipmentPanel;
        if(uiParent == null) Debug.Log("uiParent为空");
        
        equipTableData = PackageDataManage.Instance.GetPackageTableData_ByID(equip.itemID);

        uiIsEquipped.gameObject.SetActive(equip.isEquip);
        uiEquipIcon.sprite = equipTableData.itemIcon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left && uiParent != null)
        {
            uiParent.OpenDetailPanel(equipLocalData);
        }
        
    }
}
