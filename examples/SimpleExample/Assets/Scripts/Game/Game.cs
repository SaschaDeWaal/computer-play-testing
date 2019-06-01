using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class Game {

    [DataMember] public WonState wonState { get; private set; } = WonState.None;

    [DataMember] public Player[] Players;
    [DataMember] private bool[] PlayedCard = new[] {false, false};

    public string FeedbackText { get; private set; } = "";

    public Game() {
        Players = new[] {
            new Player(new List<BaseWeapon>() {new Hamer(), new Hand(), new Hand(), new Laser(), new Pistol(), new Sword()}), 
            new Player(new List<BaseWeapon>() {new Hamer(), new Hand(), new Hand(), new Laser(), new Pistol(), new Sword()}), 
        };
    }

    public void ChoiceWeapon(int player, BaseWeapon weapon) {
        if (PlayerCanPlayWeapon(player)) {

            Players[player].WeaponsInPlay.Add(weapon);
            Players[player].WeaponsInHand.Remove(weapon);
            PlayedCard[player] = true;
            FeedbackText = "";

            if (PlayedCard[0] && PlayedCard[1]) {
                OnAllPlayersPlayed();
            }
        }
    }

    public bool PlayerCanPlayWeapon(int player) {
        return (PlayedCard[player] == false);
    }

    private void OnAllPlayersPlayed() {

        int totalScore1 = Players[0].WeaponsInPlay.Select(a => a.Power).Sum();
        int totalScore2 = Players[1].WeaponsInPlay.Select(a => a.Power).Sum();
        
        // remove cards
        for (int playerIndex = 0; playerIndex < Players.Length; playerIndex++) {
            Players[playerIndex].WeaponsInPlay.ForEach(w => w.UsedLife());

            Players[playerIndex].WeaponsInPlay.FindAll(w => w.ShouldBeRemoved()).ToList().ForEach(e => {
                Players[playerIndex].WeaponsInPlay.Remove(e);
            });
            
        }
        
        
        // check who won
        if (totalScore1 == totalScore2) {
            wonState = WonState.Tie;
        } else if (totalScore1 > totalScore2) {
            wonState = WonState.Player1;
            Players[0].Score++;
        } else {
            wonState = WonState.Player2;
            Players[1].Score++;
        }
        
        
        FeedbackText = CreateString();
        ResetGame();
    }
    
    private void ResetGame() {
        PlayedCard = new[] {false, false};
    }

    private string CreateString() {
        switch (wonState) {
            case WonState.Tie:
                return "Tie";
            
            case WonState.Player1:
                return $"Player 1 wins";
            
            case WonState.Player2:
                return $"Player 2 wins";
            
            default:
                return "";
        }
    }

    private static Game _instance = null;
    public static Game Instance {
        get {
            if (_instance == null) {
                _instance = new Game();
            }

            return _instance;
        }

        set { _instance = value; }
        
    }
}
