using Assets.Scripts.Components.Items;
using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(CharacterComponent))]
    public class NpcComponent : InteractableComponent
    {
        [SerializeField] private ItemComponent _requiredFetchItem;

        public override void OnInteract(PlayerComponent player, PlayerCharacterComponent playerCharacter)
        {
            this.item = GetComponentInChildren<ItemComponent>();
            StartCoroutine(IInteract(player, playerCharacter));
        }

        private IEnumerator IInteract(PlayerComponent player, PlayerCharacterComponent playerCharacter)
        {
            if (_dialog != string.Empty)
            {
                player.AwaitDialog(_dialog, DialogAwaitType.Acknowledge, InputType.Character);
                yield return player.DialogCoroutine;
            }

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
            }
        }
    }
}