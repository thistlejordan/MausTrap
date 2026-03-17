using Assets.Scripts.Components.Items;
using System.Collections.Generic;

namespace Assets.Scripts.Helpers
{
    public static class InventoryHelper
    {
        public static bool IsDuplicate(ItemComponent newItem, IEnumerable<ItemComponent> items)
        {
            foreach (var item in items)
            {
                if (newItem.name == item.name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
