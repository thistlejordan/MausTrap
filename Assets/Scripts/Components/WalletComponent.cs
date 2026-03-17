using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Components.Items
{
    public class WalletComponent : MonoBehaviour
    {
        private readonly int small = 200;
        private readonly int large = 500;
        private readonly int jumbo = 999;

        private WalletSizeType walletSizeType;

        private int walletCapacity;
        private int money;

        public int Money { get => this.money; }

        private void Awake()
        {
            this.walletCapacity = GetWalletCapacity();
        }

        private int GetWalletCapacity()
        {
            switch (walletSizeType)
            {
                case WalletSizeType.Small:
                    return this.small;
                case WalletSizeType.Large:
                    return this.large;
                case WalletSizeType.Jumbo:
                    return this.jumbo;
            }

            Debug.LogError("Could not determine wallet size.");

            return this.small;
        }

        public void AddMoney(int value)
        {
            if (value < 0)
            {
                Debug.LogWarning("Cannot add negative money to wallet. Use 'TakeMoney' instead.");
                return;
            }

            this.money = (this.money + value > this.walletCapacity) ? this.walletCapacity : this.money + value;
        }

        public void TakeMoney(int value)
        {
            if (value < 0)
            {
                Debug.LogWarning("Cannot take negative money from wallet.");
                return;
            }

            this.money = (this.money - value < 0) ? 0 : this.money - value;
        }
    }
}
