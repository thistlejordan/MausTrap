using Assets.Scripts.Components.Items;
using Assets.Scripts.Enums;
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

        public void UpdateKeyCountInMenu(LevelEnum level, int count)
        {
            foreach (var option in _options)
            {
                if (!(option is KeyMenuOptionComponent keyOption))
                {
                    continue;
                }

                if (keyOption.Level == level)
                {
                    keyOption.UpdateKeyCount(count);
                    return;
                }
            }
        }

        public void AddKeyToInventoryMenu(LevelEnum level, int count)
        {
            foreach (var option in _options)
            {
                if (!(option is KeyMenuOptionComponent keyOption))
                {
                    continue;
                }

                if (keyOption.Level == level)
                {
                    keyOption.SetKeyData(level, count);
                    return;
                }
            }
        }
    }
}
