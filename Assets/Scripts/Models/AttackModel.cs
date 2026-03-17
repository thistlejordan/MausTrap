using UnityEngine;

namespace Assets.Scripts.Models
{
    public class AttackModel
    {
        public int Damage { get; set; }

        public int KnockbackForce { get; set; }

        public Vector2 KnockbackDirection { get; set; }
    }
}
