using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components.GameWorld
{
    public class GameAreaTransitionArea : MonoBehaviour
    {
        // Set this field in Unity Editor!
        [SerializeField] private GameAreaTransitionArea correspondingGameAreaTransitionArea;

        private bool isArriving;

        private void Awake()
        {
            if (this.correspondingGameAreaTransitionArea is null)
            {
                Debug.LogWarning($"{this.name} is missing Corresponding GameAreaTransitionArea and will not function correctly.");
            }
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            var playerCharacter = otherCollider.GetComponent<PlayerCharacterComponent>();

            if (playerCharacter is null) return;

            Debug.Log($"OnTriggerEnter2D: {this.name}");

            // PlayerCharacter is arrive. Don't send them back immediately.
            if (this.isArriving)
            {
                this.isArriving = false;
                return;
            }

            this.correspondingGameAreaTransitionArea.TeleportCharacter(playerCharacter);
        }

        private IEnumerator IMovePlayerCharacter(PlayerCharacterComponent playerCharacter)
        {
            this.isArriving = true;
            StartCoroutine(TransitionCamera(playerCharacter));
            yield return playerCharacter.IControlLoss(0.5f);
            playerCharacter.transform.position = this.transform.position;
        }

        private IEnumerator TransitionCamera(PlayerCharacterComponent playerCharacter)
        {
            var time = 0f;
            var duration = 0.5f;
            var director = playerCharacter.GetComponentInChildren<CameraDirector>();
            var startPosition = director.GetBoundedCameraPosition(director.GetBounds(this.correspondingGameAreaTransitionArea.GetComponentsInParent<Collider2D>()[1]));
            var targetPosition = director.GetBoundedCameraPosition(director.GetBounds(this.GetComponentsInParent<Collider2D>()[1]));

            director.FreeCamera = true;
            while (time < duration)
            {
                director.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            director.SetBounds(this.GetComponentsInParent<Collider2D>()[1]);
            director.FreeCamera = false;
        }

        public void TeleportCharacter(PlayerCharacterComponent playerCharacter) => StartCoroutine(IMovePlayerCharacter(playerCharacter));

    }
}
