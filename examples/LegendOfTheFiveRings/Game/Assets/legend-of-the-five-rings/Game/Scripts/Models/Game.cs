using System;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public delegate void GameUpdatedEventHandler(ChangeEvent changeEvent);

[DataContract]
public class Game {
	
	public enum GameStatus {
		Playing,
		WonByPlayer1,
		WonByPlayer2,
	}

	public static event GameUpdatedEventHandler OnGameUpdated;
	[DataMember] public GameStatus CurrentGameStatus;
	
	[DataMember] private Player _playerOne;
	[DataMember] private Player _playerTwo;
	[DataMember] public PhaseManager PhaseManager { get; private set; }
	[DataMember] public int TurnIndex = -1;
	[DataMember] public int MadeChanges { get; private set; }
	[DataMember] public ChangeEvent LastEvent { get; private set; }

	[DataMember] public string EventText;
	
	[DataMember] public bool AnimationEnabled = true;
	
	public Player PlayerInTurn => GetPlayer(TurnIndex);


	public void PrepareGame(PlayerStartValues player1StartValues, PlayerStartValues player2StartValues, GameView gameView) {
		CurrentGameStatus = GameStatus.Playing;
		
		_playerOne = new Player(this, 0);
		_playerTwo = new Player(this, 1);
		
		_playerOne.SetStartValues(true, player1StartValues);
		_playerTwo.SetStartValues(false, player2StartValues);

		TurnIndex = -1;
		PhaseManager = new PhaseManager(this);
	}

	public void StartGame() {
		PhaseManager.Start();
	}

	public void EndTurn() {
		TurnIndex = (PlayerInTurn == _playerOne) ? 1 : 0;
	}

	public void SetPlayerTurn(Player player) {
		TurnIndex = (player != null) ? player.Index : -1;
	}

	public Player GetPlayer(int index) {
		if (index == 0) {
			return _playerOne;
		} if (index == 1) {
			return _playerTwo;
		}

		return null;
	}

	public void ApplyChanges(ChangeEvent changeEvent) {
		
		// check if somebody did win
		int winResult = CheckWon();
		if (winResult != 0) {
			CurrentGameStatus = (winResult == 1) ? GameStatus.WonByPlayer1 : GameStatus.WonByPlayer2;
			LastEvent = ChangeEvent.Create(EventType.GameWon);
			OnGameUpdated(LastEvent);
			return;
		}
		
		MadeChanges++;
		LastEvent = changeEvent;
		if (OnGameUpdated != null) {
			OnGameUpdated(changeEvent);
		}
	}

	
	private static GameView _gameView;
	public static GameView GameViewInstance {
		get {
			if (_gameView == null) _gameView = GameObject.FindObjectOfType<GameView>();
			return _gameView;
		}
	}
	public static Game Instance {
		get {
			if (_gameView == null) _gameView = GameObject.FindObjectOfType<GameView>();
			return _gameView.Game;
		}
	}

	private int CheckWon() {
		int wonBy = 0;

		if (_playerOne.HonorPool <= 0 || _playerTwo.HonorPool >= 25 || _playerTwo.Provinces[4].Standing == false) {
			wonBy = 2;
		}
		
		if (_playerTwo.HonorPool <= 0 || _playerOne.HonorPool >= 25 || _playerOne.Provinces[4].Standing == false) {
			wonBy = 1;
		}
		

		return wonBy;
	}
	
}
