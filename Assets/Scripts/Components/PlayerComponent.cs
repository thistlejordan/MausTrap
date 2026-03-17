using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class PlayerComponent : MonoBehaviour
    {
        public PlayerInputComponent _playerInput;

        public PlayerCharacterComponent _playerCharacterComponent;
        public DialogComponent _dialogComponent;
        public InventoryMenuComponent _inventoryMenuComponent;
        public MapMenuComponent _mapComponent;
        public MainMenuComponent _mainMenuComponent;
        public MenuCursorComponent _cursorComponent;

        public PlayerCharacterComponent Character { get => _playerCharacterComponent; private set => _playerCharacterComponent = value; }

        public IEnumerator DialogCoroutine { get; private set; }
        public IEnumerator InventoryCoroutine { get; private set; }
        public IEnumerator MapCoroutine { get; private set; }
        public IEnumerator MenuCoroutine { get; private set; }

        private void Awake()
        {
            SetInputs(InputType.Character);
        }

        private void SetInputs(InputType inputType)
        {
            ClearInputs();

            switch (inputType)
            {
                case InputType.Character: { SetInputCharacter(); } break;
                case InputType.Menu: { SetInputMenu(); } break;
                case InputType.Inventory: { SetInputInventory(); break; }
                case InputType.Dialog: { SetInputDialog(); } break;
                default:
                    Debug.Log($"Could not set {inputType}");
                    break;
            }
        }

        private void ClearInputs()
        {
            _playerInput.DirectionalPad = null;
            _playerInput.ButtonA = null;
            _playerInput.ButtonB = null;
            _playerInput.ButtonX = null;
            _playerInput.ButtonY = null;
            _playerInput.ButtonL = null;
            _playerInput.ButtonR = null;
            _playerInput.ButtonStart = null;
            _playerInput.ButtonSelect = null;

            _playerCharacterComponent.InputMove(Vector2.zero);
        }

        private void SetInputCharacter()
        {
            _playerInput.DirectionalPad = _playerCharacterComponent.InputMove;
            _playerInput.ButtonA = () => _playerCharacterComponent.Interact(this);
            _playerInput.ButtonB = _playerCharacterComponent.Attack;
            _playerInput.ButtonX = () => AwaitInventory(InputType.Character);
            _playerInput.ButtonY = _playerCharacterComponent.UseItem;
            _playerInput.ButtonStart = () => AwaitMenu(InputType.Character);
        }

        private void SetInputDialog()
        {
            _playerInput.ButtonA = () => _dialogComponent.Accept();
            _playerInput.ButtonB = () => _dialogComponent.Cancel();
        }

        private void SetInputInventory()
        {
            _playerInput.DirectionalPad = _cursorComponent.SendInputs;
            _playerInput.ButtonA = () => _inventoryMenuComponent.Accept();
            _playerInput.ButtonB = () => _inventoryMenuComponent.Cancel();
            _playerInput.ButtonX = () => _inventoryMenuComponent.CloseMenu();
        }

        private void SetInputMenu()
        {
            _playerInput.DirectionalPad = _cursorComponent.SendInputs;
            _playerInput.ButtonA = () => _mainMenuComponent.Accept();
            _playerInput.ButtonB = () => _mainMenuComponent.Cancel();
            _playerInput.ButtonStart = () => _mainMenuComponent.CloseMenu();
        }

        public void AwaitDialog(string text, DialogAwaitType dialogAwaitType, InputType pausedInput)
        {
            DialogCoroutine = IAwaitDialog(text, dialogAwaitType, pausedInput);
            StartCoroutine(DialogCoroutine);
        }

        private IEnumerator IAwaitDialog(string text, DialogAwaitType dialogAwaitType, InputType pausedInput)
        {
            SetInputs(InputType.Dialog);
            _dialogComponent.CreateDialog(text, dialogAwaitType);

            while (_dialogComponent.isActiveAndEnabled) { yield return null; }

            SetInputs(pausedInput);
        }

        public void AwaitInventory(InputType pausedInput)
        {
            InventoryCoroutine = IAwaitInventory(pausedInput);
            StartCoroutine(InventoryCoroutine);
        }

        private IEnumerator IAwaitInventory(InputType pausedInput)
        {
            SetInputs(InputType.Inventory);
            _inventoryMenuComponent.OpenMenu();

            while (_inventoryMenuComponent.isActiveAndEnabled) { yield return null; }

            SetInputs(pausedInput);
        }

        public void AwaitMap(InputType pausedInput)
        {
            MapCoroutine = IAwaitMap(pausedInput);
            StartCoroutine(MapCoroutine);
        }

        private IEnumerator IAwaitMap(InputType pausedInput)
        {
            SetInputs(InputType.Map);
            _mapComponent.OpenMap();

            while (_mapComponent.isActiveAndEnabled) { yield return null; }

            SetInputs(pausedInput);
        }

        public void AwaitMenu(InputType pausedInput)
        {
            MenuCoroutine = IAwaitMenu(pausedInput);
            StartCoroutine(MenuCoroutine);
        }

        private IEnumerator IAwaitMenu(InputType pausedInput)
        {
            SetInputs(InputType.Menu);
            _mainMenuComponent.OpenMenu();

            while (_mainMenuComponent.isActiveAndEnabled) { yield return null; }

            SetInputs(pausedInput);
        }

        public void Await(IEnumerator coroutine, InputType pausedInput)
        {
            StartCoroutine(IAwait(coroutine, pausedInput));
        }

        public IEnumerator IAwait(IEnumerator coroutine, InputType pausedInput)
        {
            ClearInputs();
            yield return coroutine;
            SetInputs(pausedInput);
        }
    }
}
