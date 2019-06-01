﻿using ComputerPlayTesting.MyBox;
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
