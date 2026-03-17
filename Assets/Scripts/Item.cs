using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemEffect {
    NONE,
    MELEE,
    AOE,
    PROJECTILE,
    MAX_ITEMEFFECT
}

[Obsolete("Use ItemComponent instead.")]
public class Item : MonoBehaviour {

    public Item() { }
    public Item(int quantity = 1) { m_Quantity = quantity; }

    public int m_Quantity = 1;
    public bool m_Consumable = false;
    public Character m_Owner;

    public Animator Animator { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }

    public void UseItem() {

        if(m_Consumable) {

            if(m_Quantity <= 0) {

                //Abort function if we are out of the consumable item
                if(GameManager.Instance.DebugMode) { Debug.Log("Out of " + name + "(s)"); }
                return;

            } else {

                //Instantiate consumable item
                Item _item_instance = Instantiate(this);

                //Unparent item from Game Object holding the item
                //_item_instance.transform.parent = null;

                //Set Position in front of character
                transform.position = (m_Owner.m_Rigidbody.position + m_Owner.m_CharacterFacingVector * 0.5f);

                //Use the consumable
                _item_instance.Use();

                //Subtract one from quantity
                m_Quantity--;
                m_Owner.GetComponent<Player>().UpdateEquippedItemQuantityText();
            }
        } else {
            Use();
        }
    }

    //Fields that will always belong to Item

    public ItemEffect m_ItemEffect;
    public float m_Duration; // Use 0 for permenants, like arrows
    public float _range; //(Also used for radius) Use -1.0f if there is no range limit (ie: bow)
    public int _damage;
    public float m_MoveSpeed; //For Projectiles

    void Awake() {

        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        HideSprite();
        // if(gameObject.GetComponent<Pickup>()) { ShowSprite(); }
    }

    public virtual void Use() => throw new NotImplementedException();

    public virtual void Use(Character character) { }

    public void HideSprite() {
        SpriteRenderer.enabled = false;
    }

    public void ShowSprite() {
        SpriteRenderer.enabled = true;
    }

    void ProcessProjectile() {

        //Play Animation - Move to GameManager or Player (Not yet implemented)
        //PlayerAnimator(AnimationType.USE_ITEM);

        //Create instance of Projectile (Test code to begin work on projectiles)
        GameObject m_NewProjectile = Instantiate(gameObject);
        GameManager.Instance.m_ActiveProjectiles.Add(m_NewProjectile);
    }

    public void ItemGetAnimation()
    {
        StartCoroutine(IItemGetAnimation());
    }

    public IEnumerator IItemGetAnimation()
    {
        Animator.SetInteger("state", 1);
        ShowSprite();
        yield return new WaitForSeconds(1.5f);
        HideSprite();
    }
}