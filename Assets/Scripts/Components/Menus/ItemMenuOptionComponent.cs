using Assets.Scripts.Components.Items;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class ItemMenuOptionComponent : MenuOptionBaseComponent
    {
        public ItemComponent _item;

        public ItemComponent Item { get => _item; set => _item = value; }

        public void SetItem(ItemComponent item)
        {
            Item = item;
            GetComponent<Image>().enabled = true;
        }

        public override void OnSelect() => throw new System.NotImplementedException();

        public void OnSelect(PlayerCharacterComponent playerCharacter) => playerCharacter.EquipItem(_item);
    }
}