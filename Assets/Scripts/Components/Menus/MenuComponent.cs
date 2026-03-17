using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public abstract class MenuComponent : MonoBehaviour
    {
        public MenuCursorComponent _cursor;
        public Sprite _cursorSprite;
        public bool _cursorFlicker;

        public List<MenuOptionBaseComponent> _options;
        public int _columnCount = 1;
        public bool _wrapOptions = false;

        public List<MenuOptionBaseComponent> Options { get => _options; set => _options = value; }
        public int ColumnCount { get => _columnCount; set => _columnCount = value; }
        public bool WrapOptions { get => _wrapOptions; set => _wrapOptions = value; }

        private void Awake()
        {
            _options = GetComponentsInChildren<MenuOptionBaseComponent>().ToList();
            gameObject.SetActive(false);
        }

        public virtual void OpenMenu()
        {
            gameObject.SetActive(true);
            _cursor.StartCursor(this, _cursorFlicker);
        }

        public void CloseMenu()
        {
            _cursor.StopCursor();
            gameObject.SetActive(false);
        }

        public void Accept()
        {
            _options[_cursor.Focus].OnSelect();
        }

        public void Cancel()
        {
            CloseMenu();
        }
    }
}
