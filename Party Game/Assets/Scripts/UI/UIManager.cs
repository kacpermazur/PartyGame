using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Panel
{
    public class UIManager : MonoBehaviour, IInitializable
    {
        [SerializeField] private UIMainMenu _mainMenu;

        public UIMainMenu MainMenuUI => _mainMenu;
        
        private List<UIPanel> _uiPanels = new List<UIPanel>();
        
        public void Initialize()
        {
            _uiPanels.Add(_mainMenu);
            
            _mainMenu.Initialize();
        }
        
        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _mainMenu.onButtonStartClicked += OnBtnStartClicked;
        }
        
        private void RemoveListeners()
        {
            _mainMenu.onButtonStartClicked -= OnBtnStartClicked;
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
            
        }
    }
}