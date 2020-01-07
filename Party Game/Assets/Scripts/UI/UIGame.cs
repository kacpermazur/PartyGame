using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Panel
{
    public class UIGame : UIPanel
    {
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TextMeshProUGUI _messageText;

        public void ChangeTimeText(string msg)
        {
            _timeText.text = msg;
        }
        
        public void ChangeMessageText(string msg)
        {
            _messageText.text = msg;
        }
    }
}