using ComputerPlayTesting.MyBox;
using UnityEngine;


public class FiveRingsTestTickComponent: MonoBehaviour {
	[Header("Game")] [SerializeField] private GameView _gameView;
    
	[Header("Play test")]
	[SerializeField] private bool enablePlayTesting = false;
	[SerializeField] private float tickInterval = 0.0f;
	[SerializeField] private int maxGameStatusInMemory = 500;
	[SerializeField] private int cleanGameStatesAfter = 800;
    
	[SerializeField] private int resetAfterTicks = 5000;
    
	[Header("Server connection")]
	[SerializeField] private string hostName = "localhost";
	[SerializeField] private int port = 3500;
    
	[Header("Database")]
	[SerializeField] private bool randomID = false;
	[SerializeField] [ConditionalField("randomID", false)] private string testId = "";

	private FiveRingsManager _playTestManager = null;
	private float _intervalTimer = 0;
	private int _ticks = 0;

	private void Update() {

		if (enablePlayTesting) {

			if (_playTestManager == null) {
				_playTestManager = new FiveRingsManager();
				_playTestManager.PrepareTest( maxGameStatusInMemory, cleanGameStatesAfter, (randomID) ? "" : testId );
				_playTestManager.PrepareConnection(hostName, port);
			} else if ((resetAfterTicks < 0 || _ticks < resetAfterTicks) && _playTestManager.NeedsMoreTicks) {
				_intervalTimer += Time.deltaTime;

				if (_intervalTimer >= tickInterval) {
					_ticks++;
					_intervalTimer = 0;
					_playTestManager.Tick();
					Game.Instance.ApplyChanges(new ChangeEvent());
				}
			} else {
				RestartGame();
			}  

		}
        
	}

	private void RestartGame() {
		_ticks = 0;

		_gameView.CreateGame();
        
		_playTestManager = new FiveRingsManager();
		_playTestManager.PrepareTest( maxGameStatusInMemory, cleanGameStatesAfter, (randomID) ? "" : testId );
	}
}
