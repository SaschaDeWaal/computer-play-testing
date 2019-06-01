using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class QuestionPanelView : MonoBehaviour {

	[SerializeField] private Text questionField;
	[SerializeField] private GameObject[] optionsObjects;
	[SerializeField] private Text[] optionsText;

	private Action<int> _callback;

	private void Start() {
		transform.localScale = Vector3.zero;
	}

	public void AskQuestion(string question, string[] options, Action<int> callback) {

		_callback = callback;
		
		foreach (GameObject option in optionsObjects) {
			option.SetActive(false);
		}

		questionField.text = question;

		for (int i = 0; i < Mathf.Min(options.Length, optionsText.Length); i++) {
			optionsObjects[i].SetActive(true);
			optionsText[i].text = options[i];
		}

		
		transform.localScale = Vector3.one;
	}

	public void OnOptionClicked(int index) {
		_callback(index);
		transform.localScale = Vector3.zero;
	}

	private static QuestionPanelView _instance;
	public static QuestionPanelView Instance {
		get {
			if (!_instance) {
				_instance = GameObject.FindObjectOfType<QuestionPanelView>();
			}

			return _instance;
		}
	}
}
