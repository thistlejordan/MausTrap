using Assets.Scripts.Components.Items;
using System;

namespace Assets.Scripts.Components
{
    public class InventoryMenuComponent : MenuComponent
    {
        public void AddItemToInventoryMenu(ItemComponent item)
        {
            foreach (var option in _options)
            {
                var itemOption = (ItemMenuOptionComponent)option;

                if (itemOption.name.Equals(item.name, StringComparison.OrdinalIgnoreCase))
                {
                    itemOption.SetItem(item);
                    return;
                }
            }
        }
    }
}
