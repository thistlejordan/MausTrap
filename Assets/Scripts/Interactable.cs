using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;

public class Interactable : MonoBehaviour
{

    public Player m_InteractingPlayer = null;

    //Item and Randomize Contents values will not be used if the Contains Item field is false
    public bool m_ContainsItem = false;
    public Item m_Item = null;
    public bool m_RandomizeContents = false;

    //Dialog value will not be used if Contains Dialog is false
    public bool m_ContainsDialog = false;
    public string m_Dialog = null;

    private Sprite m_DefaultSprite;
    public Sprite m_AlternateSprite = null;

    public void Awake() {

        m_DefaultSprite = GetComponent<SpriteRenderer>().sprite;
    }

    //Any interactable can contain an item (NPC, chest, rock, etc.)
    public void InitializeItemContents() {
       
        if(GetComponentInChildren<Item>()) {

            if(m_RandomizeContents) { GenerateRandomContents(); }

            m_Item = GetComponentInChildren<Item>();
            m_Item.transform.SetParent(transform);
            m_ContainsItem = true;

        } else {

            if(m_RandomizeContents && m_ContainsItem) {

                GenerateRandomContents();
                m_Item.transform.SetParent(transform);
                m_ContainsItem = true;
                
            } else {

                m_ContainsItem = false;
            }
        }
    }

    public void GenerateRandomContents() {

        //Remove any already-existing items
        for(int i = 0; i < GetComponentsInChildren<Item>().Length; i++) {
            Destroy(GetComponentsInChildren<Item>()[i].gameObject);
        }

        //Create a random item
        Item _RandomItem = ItemManager.Instance.RandomItem();
        m_Item = Instantiate(_RandomItem, transform);
        m_Item.name = _RandomItem.name;
    }

    public virtual void OnInteract(Player player) {

        m_InteractingPlayer = player;

        if(m_ContainsDialog) { m_InteractingPlayer.m_DialogBox.UpdateDialog(m_Dialog); }
    }

    public void AddItem() {
                
        if(!m_InteractingPlayer.m_Inventory.CheckForDuplicate(m_Item) || m_Item.m_Consumable) {
            if(GameManager.Instance.DebugMode) { Debug.Log("No Duplicate Found for: " + m_Item.name); }
            m_InteractingPlayer.m_Inventory.AddContents(m_Item);
            m_Item.m_Owner = m_InteractingPlayer;
            m_ContainsItem = false;
            AudioManager.Instance.m_AudioSource.PlayOneShot(AudioManager.Instance.m_ItemGetSoundEffect, 0.5f);

        } else {
            if(GameManager.Instance.DebugMode) { Debug.Log("Duplicate Found for: " + m_Item.name); }
            m_InteractingPlayer.m_DialogBox.UpdateDialog("You already have a " + m_Item.name + "!\nCannot carry more.");
            StartCoroutine(ReplaceDefaultSprite());
            // if(GetComponent<Pickup>()) { GetComponent<Pickup>().m_CanInteract = false; }
        }

        ItemGetAnimation();
    }

    public virtual void ItemGetAnimation() {

        m_InteractingPlayer.m_AnimationHelper.UpdateAnimation(m_Item.GetComponent<Animator>());        
    }

    public void UseAlternateSprite() {

        GetComponent<SpriteRenderer>().sprite = m_AlternateSprite;
    }

    public IEnumerator ReplaceDefaultSprite() {

        while(!m_InteractingPlayer.m_Acknowledge) { yield return null; }
        GetComponent<SpriteRenderer>().sprite = m_DefaultSprite;
    }
}