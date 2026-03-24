using UnityEngine;

namespace Assets.Scripts.Components
{
    public class HeadsUpDisplayComponent : MonoBehaviour
    {
        #region Fields

        private EquippedItemHUDComponent _equippedItemComponent;
        private MoneyHUDComponent _moneyComponent;
        private HealthBarHUDComponent _healthBarComponent;
        private KeyCountHUDComponent _keyCountComponent;

        #endregion

        #region Unity Awake

        private void Awake()
        {
            _equippedItemComponent = _equippedItemComponent ?? GetComponentInChildren<EquippedItemHUDComponent>();
            _moneyComponent = _moneyComponent ?? GetComponentInChildren<MoneyHUDComponent>();
            _healthBarComponent = _healthBarComponent ?? GetComponentInChildren<HealthBarHUDComponent>();
            _keyCountComponent = _keyCountComponent ?? GetComponentInChildren<KeyCountHUDComponent>();
        }

        #endregion

        #region Properties

        public EquippedItemHUDComponent EquippedItemComponent { get => _equippedItemComponent; set => _equippedItemComponent = value; }

        public MoneyHUDComponent MoneyComponent { get => _moneyComponent; set => _moneyComponent = value; }

        public HealthBarHUDComponent HealthBarComponent { get => _healthBarComponent; set => _healthBarComponent = value; }

        public KeyCountHUDComponent KeyCountComponent { get => _keyCountComponent; set => _keyCountComponent = value; }

        #endregion
    }
}
