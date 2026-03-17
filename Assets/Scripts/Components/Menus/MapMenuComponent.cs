using UnityEngine;

namespace Assets.Scripts.Components
{
    public class MapMenuComponent : MenuComponent
    {
        public Sprite _map;

        public void OpenMap() => OpenMenu();

        public void CloseMap() => CloseMenu();
    }
}