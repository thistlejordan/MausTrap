using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class KeyCountHUDComponent : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Text _keyCountTextComponent;
        [SerializeField] private LevelEnum _currentLevel = LevelEnum.LEVEL_1;

        #endregion

        #region Methods

        public void UpdateKeyCount(int value)
        {
            var str = value.ToString();

            if (_keyCountTextComponent.text != str)
            {
                _keyCountTextComponent.text = str;
            }
        }

        public void SetCurrentLevel(LevelEnum level)
        {
            this._currentLevel = level;
        }

        public LevelEnum GetCurrentLevel()
        {
            return this._currentLevel;
        }

        #endregion
    }
}
