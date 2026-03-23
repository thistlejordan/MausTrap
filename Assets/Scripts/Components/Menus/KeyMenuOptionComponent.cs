using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class KeyMenuOptionComponent : MenuOptionBaseComponent
    {
        [SerializeField] private Text _keyCountText;
        private LevelEnum _level;
        private int _keyCount;

        public LevelEnum Level { get => _level; set => _level = value; }
        public int KeyCount { get => _keyCount; set => _keyCount = value; }

        public void SetKeyData(LevelEnum level, int count)
        {
            this._level = level;
            this._keyCount = count;
            UpdateDisplay();
            GetComponent<Image>().enabled = true;
        }

        public void UpdateKeyCount(int count)
        {
            this._keyCount = count;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (_keyCountText != null)
            {
                _keyCountText.text = _keyCount.ToString();
            }
        }

        public override void OnSelect()
        {
            // Keys are not equippable, so this is intentionally empty
        }
    }
}
