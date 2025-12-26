using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Sign : MonoBehaviour
{
    
    [SerializeField] private GameObject signSprite;
    [SerializeField] private Transform playerTransform;
    private PlayerInput playerInput;
    private Animator anim;

    private void Awake()
    {
        anim = signSprite.GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }


    void Update()
    {
        signSprite.transform.localScale = playerTransform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Interactable"))
        {
            signSprite.SetActive(true);
            anim.Play("keyboardF");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        signSprite.SetActive(false);
    }
}
