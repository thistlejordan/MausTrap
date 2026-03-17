using Assets.Scripts.Components;
using Assets.Scripts.Components.Items;
using Assets.Scripts.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private bool _randomizeAllChests = false;

        public List<ChestComponent> _chests;
        public List<ItemComponent> _items;

        private void Awake()
        {
            var randomChests = new List<ChestComponent>();

            if (_randomizeAllChests)
            {
                randomChests = _chests.Where(chest => chest.Randomize).ToList();
            }

            RandomizeChestsWithItems(randomChests, _items);
        }

        public void ClearChestItems(ChestComponent chest)
        {
            var items = chest.GetComponentsInChildren<ItemComponent>();
            foreach (var item in items) { Destroy(item.gameObject); }
        }

        public List<ItemComponent> TakeItemsFromChests(List<ChestComponent> chests)
        {
            var items = new List<ItemComponent>();

            return items;
        }

        public void RandomizeChestsWithItems(List<ChestComponent> chests, List<ItemComponent> items)
        {
            chests.RemoveContents();
            chests.Shuffle();
            items.Shuffle();

            for (int i = 0; i < _chests.Count && i < items.Count; i++)
            {
                var item = Instantiate(items[i]);
                item.name = items[i].name;
                _chests[i].PutItem(item);
            }
        }
    }
}
