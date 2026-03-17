using Assets.Scripts.Managers;
using Assets.Scripts.Old;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

    public DialogBox m_DialogBox;
    public AnimHelper m_AnimationHelper;

    public bool m_Acknowledge = true;

    //
    public GameState m_PlayerState = GameState.OVERWORLD;
    public List<GameState> m_PlayerStateStack;
    //

    public Item m_EquippedItem;
    public Text m_EquippedItemQuantity;
    public Inventory m_Inventory;
    public Weapon m_Weapon = null;

    void Awake() {

        m_DialogBox.m_Player = this;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<Collider2D>();
        m_PlayerStateStack.Add(m_PlayerState);
        //m_EquippedItem = m_Inventory.m_ItemList[0];
        UpdateEquippedItemQuantityText();

        UpdateHealth(m_HP);
        if(m_Inventory == null) { m_Inventory = GetComponentInChildren<Inventory>(); }
    }

    public void UpdateEquippedItemQuantityText() {

        m_EquippedItemQuantity.text = m_EquippedItem.m_Quantity.ToString();
    }

    public override void TakeDamage(int damage) {
        base.TakeDamage(damage);
        UpdateHealth(m_HP);
    }

    public void UpdateHealth(int value) {

        //GameManager.Instance.m_HealthHUD.SetCurrentHealth(value);
    }

    public Interactable ParseInteraction() {

        if(GameManager.Instance.DebugMode) {

            Debug.DrawLine(
                (m_Rigidbody.position + m_Collider.offset) + (m_CharacterFacingVector * .625f) + (new Vector2(m_CharacterFacingVector.y, m_CharacterFacingVector.x) * .375f),
                (m_Rigidbody.position + m_Collider.offset) + (m_CharacterFacingVector * .625f) - (new Vector2(m_CharacterFacingVector.y, m_CharacterFacingVector.x) * .375f),
                Color.green);
        }

        RaycastHit2D hit = Physics2D.Linecast(
            (m_Rigidbody.position + m_Collider.offset) + (m_CharacterFacingVector * .625f) + (new Vector2(m_CharacterFacingVector.y, m_CharacterFacingVector.x) * .375f), 
            (m_Rigidbody.position + m_Collider.offset) + (m_CharacterFacingVector * .625f) - (new Vector2(m_CharacterFacingVector.y, m_CharacterFacingVector.x) * .375f),
            1 << LayerMask.NameToLayer("Object"));

        if(hit.collider != null) {

            if(GameManager.Instance.DebugMode) {
                Debug.Log("Interaction with " + hit.collider.gameObject.name + " was successful!");
            }

            return hit.collider.GetComponent<Interactable>();

        } else {

            return null;
        }
    }
}