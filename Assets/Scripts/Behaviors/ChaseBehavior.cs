using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Behaviors
{
    [RequireComponent(typeof(CharacterComponent))]
    public class ChaseBehavior : MonoBehaviour
    {
        private CharacterComponent _character;
        private bool _pursuing = false;
        private List<CharacterComponent> _acquiredTargets = new List<CharacterComponent>();

        [SerializeField] private float _minChaseDistance = 0f;
        [SerializeField] private List<CharacterComponent> _targetCharacters;
        [SerializeField] private List<FactionsEnum> _targetFactions;

        public void Awake()
        {
            _character = GetComponent<CharacterComponent>();
        }

        public void AcquireTarget(CharacterComponent target)
        {
            if (_targetCharacters.Contains(target) || _targetFactions.Contains(target.Faction))
            {
                _acquiredTargets.Add(target);
                TryPursuing(target);
            }
        }

        public void DisengageTarget(CharacterComponent target) => _acquiredTargets.Remove(target);

        private void TryPursuing(CharacterComponent target)
        {
            if (!_pursuing)
            {
                StartCoroutine(IPursue(target));
            }
        }

        private IEnumerator IPursue(CharacterComponent target)
        {
            _pursuing = true;

            while (_acquiredTargets.Contains(target))
            {
                RaycastHit2D hit = Physics2D.Linecast(transform.position, target.transform.position, LayerMask.NameToLayer("Player"));

                if ((hit.collider == null || hit.collider == _character.Collider) && Vector2.Distance(transform.position, target.transform.position) > _minChaseDistance)
                {
                    Debug.DrawLine(transform.position, target.transform.position, Color.green);
                    _character.Move(new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y));
                }
                else
                {
                    Debug.DrawLine(transform.position, target.transform.position, Color.red);
                    _character.Move(Vector2.zero);
                }

                yield return null;
            }

            _pursuing = false;

            if (_acquiredTargets.Count > 0) { TryPursuing(_acquiredTargets[0]); }
            else { _character.InputMove(Vector2.zero); }
        }
    }
}
