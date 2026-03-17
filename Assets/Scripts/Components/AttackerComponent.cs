using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(CharacterComponent))]
    public class AttackerComponent : MonoBehaviour
    {
        private CharacterComponent _character;
        private Coroutine _attackCoroutine;

        private void Awake()
        {
            _character = GetComponent<CharacterComponent>();
        }

        public void Attack()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
            }

            _attackCoroutine = StartCoroutine(ISwingWeapon(_character));
        }

        IEnumerator ISwingWeapon(CharacterComponent character)
        {
            float time = 0.25f;
            float originalTime = time;

            gameObject.SetActive(true);

            while (time > 0.0f)
            {
                GetComponentInParent<Transform>().rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, character.Rotation - 90f), Quaternion.Euler(0, 0, character.Rotation + 60f), 1 - (time / originalTime));
                time -= Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            gameObject.SetActive(false);
        }
    }
}
