using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Panel
{
    public abstract class UIPanel : MonoBehaviour, IInitializable
    {
        enum UIState
        {
            NONE,
            OPEN,
            CLOSE
        }

        private UIState _state;

        public virtual void Initialize()
        {
            _state = UIState.NONE;
        }

        public virtual void Open()
        {
            if (_state == UIState.CLOSE || _state == UIState.NONE)
            {
                _state = UIState.OPEN;
                gameObject.SetActive(true);
            }
        }

        public virtual void Close()
        {
            if (_state == UIState.OPEN || _state == UIState.NONE)
            {
                _state = UIState.CLOSE;
                gameObject.SetActive(false);
            }
        }
    }
}