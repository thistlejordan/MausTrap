using Assets.Scripts.Components.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class EquippedItemHUDComponent : MonoBehaviour
    {
        [SerializeField] private Image _equippedItemImageComponent;
        [SerializeField] private Text _equippedItemTextComponent;

        public virtual void UpdateEquippedItem(ItemComponent item)
        {
            _equippedItemImageComponent.sprite = item?.GetComponent<SpriteRenderer>().sprite;
            _equippedItemImageComponent.enabled = item != null;

            _equippedItemTextComponent.text = item != null && item is ConsumableComponent ? ((ConsumableComponent)item).Quantity.ToString() : "";
            _equippedItemTextComponent.enabled = item != null && item is ConsumableComponent;
        }
    }
}
