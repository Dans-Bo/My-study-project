using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsPanal : MonoBehaviour
{
    [Header("Top")]
    [SerializeField] private TextMeshProUGUI uiItemName;
    [Header("Center")]
    [SerializeField] private Image uiItemIcon;
    //[SerializeField] private TextMeshProUGUI uiAffx_1;
    [SerializeField] private TextMeshProUGUI uiAttack_Num;
    //[SerializeField] private TextMeshProUGUI uiAffx_2;
    [SerializeField] private TextMeshProUGUI uiDefence_Num;
    //[SerializeField] private TextMeshProUGUI uiAffx_3;
    [SerializeField] private TextMeshProUGUI uiMaxHp_Num;
    //[SerializeField] private TextMeshProUGUI uiAffx_4;
    [SerializeField] private TextMeshProUGUI uiMaxMp_Num;
    [SerializeField] private TextMeshProUGUI uiLevelNum;
    [SerializeField] private TextMeshProUGUI uiItemDetailText;
    [Header("Bottom")]
    [SerializeField] private Button uiEquiButton;
    [SerializeField] private TextMeshProUGUI uiEquiButtonText;
    [SerializeField] private Button uiDeleteButton;

    private PackageTableData tableData;
    private PackageLocalTableData localData;
    private PackagePanel UIParent;
    private EquipmentPanel equipmentParent;



    void Awake()
    {
        InitUIClick();
    }

    public void Refresh(PackageLocalTableData localTableData, PackagePanel packagePanel)
    {
        tableData = PackageDataManage.Instance.GetPackageItem_ByID(localTableData.itemID);
        localData = localTableData;
        UIParent = packagePanel;

        uiItemName.text = tableData.itemName;
        uiItemIcon.sprite = tableData.itemIcon;
        uiAttack_Num.text = localData.addAttackPower.ToString();
        uiDefence_Num.text = localData.addDefense.ToString();
        uiMaxHp_Num.text = localData.addMaxHP.ToString();
        uiMaxMp_Num.text = localData.addMaxMP.ToString();
        uiLevelNum.text = localData.itemLevel.ToString();
        uiItemDetailText.text = tableData.itemDetails;

        //非装备类隐藏属性UI
        bool isEquipment = tableData.itemType == ItemType.equipment;
        uiAttack_Num.transform.parent.gameObject.SetActive(isEquipment);
        uiDefence_Num.transform.parent.gameObject.SetActive(isEquipment);
        uiMaxHp_Num.transform.parent.gameObject.SetActive(isEquipment);
        uiMaxMp_Num.transform.parent.gameObject.SetActive(isEquipment);
        uiLevelNum.transform.parent.gameObject.SetActive(isEquipment);

        //TODO 分离使用和装备按钮
        if(tableData.itemType != ItemType.equipment)
        {
            uiEquiButtonText.text = "使用";
        }
        else
        {
            uiEquiButtonText.text = "装备";
        }
    }
    public void Refresh(PackageLocalTableData localTableData, EquipmentPanel equipmentPanel)
    {
        tableData = PackageDataManage.Instance.GetPackageItem_ByID(localTableData.itemID);
        localData = localTableData;
        equipmentParent = equipmentPanel;

        uiItemName.text = tableData.itemName;
        uiItemIcon.sprite = tableData.itemIcon;
        uiAttack_Num.text = localData.addAttackPower.ToString();
        uiDefence_Num.text = localData.addDefense.ToString();
        uiMaxHp_Num.text = localData.addMaxHP.ToString();
        uiMaxMp_Num.text = localData.addMaxMP.ToString();
        uiLevelNum.text = localData.itemLevel.ToString();
        uiItemDetailText.text = tableData.itemDetails;

        //非装备类隐藏属性UI
        bool isEquipment = tableData.itemType == ItemType.equipment;
        uiAttack_Num.transform.parent.gameObject.SetActive(isEquipment);
        uiDefence_Num.transform.parent.gameObject.SetActive(isEquipment);
        uiMaxHp_Num.transform.parent.gameObject.SetActive(isEquipment);
        uiMaxMp_Num.transform.parent.gameObject.SetActive(isEquipment);
        uiLevelNum.transform.parent.gameObject.SetActive(isEquipment);

        uiEquiButtonText.text = "卸载";
    }

/// <summary>
/// 设置面板位置
/// </summary>
/// <param name="canvasRect"></param>
/// <param name="mouseScreenPos"></param>
    public void SetPanelPos(RectTransform canvasRect, Vector2 mouseScreenPos)
    {
        Camera canvasCamera = null;
        if (canvasRect.GetComponent<Canvas>().renderMode != RenderMode.ScreenSpaceOverlay)
        {
            canvasCamera = canvasRect.GetComponent<Canvas>().worldCamera;
        }
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mouseScreenPos,
            canvasCamera,
            out Vector2 localPoint
        ))
        {
            RectTransform selfRect = transform as RectTransform;
            if(selfRect == null) return;
            
            //计算UI左边界对其鼠标后的原始位置
            float uiWidth = selfRect.rect.width;
            float pivotX = selfRect.pivot.x;
            float offsetX = uiWidth * pivotX;
            localPoint.x -= offsetX;

            //计算Canvas的可视边界
             Vector3[] canvasCorners = new Vector3[4];
            canvasRect.GetLocalCorners(canvasCorners);
            float canvasLeft = canvasCorners[0].x;    // Canvas左边界（本地坐标）
            float canvasRight = canvasCorners[2].x;   // Canvas右边界（本地坐标）
            float canvasBottom = canvasCorners[0].y;  // Canvas下边界（本地坐标）
            float canvasTop = canvasCorners[2].y;     // Canvas上边界（本地坐标）
            

            float uiLeftEdge = localPoint.x; // UI左边缘的本地坐标（因为已经偏移到左对齐）
            float uiRightEdge = uiLeftEdge + uiWidth; // UI右边缘的本地坐标
            // 限制X轴：左边缘不小于Canvas左，右边缘不大于Canvas右
            float clampedX = Mathf.Clamp(uiLeftEdge, canvasLeft, canvasRight - uiWidth);

            // Y轴：保证UI下边缘 ≥ Canvas下边界，上边缘 ≤ Canvas上边界
            float uiHeight = selfRect.rect.height;
            float pivotY = selfRect.pivot.y;
            // UI下边缘相对于pivot的偏移：pivot在Y轴的占比 * UI高度
            float offsetY = uiHeight * pivotY;
            // 鼠标位置转UI下边缘位置（对齐逻辑和X轴一致）
            float uiBottomEdge = localPoint.y - offsetY;
            float uiTopEdge = uiBottomEdge + uiHeight;
            // 限制Y轴：下边缘不小于Canvas下，上边缘不大于Canvas上
            float clampedY = Mathf.Clamp(uiBottomEdge, canvasBottom, canvasTop - uiHeight);

            // 转换回以pivot为基准的localPosition
            Vector2 finalPos = new Vector2(
                clampedX + offsetX, // 加回X轴偏移，回到pivot基准
                clampedY + offsetY  // 加回Y轴偏移，回到pivot基准
            );
                selfRect.localPosition = finalPos;

        }
    }


    void InitUIClick()
    {
        uiEquiButton.onClick.AddListener(OnEquip);
        uiDeleteButton.onClick.AddListener(OnDelete);
    }

    private void OnDelete()
    {   
        if(UIParent != null)
        {
            PackageDataManage.Instance.DeleteItem_Whole(localData.itemUID);
            UIParent.RefreshScrollView();
            this.gameObject.SetActive(false);
            Debug.Log("丢弃");
        }

        if(equipmentParent != null)
        {
            Debug.Log("卸载");
        }
        
    }
    //TODO 获取item的isequip，显示已装备图标
    private void OnEquip()
    {
        if(UIParent != null)
        {
            UIParent.EquipItem();
            Debug.Log("装备");
        }

        if(equipmentParent != null)
        {
            Debug.Log($"卸载");
        }
    }
}
