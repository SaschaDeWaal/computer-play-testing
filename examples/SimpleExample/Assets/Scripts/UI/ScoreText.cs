using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    [SerializeField] private Text text = null;
    [SerializeField] private int player;

    private void Update() {
        text.text = $"Score: {Game.Instance.Players[player].Score.ToString()}";
    }
}
