using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components.Items
{
    public class MeleeWeaponComponent : WeaponComponent
    {
        [SerializeField] private float _attackSpeed;

        public override IEnumerator IAttack(CharacterComponent attacker)
        {
            float time = _attackSpeed;
            float originalTime = time;

            while (time > 0.0f)
            {
                transform.parent.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, attacker.Rotation - 90f), Quaternion.Euler(0, 0, attacker.Rotation + 60f), 1 - (time / originalTime));

                time -= Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            gameObject.SetActive(false);
        }
    }
}
