using Assets.Scripts.Models;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components.Items
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class WeaponComponent : ItemComponent
    {
        [SerializeField] private int _damage;
        [SerializeField] private int _knockbackForce;

        private Coroutine AttackCoroutine;

        public void OnTriggerEnter2D(Collider2D other)
        {
            var target = other.GetComponent<CharacterComponent>();

            if (target != null)
            {
                var attackModel = BuildAttackModel();
                attackModel.KnockbackDirection = (other.transform.position - GetComponentInParent<CharacterComponent>().transform.position).normalized;
                target.ReceiveAttack(attackModel);
            }
        }

        public override void Use(CharacterComponent user)
        {
            if (AttackCoroutine != null)
            {
                StopCoroutine(AttackCoroutine);
            }

            gameObject.SetActive(true);
            AttackCoroutine = StartCoroutine(IAttack(user));
        }

        public abstract IEnumerator IAttack(CharacterComponent attacker);

        private AttackModel BuildAttackModel() =>
            new AttackModel()
            {
                Damage = _damage,
                KnockbackForce = _knockbackForce
            };
    }
}
