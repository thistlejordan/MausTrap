using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class MenuCursorComponent : MonoBehaviour
    {
        private MenuComponent _menu;
        private Vector2 _inputs = new Vector2(0, 0);
        private bool _flicker = false;
        private int _focus = 0;

        public float _pressAndHoldKeyDelay = 0.1f;
        public float _flickerPeriod = 0.35f;
        public Image _image;

        public Image Image { get => _image; set => _image = value; }
        public int Focus { get => _focus; }

        private void Awake() => Image = GetComponent<Image>();

        public void MoveCursor()
        {
            var options = _menu.Options;
            var columnCount = _menu.ColumnCount;
            var modBool = (options.Count % columnCount == 0) ? 0 : 1;

            if (_inputs.y == 1)
            {
                if (_focus - columnCount >= 0) { _focus -= columnCount; }
                else if (_menu.WrapOptions)
                {
                    int x = (((options.Count / columnCount)) * columnCount) + (_focus % columnCount);
                    if (x <= options.Count - 1) { _focus = x; }
                    else { _focus = x - columnCount; }
                }
                else { _focus = 0; }
            }
            else if (_inputs.y == -1)
            {
                if (_focus + columnCount <= options.Count - 1) { _focus += columnCount; }
                else if (_menu.WrapOptions) { _focus = _focus % columnCount; }
                else { _focus = options.Count - 1; }
            }

            if (_inputs.x == -1)
            {
                if (_focus - 1 >= 0) { _focus -= 1; }
                else if (_menu.WrapOptions) { _focus = options.Count - 1; }
                else { _focus = 0; }
            }
            else if (_inputs.x == 1)
            {
                if (_focus + 1 <= options.Count - 1) { _focus += 1; }
                else if (_menu.WrapOptions) { _focus = 0; }
                else { _focus = options.Count - 1; }
            }

            transform.position = options[_focus].transform.position;
        }

        public void SendInputs(Vector2 inputs)
        {
            if (_inputs.x != inputs.x || _inputs.y != inputs.y)
            {
                _inputs.Set(inputs.x, inputs.y);
            }
        }

        public IEnumerator IProcessInputs()
        {
            var timer = 0f;

            // Allow Menu Options to load
            yield return new WaitForEndOfFrame();

            // Reset cursor transform
            transform.position = _menu.Options[_focus].transform.position;

            while (_menu.isActiveAndEnabled)
            {
                if (_menu.Options.Count == 0) { yield return null; }

                if (_inputs != Vector2.zero)
                {
                    if (timer <= 0)
                    {
                        MoveCursor();
                        timer = _pressAndHoldKeyDelay;
                    }
                }
                else
                {
                    timer = 0;
                }

                timer = (timer - Time.deltaTime > 0) ? timer - Time.deltaTime : 0;

                yield return new WaitForEndOfFrame();
            }
        }

        public void StartCursor(MenuComponent menu, bool hasFlicker)
        {
            _menu = menu;
            _focus = 0;
            gameObject.SetActive(true);
            Image.enabled = true;
            if (hasFlicker) { RestartCursorFlicker(); }
            StartCoroutine(IProcessInputs());
        }

        public void StopCursor()
        {
            _menu = null;
            gameObject.SetActive(false);
        }

        private void RestartCursorFlicker()
        {
            _flicker = false;
            StartCoroutine(ICursorFlicker());
        }

        private IEnumerator ICursorFlicker()
        {
            _flicker = true;
            Image.enabled = true;

            while (_flicker)
            {
                Image.enabled = !Image.enabled;
                yield return new WaitForSeconds(_flickerPeriod);
            }
        }
    }
}
