using System;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [Serializable]
    public class PlayerManager
    {
        public Color PlayerColor;
        public Transform SpawnPoint;

        [HideInInspector] public int playerID;
        [HideInInspector] public string playerName;
        [HideInInspector] public GameObject playerInstance;
        [HideInInspector] public int numberOfWins;
        
        private PlayerMovement _movement;
        private PlayerAttack _attack;
        private CollisionListener _listener;

        public void Initialize()
        {
            _movement = playerInstance.GetComponent<PlayerMovement>();
            _attack = playerInstance.GetComponent<PlayerAttack>();
            _listener = playerInstance.GetComponentInChildren<CollisionListener>();

            _movement.playerID = playerID;
            _attack.playerID = playerID;
            _listener.playerID = playerID;

            playerName = "<color=#" + ColorUtility.ToHtmlStringRGB(PlayerColor) + ">PLAYER " + playerName + "</color>";

            MeshRenderer[] renderers = playerInstance.GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = PlayerColor;
            }
        }

        public void EnableControls()
        {
            _movement.enabled = true;
            _attack.enabled = true;
            _listener.enabled = true;
        }

        public void DisableControls()
        {
            _movement.enabled = false;
            _attack.enabled = false;
            _listener.enabled = false;
        }

        public void Reset()
        {
            playerInstance.transform.position = SpawnPoint.position;
            playerInstance.transform.rotation = SpawnPoint.rotation;
            
            playerInstance.SetActive(false);
            playerInstance.SetActive(true);
        }
    }
}
