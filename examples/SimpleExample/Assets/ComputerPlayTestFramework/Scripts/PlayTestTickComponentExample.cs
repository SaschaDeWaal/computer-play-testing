using ComputerPlayTesting.MyBox;
using UnityEngine;

/*namespace UnitPlayTesting {
    public abstract class PlayTestTickComponentExample : MonoBehaviour {

        [Header("Play test")] [SerializeField] private bool enablePlayTesting = false;
        [SerializeField] private float tickInterval = 0.0f;
        [SerializeField] private int maxGameStatusInMemory = 500;
        [SerializeField] private int cleanGameStatesAfter = 800;

        [SerializeField] private int resetAfterTicks = 5000;
        [Header("Database")] [SerializeField] private bool randomId = false;

        [SerializeField] [ConditionalField("randomID", false)]

        private string testId = "";

        private PlayTestManager<PlayTester, GameStatus, PlayTestObserver> _playTestManager = null;
        private float _intervalTimer = 0;
        private int _ticks = 0;

        private void Update() {

            if (enablePlayTesting) {

                if (_playTestManager == null) {
                    _playTestManager = new PlayTestManager<PlayTester, GameStatus, PlayTestObserver>();
                    _playTestManager.PrepareTest(maxGameStatusInMemory, cleanGameStatesAfter, (randomId) ? "" : testId);
                } else if ((resetAfterTicks < 0 || _ticks < resetAfterTicks) && _playTestManager.NeedsMoreTicks) {
                    _intervalTimer += Time.deltaTime;

                    if (_intervalTimer >= tickInterval) {
                        _ticks++;
                        _intervalTimer = 0;
                        _playTestManager.Tick();
                    }
                }
                else {
                    RestartGame();
                }

            }

        }

        private void RestartGame() {
            _ticks = 0;
            _playTestManager = new PlayTestManager<PlayTester, GameStatus, PlayTestObserver>();
            _playTestManager.PrepareTest(maxGameStatusInMemory, cleanGameStatesAfter, (randomId) ? "" : testId);
        }
    }
}*/
