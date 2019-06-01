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
		// Here you could send it to a database, or write it away to the hard disk
		// Data is a json string
		
		Debug.Log(data);
	}
}
