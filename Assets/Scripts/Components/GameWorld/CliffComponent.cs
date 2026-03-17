using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Components.GameWorld
{
    public class CliffComponent : MonoBehaviour
    {
        [SerializeField] private DirectionEnum jumpDirection;
        private float time;

        private void OnCollisionStay2D(Collision2D collision)
        {
            var playerCharacter = collision.collider.GetComponent<PlayerCharacterComponent>();

            if (playerCharacter is null) return;

            if (playerCharacter.Inputs.Contains(jumpDirection))
            {
                time += Time.deltaTime;
                if (time >= 0.5f)
                {
                    this.GetComponent<Collider2D>().isTrigger = true;
                    playerCharacter.SetControlLoss(true);
                    time = 0f;
                }
            }
            else
            {
                time = 0f;
            }
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            var playerCharacter = collider.GetComponent<PlayerCharacterComponent>();

            if (playerCharacter is null) return;

            playerCharacter.Move(jumpDirection.ToVector3());
        }

        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            var playerCharacter = otherCollider.GetComponent<PlayerCharacterComponent>();

            if (playerCharacter is null) return;

            Debug.Log($"OnTriggerExit2D: {this.name}");

            playerCharacter.SetControlLoss(false);

            this.GetComponent<Collider2D>().isTrigger = false;
        }
    }
}
