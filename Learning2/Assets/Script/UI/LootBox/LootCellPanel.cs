using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Animation))]
public class LootCellPanel : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private Text UINumText;
    [SerializeField] private Transform UIItemIcon;
    //[SerializeField] private GameObject UIConfirmPanelPrefab;
     //private RectTransform prefabTrans;
    [SerializeField] private Canvas UICanvas;
    private Image icon;
    private Animation anim;

    private PackageLocalTableData localData;
    private PackageTableData tableData;
    private LootBoxPanel UIParent;
    private bool isOpenConfirmPanel = false;

    void Awake()
    {
        anim = GetComponent<Animation>();
        icon = UIItemIcon.GetComponent<Image>();

        if(UICanvas == null) UICanvas = GetComponentInParent<Canvas>();
    }

    public void Refresh(PackageLocalTableData localTableData, LootBoxPanel lootBoxPanel)
    {
        localData = localTableData;
        UIParent = lootBoxPanel;
        tableData = LootsManager.Instances.GetTableDataByID(localData.itemID);

        if(tableData == null) Debug.LogError("获取战利品物品失败");
        
        icon.sprite = tableData.itemIcon;
        UINumText.text = localData.itemCount.ToString();
        UINumText.gameObject.SetActive(localData.itemCount >1);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LootsConfirmPanel confirmPanel = LootsConfirmPanel.Instance;
        if(confirmPanel == null) return;

        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(isOpenConfirmPanel)
            {
                isOpenConfirmPanel = false;
                LootsConfirmPanel.Instance?.gameObject.SetActive(false);
                Debug.Log("左键关闭确认面板");
                return;
            }
            
            isOpenConfirmPanel = true;
            LootsConfirmPanel.Instance?.gameObject.SetActive(true);
            Vector2 mousePos = Mouse.current.position.ReadValue();
            confirmPanel.SetPanelPos(UICanvas.transform as RectTransform,mousePos);
            
        }

        if(eventData.button == PointerEventData.InputButton.Right && isOpenConfirmPanel)
        {
            LootsConfirmPanel.Instance?.gameObject.SetActive(false);
        }
    }

}
