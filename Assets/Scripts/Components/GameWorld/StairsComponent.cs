using Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Extensions;

namespace Assets.Scripts.Components.GameWorld
{
    public class StairsComponent : MonoBehaviour
    {
        [SerializeField] private List<DirectionEnum> directions;
        private DirectionEnum? matchingDirection;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var playerCharacter = collider.GetComponent<PlayerCharacterComponent>();
            if (playerCharacter is null) return;

            matchingDirection = null;
            foreach (var direction in directions)
            {
                if (matchingDirection.HasValue) break;
                if (playerCharacter.Inputs.Contains(direction)) matchingDirection = direction;
            }

            if (matchingDirection.HasValue)
            {
                playerCharacter.SetControlLoss(true);
            }
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            var playerCharacter = collider.GetComponent<PlayerCharacterComponent>();
            if (playerCharacter is null) return;

            if (matchingDirection.HasValue)
            {
                playerCharacter.UpdateFacing(matchingDirection.Value);
                playerCharacter.Move(matchingDirection.Value.ToVector3(), 0.35f);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            var playerCharacter = collider.GetComponent<PlayerCharacterComponent>();
            if (playerCharacter is null) return;

            playerCharacter.SetControlLoss(false);
            matchingDirection = null;
        }
    }
}
