using Assets.Scripts.Components;
using Assets.Scripts.Components.Items;
using Assets.Scripts.Enums;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using System.Collections;
using UnityEngine;

public class PlayerCharacterComponent : CharacterComponent
{
    [SerializeField] private HeadsUpDisplayComponent HUD;
    [SerializeField] private PlayerComponent player;
    [SerializeField] private ItemComponent equippedItem;
    [SerializeField] private InventoryComponent inventory;
    [SerializeField] private KeyChainComponent keyChain;
    [SerializeField] private WalletComponent wallet;
    [SerializeField] private WeaponComponent weapon;

    public PlayerComponent Player { get => this.player; }
    public InventoryComponent Inventory { get => this.inventory; }
    public KeyChainComponent KeyChain { get => this.keyChain;  }

    void Awake()
    {
        EquipItem(this.inventory.GetDefaultItem());
        this.HUD.HealthBarComponent.UpdateHealth(this.Health);
        this.HUD.MoneyComponent.UpdateMoney(this.wallet.Money);
        this.HUD.KeyCountComponent.UpdateKeyCount(this.keyChain.GetKeyCount(LevelEnum.LEVEL_1));

        // Subscribe to key count changes
        this.keyChain.OnKeyCountChanged += OnKeyCountChanged;
    }

    public void EquipItem(ItemComponent item)
    {
        this.equippedItem = item;
        this.HUD.EquippedItemComponent.UpdateEquippedItem(item);
    }

    public void AddMoney(int value)
    {
        this.wallet.AddMoney(value);
        this.HUD.MoneyComponent.UpdateMoney(this.wallet.Money);
    }

    public void TakeMoney(int value)
    {
        this.wallet.TakeMoney(value);
        this.HUD.MoneyComponent.UpdateMoney(this.wallet.Money);
    }

    public override void RestoreHealth(int value)
    {
        base.RestoreHealth(value);
        this.HUD.HealthBarComponent.UpdateHealth(this.Health);
    }

    public override void ReceiveAttack(AttackModel attack)
    {
        base.ReceiveAttack(attack);
        this.HUD.HealthBarComponent.UpdateHealth(this.Health);
    }

    public void Interact(PlayerComponent player) => ParseInteraction()?.OnInteract(player, this);

    public InteractableComponent ParseInteraction()
    {
        var start = this.Rigidbody.position + this.Collider.offset + (_facing * 1.25f) + (new Vector2(_facing.y, _facing.x) * 0.75f);
        var end = this.Rigidbody.position + this.Collider.offset + (_facing * 1.25f) - (new Vector2(_facing.y, _facing.x) * 0.75f);

        if (GameManager.Instance.DebugMode) { Debug.DrawLine(start, end, Color.green); }
        RaycastHit2D hit = Physics2D.Linecast(start, end, 1 << LayerMask.NameToLayer("Object"));

        return hit.collider?.GetComponent<InteractableComponent>();
    }

    public void Attack()
    {
        this.weapon.Use(this);

        if (!GetComponent<Animator>().GetBool("IsAttacking"))
        {
            StartCoroutine(IAttack());
        }
    }

    public void UseItem() => throw new System.NotImplementedException();

    private void OnKeyCountChanged(LevelEnum level, int count)
    {
        // Update HUD
        if (this.HUD.KeyCountComponent != null)
        {
            this.HUD.KeyCountComponent.UpdateKeyCount(count);
        }

        // Update inventory menu
        if (this.inventory._inventoryMenu != null)
        {
            this.inventory._inventoryMenu.UpdateKeyCountInMenu(level, count);
        }
    }

    private IEnumerator IAttack()
    {
        this.Animator.SetBool("IsAttacking", true);
        var multiplier = moveSpeedMultiplier;
        moveSpeedMultiplier = 0.0f;

        yield return new WaitForSeconds(1f / 3f); // TODO: Replace with anim.ClipLength if possible: will need to figure out how to access the clip from the Animator

        this.Animator.SetBool("IsAttacking", false);
        moveSpeedMultiplier = multiplier;
    }
}