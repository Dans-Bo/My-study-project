using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class LootCellPanel : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI UINumText;
    [SerializeField] private Transform UIItemIcon;
    //[SerializeField] private GameObject UIConfirmPanelPrefab;
     //private RectTransform prefabTrans;
    private Canvas UICanvas;
    private Image icon;
    [SerializeField] private Animator anim;

    private PackageLocalTableData localData;
    private PackageTableData tableData;
    private LootBoxPanel UIParent;

    void Awake()
    {
        anim.gameObject.SetActive(false);
        icon = UIItemIcon.GetComponent<Image>();

        if(UICanvas == null) UICanvas = GetComponentInParent<Canvas>();
    }

    public void Refresh(PackageLocalTableData localTableData, LootBoxPanel lootBoxPanel)
    {
        localData = localTableData;
        UIParent = lootBoxPanel;
        tableData = LootsManager.Instance.GetTableDataByID(localData.itemID);

        if(tableData == null)
        {
            Debug.LogError("获取战利品物品失败");
            return;
        } 
        
        icon.sprite = tableData.itemIcon;
        UINumText.text = localData.itemCount.ToString();
        UINumText.gameObject.SetActive(localData.itemCount >1);

        gameObject.SetActive(true);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseSlide);

            UIParent.OpenConfirmPanel();
            UIParent.ChooseUid = localData.itemUID;
            return;
        }

        if(eventData.button == PointerEventData.InputButton.Right)
        {
            UIParent.CloseConfirmPanel();
            return;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.gameObject.SetActive(true);
        anim.SetTrigger("in");
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_MouseSlide);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.gameObject.SetActive(true);
        anim.SetTrigger("out");
        Invoke("CloseAnimObj",0.3f);
    }

    private void CloseAnimObj()
    {
        if(anim != null) anim.gameObject.SetActive(false);
    }
}
