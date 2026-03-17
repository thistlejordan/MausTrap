using Assets.Scripts.Enums;
using Assets.Scripts.Managers;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class DialogComponent : MonoBehaviour
    {
        public GameObject _dialogBox;
        public Text _dialogText;

        private bool _acknowledgedFlag;
        private bool _typingFlag;

        private IEnumerator _typing;
        private IEnumerator _awaitAcknowledge;

        public void CreateDialog(string text, DialogAwaitType dialogAwaitType)
        {
            if (_typing != null) { StopCoroutine(_awaitAcknowledge); }
            if (_typing != null) { StopCoroutine(_typing); }
            gameObject.SetActive(true);
            TypeText(text);
            _acknowledgedFlag = false;

            switch (dialogAwaitType)
            {
                case DialogAwaitType.Acknowledge: { AwaitAcknowledge(); } break;
                case DialogAwaitType.TypingAndAcknowledge: { AwaitTypingAndAcknowledge(); } break;
                default: throw new NotImplementedException();
            }
        }

        public void Accept() => _acknowledgedFlag = true;

        public void Cancel() => _acknowledgedFlag = true;

        private void TypeText(string text)
        {
            _typing = ITyping(text);
            StartCoroutine(_typing);
        }

        private IEnumerator ITyping(string text)
        {
            _dialogText.text = "";
            _typingFlag = true;

            foreach (var character in text)
            {
                _dialogText.text += character;
                yield return new WaitForSecondsRealtime(1f - GameManager.Instance.m_TextSpeed / 10f);
            }

            _typingFlag = false;
        }

        public void AwaitAcknowledge()
        {
            _awaitAcknowledge = IAwaitAcknowledge();
            StartCoroutine(_awaitAcknowledge);
        }

        public IEnumerator IAwaitAcknowledge()
        {
            while (!_acknowledgedFlag) { yield return null; }
            gameObject.SetActive(false);
        }

        public void AwaitTypingAndAcknowledge()
        {
            _awaitAcknowledge = IAwaitTypingAndAcknowledge();
            StartCoroutine(_awaitAcknowledge);
        }

        IEnumerator IAwaitTypingAndAcknowledge()
        {
            while (_typingFlag || !_acknowledgedFlag) { yield return null; }
            gameObject.SetActive(false);
        }
    }
}
