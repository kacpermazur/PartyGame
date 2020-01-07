using System;
using System.Collections;
using UnityEngine;
using Player;
using Sound;
using TMPro;
using UI.Panel;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        
        [Header("Managers")] 
        [SerializeField] private UIManager _uiManger;
        [SerializeField] private SoundManager _soundManager;
        
        public static GameManager instance => _instance;
        public UIManager UiManager => _uiManger;
        public SoundManager SoundManager => _soundManager;
        
        [Header("Game Settings")] 
        public int numberOfRoundsToWin = 3;
        public float startCountDown = 3.0f;
        public float endCountDown = 10.0f;

        [Header("Game Setup")] 
        public ScalingMiniGame _ScalingMiniGame;
        public GameObject playerPrefab;
        public PlayerManager[] players;

        private int _currentRound;
        private WaitForSeconds _startWait;
        private WaitForSeconds _endWait;

        private PlayerManager _roundWinner;
        private PlayerManager _gameWinner;

        private PlayerInputManager _playerInputManager;
        private PlayerInput[] _playerSchemes;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            
            _playerInputManager = PlayerInputManager.instance;
            _soundManager.Initialize();
            _uiManger.Initialize();
            _uiManger.OpenPanel(_uiManger.MainMenuUI);
        }

        private void Start()
        {
            _startWait = new WaitForSeconds(startCountDown);
            _endWait = new WaitForSeconds(endCountDown);
            
            InitializeAllPlayer();
            //SetControlSchemes();
        }

        public void StartGame()
        {
            _uiManger.OpenPanel(_uiManger.InGameUI);
            _uiManger.InGameUI.ChangeMessageText("Waiting For Players!");
                
            _soundManager.PlaySound("music", SoundManager.SoundType.MUSIC);
            StartCoroutine(Game());
        }

        private void SetControlSchemes()
        {
            var player1 = PlayerInput.all[0];
            var player2 = PlayerInput.all[1];
            var player3 = PlayerInput.all[2];
            var player4 = PlayerInput.all[3];

            player1.user.UnpairDevices();
            player2.user.UnpairDevices();
            player3.user.UnpairDevices();
            player4.user.UnpairDevices();

            int gamepadCount = Gamepad.all.Count;

            if (gamepadCount >= 4)
            {
                Debug.Log("4");
                InputUser.PerformPairingWithDevice(Gamepad.all[0], user: player1.user);
                InputUser.PerformPairingWithDevice(Gamepad.all[1], user: player2.user);
                InputUser.PerformPairingWithDevice(Gamepad.all[2], user: player3.user);
                InputUser.PerformPairingWithDevice(Gamepad.all[3], user: player4.user);


                player1.user.ActivateControlScheme("Gamepad");
                player2.user.ActivateControlScheme("Gamepad");
                player3.user.ActivateControlScheme("Gamepad");
                player4.user.ActivateControlScheme("Gamepad");
            }
            else if (gamepadCount >= 3)
            {
                Debug.Log("3");
                InputUser.PerformPairingWithDevice(Gamepad.all[0], user: player1.user);
                InputUser.PerformPairingWithDevice(Gamepad.all[1], user: player2.user);
                InputUser.PerformPairingWithDevice(Gamepad.all[2], user: player3.user);
                InputUser.PerformPairingWithDevice(Keyboard.current, user: player4.user);


                player1.user.ActivateControlScheme("Gamepad");
                player2.user.ActivateControlScheme("Gamepad");
                player3.user.ActivateControlScheme("Gamepad");
                player4.user.ActivateControlScheme("Keyboard&Mouse");
            }
            else
            {

                Debug.Log("error");
            }

        }

        void InitializeAllPlayer()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].playerInstance =
                    Instantiate(playerPrefab, players[i].SpawnPoint.position, players[i].SpawnPoint.rotation);
                players[i].playerID = i;
                players[i].Initialize();
            }
        }

        private void ResetPlayers()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].Reset();
            }
        }

        private void EnablePlayersControls()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].EnableControls();
            }
        }

        private void DisablePlayersControls()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].DisableControls();
            }
        }

        private bool LastPlayerCheck()
        {
            int numberOfPlayersLeft = 0;

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].isAlive)
                {
                    numberOfPlayersLeft++;
                }
            }

            return numberOfPlayersLeft <= 1;
        }

        private PlayerManager RoundWinner()
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].isAlive)
                    return players[i];
            }

            return null;
        }

        private PlayerManager GameWinner()
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].numberOfWins == numberOfRoundsToWin)
                    return players[i];
            }

            return null;
        }

        private String RoundEndMessage()
        {
            string message = "Draw";

            if (_roundWinner != null)
                message = _roundWinner.playerName + "Wins The Round!";
            message += "\n\n";

            message += "SCORES: \n";

            for (int i = 0; i < players.Length; i++)
            {
                message += players[i].playerName + ": " + players[i].numberOfWins + " Wins\n";
            }

            return message;
        }

        private IEnumerator Game()
        {
            yield return StartCoroutine(StartRound());
            yield return StartCoroutine(InRound());
            yield return StartCoroutine(EndRound());

            if (_gameWinner != null)
            {
                SceneManager.LoadScene(0); // ToDO: Add Mini Game Change
            }
            else
            {
                StartCoroutine(Game());
            }
        }

        private IEnumerator StartRound()
        {
            ResetPlayers();
            DisablePlayersControls();
            _currentRound++;

            string round = "Round: " + _currentRound;
            _uiManger.InGameUI.ChangeMessageText(round);

            yield return _startWait;
        }

        private IEnumerator InRound()
        {
            EnablePlayersControls();
            //_uiManger.InGameUI.ChangeMessageText(String.Empty);

            _ScalingMiniGame.StartMiniGame();

            while (!LastPlayerCheck())
            {
                yield return null;
            }

            _ScalingMiniGame.RestMiniGame();
        }

        private IEnumerator EndRound()
        {
            DisablePlayersControls();

            _roundWinner = null;
            _roundWinner = RoundWinner();

            if (_roundWinner != null)
                _roundWinner.numberOfWins++;

            _gameWinner = null;
            _gameWinner = GameWinner();
            
            _uiManger.InGameUI.ChangeMessageText(RoundEndMessage());
            
            if (_gameWinner != null)
            {
                _soundManager.StopSound("music", SoundManager.SoundType.MUSIC);
                _uiManger.OpenPanel(_uiManger.WinnerUI);
                
                string winner = _gameWinner.playerName + " WINS THE GAME!";
                _uiManger.WinnerUI.ChangeWinnerText(winner);
            }

            yield return _endWait;
        }
    }
}