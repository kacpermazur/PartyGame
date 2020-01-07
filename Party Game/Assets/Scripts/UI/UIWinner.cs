using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Panel
{
    public class UIWinner : UIPanel
    {
        [SerializeField] private TextMeshProUGUI _winnerText;
        
        public void ChangeWinnerText(string msg)
        {
            _winnerText.text = msg;
        }
    }
}