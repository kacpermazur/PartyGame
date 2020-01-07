using System.Collections;
using System.Collections.Generic;
using Core;
using Sound;
using UnityEngine;

namespace UI.Panel
{
    public class UIManager : MonoBehaviour, IInitializable
    {
        [SerializeField] private UIMainMenu _mainMenu;
        [SerializeField] private UIGame _uiGame;
        [SerializeField] private UIWinner _uiWinner;

        public UIMainMenu MainMenuUI => _mainMenu;
        public UIGame InGameUI => _uiGame;
        public UIWinner WinnerUI => _uiWinner;
        
        private List<UIPanel> _uiPanels = new List<UIPanel>();
        
        public void Initialize()
        {
            AddListeners();
            
            _uiPanels.Add(_mainMenu);
            _uiPanels.Add(_uiGame);
            _uiPanels.Add(_uiWinner);
            
            _mainMenu.Initialize();
        }
        
        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _mainMenu.onButtonStartClicked += OnBtnStartClicked;
            _mainMenu.onButtonExitClicked += OnBtnExitClicked;
        }
        
        private void RemoveListeners()
        {
            _mainMenu.onButtonStartClicked -= OnBtnStartClicked;
            _mainMenu.onButtonExitClicked -= OnBtnExitClicked;
        }
        
        public void OpenPanel(UIPanel uiPanel)
        {
            foreach (UIPanel panel in _uiPanels)
            {
                panel.Close();
            }

            uiPanel.Open();
        }

        private void OnBtnStartClicked()
        {
            GameManager.instance.SoundManager.PlaySound("ui-click", SoundManager.SoundType.UI);
            OpenPanel(_uiGame);
            GameManager.instance.StartGame();
        }
        
        private void OnBtnExitClicked()
        {
            Application.Quit();
        }
    }
}