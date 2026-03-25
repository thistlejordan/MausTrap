using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Behaviors
{
    [RequireComponent(typeof(CharacterComponent))]
    public class ChaseBehavior : MonoBehaviour
    {
        private bool pursuing = false;

        [SerializeField] private float chaseDistanceMinimum = 0f;
        //[SerializeField] private List<CharacterComponent> targetCharacters;
        [SerializeField] private List<FactionsEnum> targetFactions;
        [SerializeField] private List<CharacterComponent> acquiredTargets = new List<CharacterComponent>();
        private CharacterComponent character;


        private CharacterComponent Character
        {
            get
            {
                if (this.character == null)
                {
                    if (this.TryGetComponent<CharacterComponent>(out var character))
                    {
                        this.character = character;
                    }
                    else
                    {
                        Debug.LogError($"ChaseBehavior on {this.gameObject.name} is missing a CharacterComponent.");
                    }
                }

                return this.character;
            }
        }

        public void AcquireTarget(CharacterComponent target)
        {
            if (this.acquiredTargets.Contains(target))
            {
                // Target is not valid, ignore it.
                Debug.Log($"{this.gameObject.name} cannot acquire target {target.gameObject.name} because it is already an acquired target.");
                return;
            }

            if (!this.targetFactions.Contains(target.Faction))
            {
                Debug.Log($"{this.gameObject.name} cannot acquire target {target.gameObject.name} because it is not a target Faction.");
                return;
            }

            Debug.Log($"{this.gameObject.name} acquired target {target.gameObject.name}.");

            this.acquiredTargets.Add(target);
            this.TryPursuing(target);
        }

        public void DisengageTarget(CharacterComponent target) => acquiredTargets.Remove(target);

        private void TryPursuing(CharacterComponent target)
        {
            if (pursuing)
            {
                // Already pursuing a target.
                return;
            }

            this.StartCoroutine(IPursue(target));
        }

        private IEnumerator IPursue(CharacterComponent target)
        {
            this.pursuing = true;

            while (this.acquiredTargets.Contains(target))
            {
                var layerMask = LayerMask.GetMask("Player");

                var hit = Physics2D.Linecast(this.transform.position, target.transform.position, layerMask);

                if ((hit.collider == null || hit.collider == this.Character.Collider) && Vector2.Distance(this.transform.position, target.transform.position) > this.chaseDistanceMinimum)
                {
                    Debug.DrawLine(this.transform.position, target.transform.position, Color.green);
                    this.Character.Move(new Vector2(target.transform.position.x - this.transform.position.x, target.transform.position.y - this.transform.position.y));
                }
                else
                {
                    Debug.DrawLine(this.transform.position, target.transform.position, Color.red);
                    this.Character.Move(Vector2.zero);
                }

                yield return null;
            }

            this.pursuing = false;

            if (this.acquiredTargets.Count > 0)
            {
                this.TryPursuing(this.acquiredTargets.First());
            }
            else
            {
                this.Character.InputMove(Vector2.zero);
            }
        }
    }
}
