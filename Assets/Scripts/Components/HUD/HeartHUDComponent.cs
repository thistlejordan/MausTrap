using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class HeartHUDComponent : MonoBehaviour
    {
        public Animator _animator;

        public void SetToFull()
        {
            switch ((HeartAnimationType)_animator.GetInteger("state"))
            {
                case HeartAnimationType.FullToEmpty: EmptyToFull(); break;
                case HeartAnimationType.HalfToEmpty: EmptyToFull(); break;
                case HeartAnimationType.FullToHalf: HalfToFull(); break;
                case HeartAnimationType.EmptyToHalf: HalfToFull(); break;
                case HeartAnimationType.HalfToFull: break;
                case HeartAnimationType.EmptyToFull: break;
                default: throw new System.NotImplementedException();
            }
        }

        public void SetToEmpty()
        {
            switch ((HeartAnimationType)_animator.GetInteger("state"))
            {
                case HeartAnimationType.FullToEmpty: break;
                case HeartAnimationType.HalfToEmpty: break;
                case HeartAnimationType.FullToHalf: HalfToEmpty(); break;
                case HeartAnimationType.EmptyToHalf: HalfToEmpty(); break;
                case HeartAnimationType.HalfToFull: FullToEmpty(); break;
                case HeartAnimationType.EmptyToFull: FullToEmpty(); break;
                default: throw new System.NotImplementedException();
            }
        }

        public void SetToHalf()
        {
            switch ((HeartAnimationType)_animator.GetInteger("state"))
            {
                case HeartAnimationType.FullToEmpty: EmptyToHalf(); break;
                case HeartAnimationType.HalfToEmpty: EmptyToHalf(); break;
                case HeartAnimationType.FullToHalf: break;
                case HeartAnimationType.EmptyToHalf: break;
                case HeartAnimationType.HalfToFull: FullToHalf(); break;
                case HeartAnimationType.EmptyToFull: FullToHalf(); break;
                default: throw new System.NotImplementedException();
            }
        }

        private void FullToEmpty()
        {
            _animator.SetInteger("state", (int)HeartAnimationType.FullToEmpty);
            _animator.SetTrigger("trigger");
        }

        private void HalfToEmpty()
        {
            _animator.SetInteger("state", (int)HeartAnimationType.HalfToEmpty);
            _animator.SetTrigger("trigger");
        }

        private void FullToHalf()
        {
            _animator.SetInteger("state", (int)HeartAnimationType.FullToHalf);
            _animator.SetTrigger("trigger");
        }

        private void EmptyToHalf()
        {
            _animator.SetInteger("state", (int)HeartAnimationType.EmptyToHalf);
            _animator.SetTrigger("trigger");
        }

        private void HalfToFull()
        {
            _animator.SetInteger("state", (int)HeartAnimationType.HalfToFull);
            _animator.SetTrigger("trigger");
        }

        private void EmptyToFull()
        {
            _animator.SetInteger("state", (int)HeartAnimationType.EmptyToFull);
            _animator.SetTrigger("trigger");
        }
    }
}
