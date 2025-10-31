using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject inventoryMenu;

    private void Start()
    {
        inventoryMenu.gameObject.SetActive(false);
    }
    private void Update()
    {
        InventoryControl();
    }
    
    private void InventoryControl()
    {
        
    }
}
