using Assets.Scripts.Behaviors;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(Collider2D))]
    public class ChaseArea : MonoBehaviour
    {
        private ChaseBehavior _chaseBehavior;

        private void Awake()
        {
            _chaseBehavior = GetComponentInParent<ChaseBehavior>();

            if (_chaseBehavior != null)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _chaseBehavior.GetComponent<Collider2D>());
            }
            else
            {
                Debug.LogWarning("Entity will not pursue targets: cannot find ChaseBehavior in parent component.", this);
            }
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            CharacterComponent otherCharacter = collider.GetComponent<CharacterComponent>();
            if (otherCharacter) { _chaseBehavior.AcquireTarget(otherCharacter); }
        }

        public void OnTriggerExit2D(Collider2D collider)
        {
            CharacterComponent otherCharacter = collider.GetComponent<CharacterComponent>();
            if (otherCharacter) { _chaseBehavior.DisengageTarget(otherCharacter); }
        }
    }
}
