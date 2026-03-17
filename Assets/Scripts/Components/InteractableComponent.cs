using Assets.Scripts.Components.Items;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public abstract class InteractableComponent : MonoBehaviour
    {
        #region Fields

        protected SpriteRenderer _spriteRenderer;

        [SerializeField] protected string _dialog;
        [SerializeField] protected ItemComponent item;

        #endregion

        #region Unity Awake

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            var item = GetComponentInChildren<ItemComponent>();

            if (this.item == null && item != null)
            {
                PutItem(item);
            }
        }

        #endregion

        #region Methods

        public abstract void OnInteract(PlayerComponent player, PlayerCharacterComponent playerCharacter);

        public void PutItem(ItemComponent item)
        {
            if (item == null)
            {
                Debug.LogWarning("Will not put null item. Use TakeItem() to nullify item.", this);
                return;
            }

            this.item = item;
            this.item.transform.SetParent(this.transform);
            this.item.Hide();
        }

        public ItemComponent TakeItem()
        {
            var item = this.item;
            this.item = null;
            return item;
        }

        public void RemoveContents()
        {
            if (this.item != null)
            {
                Destroy(this.item);
            }
        }

        #endregion
    }
}
