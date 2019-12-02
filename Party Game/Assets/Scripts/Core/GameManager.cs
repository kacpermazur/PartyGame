using System;
using System.Collections;
using UnityEngine;
using Player;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int numberOfRoundsToWin = 3;
    public float startCountDown = 3.0f;
    public float endCountDown = 3.0f;

    public TextMeshProUGUI gameMessage;
    
    public GameObject playerPrefab;
    public PlayerManager[] players;

    private int _currentRound;
    private WaitForSeconds _startWait;
    private WaitForSeconds _endWait;

    private PlayerManager _roundWinner;
    private PlayerManager _gameWinner;
    
    private void Start()
    {
        _startWait = new WaitForSeconds(startCountDown);
        _endWait = new WaitForSeconds(endCountDown);
        
        InitializeAllPlayer();

        StartCoroutine(Game());
    }

    void InitializeAllPlayer()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].playerInstance =
                Instantiate(playerPrefab, players[i].SpawnPoint.position, players[i].SpawnPoint.rotation);
            players[i].playerID = i + 1;
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
        message += "\n\n\n\n";
        
        message += "SCORES: \n";
        
        for (int i = 0; i < players.Length; i++)
        {
            message += players[i].playerName + ": " + players[i].numberOfWins + " Wins/n";
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
        DisablePlayersControls();
        _currentRound++;

        gameMessage.text = "Round: " + _currentRound;
        yield return _startWait;
    }

    private IEnumerator InRound()
    {
        EnablePlayersControls();
        gameMessage.text = String.Empty;

        while (!LastPlayerCheck())
        {
            yield return null;
        }
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
        
        if (_roundWinner != null)
            _roundWinner.numberOfWins++;
        
        gameMessage.text = RoundEndMessage();

        yield return endCountDown;
    }
}
