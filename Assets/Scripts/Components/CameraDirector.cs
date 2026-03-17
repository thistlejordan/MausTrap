using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(Camera))]
    public class CameraDirector : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Bounds? bounds;
        private bool freeCamera;

        public Transform Target { get => this.target; set => this.target = value; }
        public bool FreeCamera { get => this.freeCamera; set => this.freeCamera = value; }

        private void LateUpdate()
        {
            if (this.freeCamera) return;
            
            if (this.bounds.HasValue) this.transform.position = GetBoundedCameraPosition(bounds.Value);
            else this.transform.position = new Vector3(this.target.position.x, this.target.position.y, this.transform.position.z);
        }

        public Vector3 GetBoundedCameraPosition(Bounds bounds) => new Vector3(
            Mathf.Clamp(this.target.position.x, bounds.min.x, bounds.max.x),
            Mathf.Clamp(this.target.position.y, bounds.min.y, bounds.max.y),
            this.transform.position.z);

        public void ClearBounds() => this.bounds = null;

        public void SetBounds(Collider2D collider2D) => this.bounds = GetBounds(collider2D);

        public Bounds GetBounds(Collider2D collider2D)
        {
            var boundsVectors = GetBoundsVectors(collider2D);
            var bounds = new Bounds();
            bounds.SetMinMax(boundsVectors.Item1, boundsVectors.Item2);
            return bounds;
        }

        private (Vector3, Vector3) GetBoundsVectors(Collider2D collider2D)
        {
            var bounds = collider2D.bounds;
            var camera = this.GetComponent<Camera>();

            var height = camera.orthographicSize;
            var width = height * camera.aspect;

            var minX = bounds.min.x + width;
            var minY = bounds.min.y + height;
            var maxX = bounds.max.x - width;
            var maxY = bounds.max.y - height;

            return (new Vector2(minX, minY), new Vector2(maxX, maxY));
        }
    }
}
