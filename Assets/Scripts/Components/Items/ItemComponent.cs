using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components.Items
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemComponent : MonoBehaviour
    {
        [SerializeField] protected float _cooldown;

        public virtual void Use(CharacterComponent user) => throw new NotImplementedException();

        public void ItemGetAnimation() => StartCoroutine(IItemGetAnimation());

        public IEnumerator IItemGetAnimation()
        {
            GetComponent<Animator>().SetInteger("state", 1);
            Show();
            yield return new WaitForSeconds(1.5f);
            Hide();
        }

        public void Show() => GetComponent<SpriteRenderer>().enabled = true;

        public void Hide() => GetComponent<SpriteRenderer>().enabled = false;
    }
}
