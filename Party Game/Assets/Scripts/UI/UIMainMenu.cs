using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Panel
{
    public class UIMainMenu : UIPanel
    {
        public UnityAction onButtonStartClicked;
        public UnityAction onButtonExitClicked;

        [SerializeField] private Button _start;
        [SerializeField] private Button _exit;

        public override void Initialize()
        {
            _start.onClick.AddListener(ButtonStart);
            _exit.onClick.AddListener(ButtonExit);
        }

        private void ButtonStart()
        {
            onButtonStartClicked?.Invoke();
        }
        
        private void ButtonExit()
        {
            onButtonExitClicked?.Invoke();
        }
        
    }
}