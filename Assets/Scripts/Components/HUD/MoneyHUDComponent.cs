using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class MoneyHUDComponent : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Text _moneyTextComponent;

        #endregion

        #region Methods

        public void UpdateMoney(int value)
        {
            var str = value.ToString();

            if (_moneyTextComponent.text != str)
            {
                _moneyTextComponent.text = str;
            }
        }

        #endregion
    }
}
