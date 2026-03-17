using Assets.Scripts.Components;
using Assets.Scripts.Components.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class CollectionsExtensions
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        public static List<ItemComponent> TakeItems(this List<InteractableComponent> interactables)
        {
            var items = new List<ItemComponent>();

            foreach (var interactable in interactables)
            {
                var item = interactable.TakeItem();

                if (item != null)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        public static List<ItemComponent> TakeItems(this List<ChestComponent> chests)
        {
            var items = new List<ItemComponent>();

            foreach (var chest in chests)
            {
                var item = chest.TakeItem();

                if (item != null)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        public static void RemoveContents(this List<ChestComponent> chests)
        {
            foreach (var chest in chests)
            {
                chest.RemoveContents();
            }
        }
    }
}
