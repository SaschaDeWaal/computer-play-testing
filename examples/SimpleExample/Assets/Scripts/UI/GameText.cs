using UnityEngine;
using UnityEngine.UI;

public class GameText : MonoBehaviour {
    [SerializeField] private Text text = null;
    
    private void Update() {
        text.text = Game.Instance.FeedbackText.ToString();
    }
}
