using UnityEngine;

namespace Assets.Scripts.Components.GameWorld
{
    public class GameAreaTransitionArea : MonoBehaviour
    {
        // Set this field in Unity Editor!
        [SerializeField] private GameAreaTransitionArea exitTransitionArea;

        private bool isArriving;
        private DoorComponent door;

        private void Awake()
        {
            this.door = this.GetComponentInParent<DoorComponent>();
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var playerCharacter = otherCollider.GetComponent<PlayerCharacterComponent>();

            if (playerCharacter is null) return;

            if (this.door is null)
            {
                Debug.LogError($"DoorComponent missing in parent of {this.name}");
            }

            // PlayerCharacter is arrive. Don't send them back immediately.
            if (this.isArriving)
            {
                this.isArriving = false;
                return;
            }

            this.door.TeleportCharacter(playerCharacter);
        }

        public void IsArriving() => this.isArriving = true;
    }
}
