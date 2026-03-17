using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class EnemyComponent : CharacterComponent
    {
        public int _attackDamage = 1;
        public float _knockbackForce = 20;

        public void Attack(CharacterComponent target) => target.ReceiveAttack(new Models.AttackModel() { Damage = _attackDamage });

        public void OnCollisionStay2D(Collision2D collision)
        {
            CharacterComponent otherCharacter = collision.collider.GetComponent<CharacterComponent>();

            if (!otherCharacter.Invincible)
            {
                otherCharacter.Knockback(-collision.contacts[0].normal, _knockbackForce);
                Attack(otherCharacter);
            }
        }
    }
}