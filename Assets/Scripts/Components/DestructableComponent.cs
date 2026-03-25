using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DestructableComponent : AnimatedObjectComponent
    {
        [SerializeField] private int defense;
        [SerializeField] private int health;
        [SerializeField] private int healthMax;
        [SerializeField] private bool invincible;
        private new Rigidbody2D rigidbody;

        public int Defense => this.defense;

        public int Health
        {
            get => this.health;
            protected set
            {
                if (value > this.HealthMax)
                {
                    this.health = this.HealthMax;
                }
                else if (value < 0)
                {
                    this.health = 0;
                }
                else
                {
                    this.health = value;
                }
            }
        }

        public int HealthMax => this.healthMax;

        public bool Invincible
        {
            get => this.invincible;
            protected set
            {
                this.invincible = value;
            }
        }

        public Rigidbody2D Rigidbody
        {
            get
            {
                if (this.rigidbody == null)
                {
                    if (this.TryGetComponent<Rigidbody2D>(out var rigidbody))
                    {
                        this.rigidbody = rigidbody;
                    }
                    else
                    {
                        Debug.LogError($"DestructableComponent on {this.gameObject.name} is missing a Rigidbody2D component.");
                    }
                }

                return this.rigidbody;
            }
        }

        public virtual void RestoreHealth(int value) => this.Health += (this.Health + value > this.HealthMax) ? 0 : value;

        private int CalculateTrueDamage(int damage) => (damage - this.Defense > 0) ? damage - this.Defense : 1;

        private void TakeDamage(int trueDamage) => this.Health = (this.Health - trueDamage >= 0) ? this.Health - trueDamage : 0;

        private void CheckForDeath()
        {
            if (this.Health <= 0 && !this.Animator.GetBool("dead"))
            {
                Debug.Log($"Health: {this.Health}");
                this.Animator.SetBool("dead", true);
                this.Die();
            }
        }

        public virtual void ReceiveAttack(AttackModel attack)
        {
            if (this.Invincible)
            {
                return;
            }

            this.InvincibilityFrames();
            this.TakeDamage(CalculateTrueDamage(attack.Damage));
            this.Knockback(attack.KnockbackDirection, attack.KnockbackForce);
            this.CheckForDeath();
        }

        private void InvincibilityFrames()
        {
            this.Invincible = true;
            this.StartCoroutine(IInvincibilityFrames());
        }

        private IEnumerator IInvincibilityFrames()
        {
            var time = 0f;
            var i = 1;

            while (time < GameManager.Instance.IFramesDuration)
            {
                this.SpriteRenderer.enabled = i > GameManager.Instance.m_IFrameFlickerRate ? true : false;

                if (i < (11 - GameManager.Instance.m_IFrameFlickerRate) * 2)
                {
                    ++i;
                }
                else
                {
                    i = 1;
                }

                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            this.SpriteRenderer.enabled = true;
            this.Invincible = false;
        }

        public virtual Coroutine Knockback(Vector2 direction, float force) => this.StartCoroutine(IKnockback(direction, force));

        private IEnumerator IKnockback(Vector2 direction, float force)
        {
            this.Rigidbody.linearVelocity = direction * force;
            yield return new WaitForSeconds(0.1f);
        }

        private void Die() => this.StartCoroutine(IDie());

        private IEnumerator IDie()
        {
            this.Animator.SetBool("dead", true);
            yield return new WaitForSeconds(this.Animator.GetCurrentAnimatorStateInfo(0).length);
            Destroy(this.gameObject);
        }
    }
}
