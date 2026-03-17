using UnityEngine;

namespace Assets.Scripts.Components.GameWorld
{
    public class GameAreaTransitionArea : MonoBehaviour
    {
        private bool isArriving;
        private DoorComponent door;

        private void Awake()
        {
            this.door = this.GetComponentInParent<DoorComponent>();
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var playerCharacter = otherCollider.GetComponent<PlayerCharacterComponent>();

            if (playerCharacter == null)
            {
                return;
            }

            if (this.door is null)
            {
                Debug.LogError($"DoorComponent missing in parent of {this.name}");
            }

            //// PlayerCharacter is arriving. Don't send them back immediately.
            //if (this.isArriving)
            //{
            //    this.isArriving = false;
            //    return;
            //}

            this.door.TransitionCharacter(playerCharacter);
        }

        public void IsArriving() => this.isArriving = true;
    }
}
