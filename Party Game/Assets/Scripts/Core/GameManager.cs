using System;
using System.Collections;
using UnityEngine;
using Player;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int numberOfRoundsToWin = 3;
    public float startCountDown = 3.0f;
    public float endCountDown = 10.0f;

    [Header("Game Setup")]
    public ScalingMiniGame _ScalingMiniGame;
    public TextMeshProUGUI gameMessage;
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
        _playerInputManager = PlayerInputManager.instance;
        
    }

    private void Start()
    {
        gameMessage.text = "Waiting For Players!";
        
        _startWait = new WaitForSeconds(startCountDown);
        _endWait = new WaitForSeconds(endCountDown);

        InitializeAllPlayer();
        
        //SetControlSchemes();

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
            InputUser.PerformPairingWithDevice(Gamepad.all[3], user: player3.user);
            InputUser.PerformPairingWithDevice(Gamepad.all[4], user: player4.user);
            
 
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
            if (players[i].playerInstance.activeSelf)
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
            if (players[i].playerInstance.activeSelf)
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

        if (_gameWinner != null)
        {
            message = _gameWinner.playerName + " WINS THE GAME!";
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
        SetControlSchemes();
        DisablePlayersControls();
        _currentRound++;

        gameMessage.text = "Round: " + _currentRound;
        
        yield return _startWait;
    }

    private IEnumerator InRound()
    {
        EnablePlayersControls();
        gameMessage.text = String.Empty;
        
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
        

        gameMessage.text = RoundEndMessage();
        
        yield return _endWait;
    }
}
