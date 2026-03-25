using Assets.Scripts.Models;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components.Items
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class WeaponComponent : ItemComponent
    {
        [SerializeField] private int damage;
        [SerializeField] private int knockbackForce;
        private Coroutine attackCoroutine;

        public void OnTriggerEnter2D(Collider2D other)
        {
            var target = other.GetComponent<DestructableComponent>();

            if (target == null)
            {
                // No target to attack, ignore collision
                return;
            }

            var attackModel = this.BuildAttackModel();
            attackModel.KnockbackDirection = (other.transform.position - this.transform.position).normalized;
            target.ReceiveAttack(attackModel);
        }

        public override void Use(CharacterComponent user)
        {
            if (this.attackCoroutine != null)
            {
                this.StopCoroutine(attackCoroutine);
            }

            this.gameObject.SetActive(true);
            this.attackCoroutine = this.StartCoroutine(this.IAttack(user));
        }

        public abstract IEnumerator IAttack(CharacterComponent attacker);

        private AttackModel BuildAttackModel() =>
            new()
            {
                Damage = this.damage,
                KnockbackForce = this.knockbackForce
            };
    }
}
