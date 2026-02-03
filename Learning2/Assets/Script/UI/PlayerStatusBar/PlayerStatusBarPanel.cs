using UnityEngine.UI;
using UnityEngine;

public class PlayerStatusBarPanel : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBar2;
    [SerializeField] private Image manathBar;
    [SerializeField] private Image manahBar2;

    private float currentMaxHp;
    private float currentMaxMp;
    private float currentMaxExp;

    [SerializeField] private Image expBar;
    [SerializeField] private PlayerAttributeManager AttrManager;

    


    void Awake()
    {
        if(AttrManager == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            AttrManager = player.GetComponent<PlayerAttributeManager>();
        }

        if(AttrManager == null)
        {
            #if UNITY_EDITOR
            Debug.LogError($"找不到PlayerAttributeManager组件");
            #endif
        }    
    }

    void Start()
    {
        currentMaxExp = AttrManager.GetAttribute(PlayerAttribute.MaxExp);
        currentMaxHp = AttrManager.GetAttribute(PlayerAttribute.MaxHP);
        currentMaxMp = AttrManager.GetAttribute(PlayerAttribute.MaxMP);
    }

    void OnEnable()
    {
        AttrManager.OnAttributeChange += OnChange;
    }

    void OnDisable()
    {
        AttrManager.OnAttributeChange -= OnChange;
    }

    void Update()
    {
        if(manahBar2.fillAmount > manathBar.fillAmount)
        {
            manahBar2.fillAmount -= Time.deltaTime * 0.5f;
        }

        if(healthBar2.fillAmount > healthBar.fillAmount)
        {
            healthBar2.fillAmount -= Time.deltaTime * 0.5f;
        }
    }

    private void OnChange(PlayerAttribute attribute, int newValue)
    {
        
        switch(attribute)
        {
            case PlayerAttribute.MaxHP:
                currentMaxHp = newValue;
                break;
            case PlayerAttribute.MaxMP:
                currentMaxMp = newValue;
                break;
            case PlayerAttribute.MaxExp:
                currentMaxExp = newValue;
                break;
            case PlayerAttribute.HP:
                healthBar.fillAmount = Mathf.Clamp01(newValue/currentMaxHp);
                break;
            case PlayerAttribute.MP:
                manathBar.fillAmount = Mathf.Clamp01(newValue / currentMaxMp);
                break;
            case PlayerAttribute.Exp:
                expBar.fillAmount = Mathf.Clamp01(newValue/currentMaxExp);
                break;     
        }
    }


}
