using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components.Items
{
    public abstract class ProjectileComponent : MonoBehaviour
    {
        public int _damage;
        public float _velocity;

        private Rigidbody2D Rigidbody { get; set; }

        private IEnumerator FireCoroutine;

        private void Awake() => Rigidbody = GetComponent<Rigidbody2D>();

        public void Fire(Vector2 direction)
        {
            if (FireCoroutine != null) { StopCoroutine(FireCoroutine); }
            FireCoroutine = IFire(direction);
            StartCoroutine(FireCoroutine);
        }

        private IEnumerator IFire(Vector2 direction)
        {
            yield return transform.position = transform.position * (direction * Time.deltaTime);
        }
    }
}
