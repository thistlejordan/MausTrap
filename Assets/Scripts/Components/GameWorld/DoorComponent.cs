using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components.GameWorld
{
    public class DoorComponent : LockableComponent
    {
        [SerializeField] private bool isOpen;
        [SerializeField] private DoorComponent exitDoor;

        private LockComponent @lock;
        private GameAreaTransitionArea transitionArea;
        private GameArea gameArea;

        public bool IsOpen => this.isOpen;

        public DoorComponent ExitDoor => this.exitDoor;

        public LockComponent Lock
        {
            get
            {
                if (this.@lock == null)
                {
                    this.@lock = this.GetComponent<LockComponent>();
                }

                return this.@lock;
            }
        }

        public GameArea GameArea
        {
            get
            {
                if (this.gameArea == null)
                {
                    this.gameArea = this.GetComponentInParent<GameArea>();

                    if (this.ExitDoor == null)
                    {
                        Debug.LogError($"{this.GameArea.name} {this.name} is missing Corresponding exit door DoorComponent.");
                    }
                }

                return this.gameArea;
            }
        }

        private GameAreaTransitionArea TransitionArea
        {
            get
            {
                if (this.transitionArea == null)
                {
                    this.transitionArea = this.GetComponentInChildren<GameAreaTransitionArea>();

                    if (this.transitionArea == null)
                    {
                        Debug.LogError($"{this.GameArea.name} {this.name} is missing a GameAreaTransitionArea component.");
                    }
                }

                return this.transitionArea;
            }
        }

        private void Awake()
        {
        }

        public void Open()
        {
            if (this.isOpen)
            {
                return;
            }

            this.AnimateDoorOpen();
            this.GetComponent<Collider2D>().isTrigger = true;
            this.isOpen = true;
        }

        public void Close()
        {
            if (!this.isOpen) return;
            this.AnimateDoorClose();
            this.GetComponent<Collider2D>().isTrigger = false;
            this.isOpen = false;
        }

        public void Arrive()
        {
            this.TransitionArea.IsArriving();
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
            if (this.Lock == null)
            {
                return;
            }

            var playerCharacter = collision.collider.GetComponent<PlayerCharacterComponent>();

            if (playerCharacter == null)
            {
                return;
            }

            if (playerCharacter.KeyChain.HasKey(this.Lock) && this.Lock.IsLocked)
            {
                this.Lock.Unlock(playerCharacter.KeyChain);
                this.Open();
                this.exitDoor.Open();
            }
        }

        private IEnumerator IExitThroughDoorway(PlayerCharacterComponent playerCharacter)
        {
            this.ExitDoor.Arrive();
            this.StartCoroutine(ITransitionCameraAndCharacter(playerCharacter));
            yield return playerCharacter.IControlLoss(0.5f);
            this.ExitDoor.EnterThroughDoorway(playerCharacter);
        }

        private void EnterThroughDoorway(PlayerCharacterComponent playerCharacter)
        {
            playerCharacter.transform.position = this.transform.position;
        }

        private IEnumerator ITransitionCameraAndCharacter(PlayerCharacterComponent playerCharacter)
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
            director.SetBounds(this.ExitDoor.GameArea.GetComponent<Collider2D>());
            director.FreeCamera = false;
        }

        public void TransitionCharacter(PlayerCharacterComponent playerCharacter) => this.StartCoroutine(IExitThroughDoorway(playerCharacter));

        public override void Unlock()
        {
            this.Open();
        }
    }
}
