using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class AnimatedObjectComponent : MonoBehaviour
    {
        private Animator animator;
        private new Collider2D collider;
        private SpriteRenderer spriteRenderer;

        protected Animator Animator
        {
            get
            {
                if (this.animator == null)
                {
                    var animator = this.GetComponent<Animator>();

                    if (animator != null)
                    {
                        this.animator = animator;
                    }
                    else
                    {
                        Debug.LogError($"AnimatedObjectComponent on {gameObject.name} is missing an Animator component.");
                    }
                }

                return this.animator;
            }
        }

        public Collider2D Collider
        {
            get
            {
                if (this.collider == null)
                {
                    var collider = this.GetComponent<Collider2D>();

                    if (collider != null)
                    {
                        this.collider = collider;
                    }
                    else
                    {
                        Debug.LogError($"AnimatedObjectComponent on {gameObject.name} is missing a Collider2D component.");
                    }
                }

                return this.collider;
            }
        }

        protected SpriteRenderer SpriteRenderer
        {
            get
            {
                if (this.spriteRenderer == null)
                {
                    var spriteRenderer = this.GetComponent<SpriteRenderer>();

                    if (spriteRenderer != null)
                    {
                        this.spriteRenderer = spriteRenderer;
                    }
                    else
                    {
                        Debug.LogError($"AnimatedObjectComponent on {gameObject.name} is missing a SpriteRenderer component.");
                    }
                }

                return this.spriteRenderer;
            }
        }
    }
}
