using UnityEngine;

namespace Assets.Scripts.Components
{
    public class PlayerInputComponent : MonoBehaviour
    {
        public delegate void DirectionalInput(Vector2 direction);
        public delegate void ButtonInput();

        public DirectionalInput DirectionalPad;
        public ButtonInput ButtonA;
        public ButtonInput ButtonB;
        public ButtonInput ButtonX;
        public ButtonInput ButtonY;
        public ButtonInput ButtonL;
        public ButtonInput ButtonR;
        public ButtonInput ButtonStart;
        public ButtonInput ButtonSelect;

        void Update()
        {
            var dir = new Vector2()
            {
                x = Input.GetAxisRaw("Horizontal"),
                y = Input.GetAxisRaw("Vertical")
            };

            DirectionalPad?.Invoke(dir);

            if (Input.GetButtonDown("ButtonA")) { ButtonA?.Invoke(); }
            if (Input.GetButtonDown("ButtonB")) { ButtonB?.Invoke(); }
            if (Input.GetButtonDown("ButtonX")) { ButtonX?.Invoke(); }
            if (Input.GetButtonDown("ButtonY")) { ButtonY?.Invoke(); }
            if (Input.GetButtonDown("ButtonL")) { ButtonL?.Invoke(); }
            if (Input.GetButtonDown("ButtonR")) { ButtonR?.Invoke(); }
            if (Input.GetButtonDown("ButtonStart")) { ButtonStart?.Invoke(); }
            if (Input.GetButtonDown("ButtonSelect")) { ButtonSelect?.Invoke(); }
        }
    }
}