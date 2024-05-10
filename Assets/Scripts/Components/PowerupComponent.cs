using Assets.Scripts.Components.Items;
using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class PowerupComponent : MonoBehaviour
    {
        [SerializeField] private PowerupType _type;
        private ItemComponent _item;

        private void Awake()
        {
            _item = GetComponentInChildren<ItemComponent>();
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
            switch (_type)
            {
                case PowerupType.HEART: AddHeart(playerCharacter); break;
                case PowerupType.CASH_1: AddCash(playerCharacter, 1); break;
                case PowerupType.CASH_5: AddCash(playerCharacter, 5); break;
                case PowerupType.CASH_20: AddCash(playerCharacter, 20); break;
                case PowerupType.ITEM: PickUpItem(playerCharacter); break;
                default: Debug.Log("PickUpType does not exist: AssetsScriptsPickup.cs"); break;
            }
        }

        private void PickUpItem(PlayerCharacterComponent playerCharacter) => StartCoroutine(IPickUpItem(playerCharacter.Player, playerCharacter.Inventory));

        private IEnumerator IPickUpItem(PlayerComponent player, InventoryComponent inventory)
        {
            if (_item != null)
            {
                yield return inventory.IAddContents(_item, player, (ItemComponent item) => { _item = item; });

                if (_item != null)
                {
                    player.AwaitDialog($"You already have a { _item.name }!\nCannot carry more.", DialogAwaitType.Acknowledge, InputType.Character);
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
