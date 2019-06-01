## Prevent broken games with computer play testing

#### Part 3. Example of implementing the framework

This is part 3 of 4 about preventing broken games with computer play testing. If you didn't read part 1, please read that first. 



Introduction

In this part we will look at how to implement this framework into your game. For this example, I have made a simple game to test it on. You can view and download it [here](https://github.com/SaschaDeWaal/computer-play-testing/tree/master/examples/SimpleExample).

It's a two player game. Each player receives 5 different weapons. Every weapon has a power level and a lifetime. At the start of the round each player selects a single weapon and places it in the battle area. The player with the most power points (all weapons summed up together) will win that round, and gets 1 point.

However, every weapon has a different lifetime. The lifetime is the amount of round it can be used. When the lifetime is below 0, the weapon will be removed.

The game ends when all weapons from the hands are played. The player who has the most points wins.

These are the weapons that can be played.

| Name   | Power | Lifetime |
| ------ | ----- | -------- |
| Sword  | 5     | 5        |
| Pistol | 10    | 1        |
| Hand   | 1     | 8        |
| Laser  | 8     | 2        |
| Hamer  | 1     | 12       |

![ExampleGame](images/ExampleGame.gif?raw=true)



### Example of implementing

We are going to start with building the `GameStatus`. This is a data object that stores the status of the game. In our case, we can simply add the Game object as a DataMember field. So when we serialize it, it will be stored with the status. When we will jump to this status, we will set Game.Instance to this Game object. So all objects know which one to use.

Please notice the comments in the code.

```c#
using System.Runtime.Serialization;
using ComputerPlayTesting;

[DataContract] //<-- We are going to copy game status. Instead of creating a deep copy, we will use Serialization
public class MyGameStatus : GameStatus {
    
	// Contains our game
	[DataMember] public Game Game;

	public MyGameStatus(int madeActions = 0) : base(madeActions) {
		
	}

	// This will be called when we jump to this status
	public override void OnWillJumpToThis() {
		
		// We will replace the current game object with the game from this status
		Game.Instance = Game;
	}


	// This will be called when we jump away from this point
	public override void OnWillJumpAway() {
		// In this example, we don't need this
		// But when you work with saving system, you should save the file here.
	}

}
```



Next we need to define the actions. With this game there are up to 5 possible actions. For example, play Sword or play Pistol from hand. These actions are very similar, Place a weapon from hand to the battle area. So we can make a single action for all 5 items. We can add the selected weapon to the constructor.

Please, note that we store the index instead of the weapon object. This is because after we checked if this action is executable, we will copy the `GameStatus` and execute this action in this new copied status. So we need to execute it to a different object.

```c#
using ComputerPlayTesting;

public class PlayWeaponPlayerAction : PlayerAction {

	// We want to prevent saving objects, so we store the index
	// So when the gamestatus changes, we are still able to execute this action
	private int weaponIndex = -1;
	
	// Store name for tracking
	public string WeaponName { get; private set; }

	public PlayWeaponPlayerAction(int weapon) {
		weaponIndex = weapon;
	}
	
	public override void Execute(GameStatus gameStatus, int playerIndex) {
		
		// Get the necessary variables
		Game game = ((MyGameStatus) gameStatus).Game;
		BaseWeapon weapon = game.Players[playerIndex].WeaponsInHand[weaponIndex];
		WeaponName = weapon.Name;
		
		// Execute the action
		game.ChoiceWeapon(playerIndex, weapon);
	}

	public override bool IsExecutable(GameStatus gameStatus, int playerIndex) {
		Game game = ((MyGameStatus) gameStatus).Game;
		return game.PlayerCanPlayWeapon(playerIndex);
	}

}

```



And of course we need a `Playtester` who will decide what action we are going to test. In this example game, it doesn't really matter. All actions are always possible. But when you have more complex gameplay, not all actions can be executed. This player will look at the actions, and return actions that can be executed.

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using ComputerPlayTesting;

public class MyPlayTester : PlayTester {
    
    public override List<PlayerAction> GetTestableGameActions(GameStatus gameStatus, int playerIndex) {
        Game game = ((MyGameStatus) gameStatus).Game;
        List<PlayerAction> playerActions = new List<PlayerAction>();

        // Loop thought all objects.
        // If action is possible to execute, add them to the list
        for (int i = 0; i < game.Players[playerIndex].WeaponsInHand.Count; i++) {
            PlayWeaponPlayerAction action = new PlayWeaponPlayerAction(i);

            if (action.IsExecutable(gameStatus, playerIndex)) {
                playerActions.Add(action);
            }
        }
        
        // Shuffle for randomness
        Random rnd = new Random();
        playerActions = playerActions.OrderBy(o => rnd.Next()).ToList();

        return playerActions;
    }
    
}

```



The last part to be able to play it, is the manager. The manager, like the name is saying, managed the playtest. It will receive ticks from the `TickComponent`. Every tick it will jump to a Status, all `Playtesters` are asked what kind of action they like to test with this status. For every selected action, the status will be copied with serialization and the action will be executed to that new copied status. The status will then be added to the queue for a later tick.

| Step   | Action                                                       |
| ------ | ------------------------------------------------------------ |
| Step 1 | Receive tick                                                 |
| Step 2 | Jump to a queued status                                      |
| Step 3 | ask every  playtesters what actions must be tested with this status |
| Step 4 | For every testable action, copy current status and apply that action |
| Step 5 | These new status will be added to the queue for later tick   |
| Step 6 | Selected status will be removed.                             |
| Step 7 | Waiting for next tick. Go to step 1                          |

Implementing the manager might looks like this

```c#
using ComputerPlayTesting;
using UnityEngine;


public class MyPlayTestManager : PlayTestManager<MyPlayTester, MyGameStatus, PlayTestObserver> {
	protected override void CreateObjects(string testId) {
		PlayTesters = new MyPlayTester[]{new MyPlayTester(), new MyPlayTester()};
		GameStatus = new MyGameStatus();
		PlayTestObserver = new PlayTestObserver();
		
		GameStatus.Game = Game.Instance;
	}

	protected override void SaveResult(string data) {
		// All data that comes from the test, will go here
		// Here you could send it to a database, or write it away to the hard disk
		// Data is a json string
		
		Debug.Log(data);
	}
}

```

We now only need to tick the manager. This should be a component that will be placed in the scene. 

It does nothing else then ticking the test manager. 

```c#
using ComputerPlayTesting.MyBox;
using UnityEngine;

public class MyTickComponent : MonoBehaviour {
        
    [Header("Play test")]
    [SerializeField] private bool enablePlayTesting = false;
    [SerializeField] private float tickInterval = 0.0f;
    [SerializeField] private int maxGameStatusInMemory = 500;
    [SerializeField] private int cleanGameStatesAfter = 800;
    [SerializeField] private int resetAfterTicks = 5000;
    
    [Header("Database")]
    [SerializeField] private bool randomID = false;
    [SerializeField] [ConditionalField("randomID", false)] private string testId = "";

    private MyPlayTestManager _playTestManager = null;
    private float _intervalTimer = 0;
    private int _ticks = 0;

    private void Update() {
        if (enablePlayTesting) {

            bool shouldBeReset = (resetAfterTicks > 0 && _ticks >= resetAfterTicks);

            if (_playTestManager == null || shouldBeReset) {
                RestartGame();
            } else if ((resetAfterTicks < 0 || _ticks < resetAfterTicks) && _playTestManager.NeedsMoreTicks) {
                _intervalTimer += Time.deltaTime;

                if (_intervalTimer >= tickInterval) {
                    _ticks++;
                    _intervalTimer = 0;
                    _playTestManager.Tick();
                }
            } 
        }
    }

    private void RestartGame() {
        _ticks = 0;

        Game.Instance = new Game();
        _playTestManager = new MyPlayTestManager();
        _playTestManager.PrepareTest( maxGameStatusInMemory, cleanGameStatesAfter, (randomID) ? "" : testId );
    }
}
```



So, if you add the component to the scene, enable the test and start the game you should see the game playing.

![FirstExampleRunning](images/FirstExampleRunning.gif?raw=true)



Now the computer can play the game, however we are building this to test the game. We first need to know what we do need. In this example, we are going to look at how many times a weapon did win. So first we need to build the result object. This is a object that contains the result, and our system will serialize to json and will store it for later use.

```c#
using System.Runtime.Serialization;
using ComputerPlayTesting;

[DataContract]
public class MyWeaponResult : PlayTestResult {

	[DataMember] private string weaponName;
	[DataMember] public int BattledTimes = 0;
	[DataMember] public int WonTimes = 0;

	[DataMember] public override string ResultType {
		get => "WeaponResult";
		set { }
	}


	public MyWeaponResult(string name, int battled, int won) {
		weaponName = name;
		BattledTimes = battled;
		WonTimes = won;
	}
}
```



We also need a tracker. When a weapon is played, we want to keep watching it until it's life has ended. When watching, we will count the amount of times it did win, and the amount it battled.

```c#
using System.Runtime.Serialization;
using ComputerPlayTesting;

[DataContract]
public class MyObjectTracker: PlayTestObjectTracker {

	[DataMember] private int ownerIndex;
	[DataMember] private string weaponName;
	
	[DataMember] private int playedBattles;
	[DataMember] private int wonBattles;
	
	public MyObjectTracker(int owner, string name) {
		ownerIndex = owner;
		weaponName = name;
	}

	public override void OnTick(GameStatus gameStatus) {

		Game game = ((MyGameStatus) gameStatus).Game;
		BaseWeapon weapon = GetTrackingWeapon(game);

		if (weapon == null) {
			// Weapon not found, this means it's life has ended. We can store the results and destroy this tracker
			StoreResult(new MyWeaponResult(weaponName, playedBattles, wonBattles));
			Destroy();

		} else {
			// Every turn we do a battle. So let's count them
			playedBattles++;

			if (game.wonState == ((ownerIndex == 0) ? WonState.Player1 : WonState.Player2)) {
				wonBattles++;
			}
		}
	}

	private BaseWeapon GetTrackingWeapon(Game game) {
		return game.Players[ownerIndex].WeaponsInPlay.Find(w => w.Name == weaponName);
	}
	
}
```



We need a observer. A observer is a object that observes the game. It can do different things, but for now it will only create our trackers. Every time when the `playtester` plays a weapon, we will create a new tracker.

```c#
using ComputerPlayTesting;
using UnityEngine;


public class MyObserver : PlayTestObserver {


	public override void ObserveAfterExecutedAction(GameStatus prevGameStatus, GameStatus currentGameStatus, PlayerAction lastAppliedAction, int playerIndex) {
		base.ObserveAfterExecutedAction(prevGameStatus, currentGameStatus, lastAppliedAction, playerIndex);
		
		// If last action is PlayWeaponPlayerAction, we will add a tracker to track this item
		if (lastAppliedAction is PlayWeaponPlayerAction) {
			PlayWeaponPlayerAction weaponAction = (PlayWeaponPlayerAction) lastAppliedAction;
			AddTracker(currentGameStatus, new MyObjectTracker(playerIndex, weaponAction.WeaponName));
		}

	}
	
}

```



Lastly, we need to edit the manager a bit so the observer will be called

```c#
using ComputerPlayTesting;
using UnityEngine;


public class MyPlayTestManager : PlayTestManager<MyPlayTester, MyGameStatus, MyObserver> {
	protected override void CreateObjects(string testId) {
		PlayTesters = new MyPlayTester[]{new MyPlayTester(), new MyPlayTester()};
		GameStatus = new MyGameStatus();
		PlayTestObserver = new MyObserver();
		
		GameStatus.Game = Game.Instance;
	}

	protected override void SaveResult(string data) {
		// All data that comes from the test, will go here
		// Here you could send it to a database, or write it away to the harddisk
		// Data is a json string
		
		Debug.Log(data);
	}
}
```

So if you run it now, you will see the result object in the log.

![resultLog](images/resultLog.gif?raw=true)

The logs you see are the results of the test. It is the data that you won by running the test. Right now, it is logged but you should save this on the hard disk or store it to a database.

This tutorial is about computer play testing and not about storing and showing data. I am sure you can find plenty of other tutorial about this. For my test, I created with NodeJS a socket server that is connected to CouchDB. The test script sends the data via socket to the NodeJS server that then stores the data there.

A website build with react can then show you the data with nice graphs from the internet. You can find this code [here](https://github.com/SaschaDeWaal/computer-play-testing/tree/master/examples/LegendOfTheFiveRings/Game) if you would like to see it: but I know there are better tutorials online about this topic.



### Next up

[**Part 4: Tips , Tricks,and problem solvers**](part4.md)



### Thanks for reading

Thanks for reading this tutorial. If you have any question, or if you made something cool with or based on this framework. Please contact me at info@developersascha.nl