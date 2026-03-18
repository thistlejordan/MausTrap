using UnityEngine;

namespace Assets.Scripts.Components.GameWorld
{
    public class GameArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.GetComponent<PlayerCharacterComponent>() is null) return;

            var cameraDirector = otherCollider.GetComponentInChildren<CameraDirector>();

            cameraDirector.SetBounds(this.GetComponent<Collider2D>());
        }

        //private void OnTriggerExit2D(Collider2D otherCollider)
        //{
        //    if (otherCollider.GetComponent<PlayerCharacterComponent>() is null) return;

        //    var cameraDirector = otherCollider.GetComponentInChildren<CameraDirector>();

        //    Debug.Log($"OnTriggerEnter2D: {this.name}");

        //    cameraDirector.ClearBounds();
        //}
    }
}
