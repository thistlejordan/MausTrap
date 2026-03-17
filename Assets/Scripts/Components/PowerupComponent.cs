using Assets.Scripts.Components.Items;
using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class PowerupComponent : MonoBehaviour
    {
        [SerializeField] private PowerupType type;
        private ItemComponent item;

        private void Awake()
        {
            this.item = GetComponentInChildren<ItemComponent>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var playerCharacter = collision.GetComponent<PlayerCharacterComponent>();

            if (playerCharacter != null)
            {
                PickUp(playerCharacter);
            }
        }

        private void PickUp(PlayerCharacterComponent playerCharacter)
        {
            switch (this.type)
            {
                case PowerupType.HEART: AddHeart(playerCharacter); break;
                case PowerupType.CASH_1: AddCash(playerCharacter, 1); break;
                case PowerupType.CASH_5: AddCash(playerCharacter, 5); break;
                case PowerupType.CASH_20: AddCash(playerCharacter, 20); break;
                case PowerupType.ITEM: PickUpItem(playerCharacter); break;
                default: Debug.Log("PickUpType does not exist: AssetsScriptsPickup.cs"); break;
            }
        }

        private void PickUpItem(PlayerCharacterComponent playerCharacter) => StartCoroutine(IPickUpItem(playerCharacter.Player, playerCharacter));

        private IEnumerator IPickUpItem(PlayerComponent player, PlayerCharacterComponent playerCharacter)
        {
            if (this.item != null)
            {
                var key = this.item.GetComponent<KeyComponent>();
                if (!(key is null))
                {
                    yield return playerCharacter.KeyChain.IAddItem(this.item, player, (ItemComponent item) => { this.item = item; });
                }
                else
                {
                    yield return playerCharacter.Inventory.IAddItem(this.item, player, (ItemComponent item) => { this.item = item; });
                }

                if (this.item != null)
                {
                    player.AwaitDialog($"You already have a { this.item.name }!\nCannot carry more.", DialogAwaitType.Acknowledge, InputType.Character);
                    yield return player.DialogCoroutine;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogError("No ItemComponent attached to PowerUp, but PowerUp Type is set to 'Item'", this);
            }
        }

        private void AddHeart(PlayerCharacterComponent playerCharacter)
        {
            playerCharacter.RestoreHealth(1);
            Destroy(transform.parent.gameObject);
        }

        private void AddCash(PlayerCharacterComponent playerCharacter, int amount)
        {
            playerCharacter.AddMoney(amount);
            Destroy(transform.parent.gameObject);
        }
    }
}
