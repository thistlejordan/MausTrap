using UnityEngine;

namespace Assets.Scripts.Components.GameWorld
{
    public class DoorComponent : MonoBehaviour
    {
        [SerializeField] private bool isOpen;
        [SerializeField] private LockComponent @lock;

        public bool IsOpen { get => this.isOpen; }

        public void Open()
        {
            AnimateDoorOpen();
            this.GetComponent<Collider2D>().isTrigger = true;
            this.isOpen = true;
        }

        public void Close()
        {
            AnimateDoorClose();
            this.GetComponent<Collider2D>().isTrigger = false;
            this.isOpen = false;
        }

        private void AnimateDoorOpen()
        {
            // TODO: Replace with Coroutine
            this.GetComponent<SpriteRenderer>().enabled = false;
        }

        private void AnimateDoorClose()
        {
            // TODO: Replace with Coroutine
            this.GetComponent<SpriteRenderer>().enabled = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var playerCharacter = collision.collider.GetComponent<PlayerCharacterComponent>();

            if (playerCharacter is null) return;

            if (playerCharacter.KeyChain.HasKey(this.@lock) && this.@lock.IsLocked)
            {
                this.@lock.Unlock(playerCharacter.KeyChain);
                Open();
            }
        }
    }
}
