using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components.GameWorld
{
    public class DoorComponent : MonoBehaviour
    {
        [SerializeField] private bool isOpen;
        [SerializeField] private LockComponent @lock;
        private GameArea gameArea;
        private DoorComponent exitDoor;

        public GameArea GameArea { get => this.gameArea; }

        public bool IsOpen { get => this.isOpen; }
        public DoorComponent ExitDoor { get => this.exitDoor; }

        private void Awake()
        {
            this.gameArea = this.GetComponentInParent<GameArea>();

            if (this.ExitDoor is null)
            {
                Debug.LogWarning($"{this.name} is missing Corresponding exit door DoorComponent and will not function correctly.");
            }
        }

        public void Open()
        {
            if (this.isOpen) return;
            AnimateDoorOpen();
            this.GetComponent<Collider2D>().isTrigger = true;
            this.isOpen = true;
        }

        public void Close()
        {
            if (!this.isOpen) return;
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
                this.exitDoor.Open();
            }
        }

        private IEnumerator IMovePlayerCharacter(PlayerCharacterComponent playerCharacter)
        {
            var exitGameAreaTransitionArea = this.ExitDoor.GetComponentInChildren<GameAreaTransitionArea>();

            exitGameAreaTransitionArea.IsArriving();
            StartCoroutine(TransitionCamera(playerCharacter));
            yield return playerCharacter.IControlLoss(0.5f);
            playerCharacter.transform.position = exitGameAreaTransitionArea.transform.position;
        }

        private IEnumerator TransitionCamera(PlayerCharacterComponent playerCharacter)
        {
            var time = 0f;
            var duration = 0.5f;
            var director = playerCharacter.GetComponentInChildren<CameraDirector>();
            var targetPosition = director.GetBoundedCameraPosition(director.GetBounds(this.ExitDoor.GameArea.GetComponent<Collider2D>()));
            var startPosition = director.GetBoundedCameraPosition(director.GetBounds(this.GameArea.GetComponent<Collider2D>()));

            director.FreeCamera = true;
            while (time < duration)
            {
                director.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            director.SetBounds(this.GameArea.GetComponent<Collider2D>());
            director.FreeCamera = false;
        }

        public void TeleportCharacter(PlayerCharacterComponent playerCharacter) => StartCoroutine(IMovePlayerCharacter(playerCharacter));
    }
}
