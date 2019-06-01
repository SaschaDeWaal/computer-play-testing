using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectsDatabase : MonoBehaviour {

	[SerializeField] private ScriptableObjects _scriptableObjects;


	public T FindByName<T>(string name) where T : CardData{
						
		foreach (ScriptableObject scriptableObject in _scriptableObjects.List) {
			if (scriptableObject.name == name) {
				return scriptableObject as T;
			}
		}

		Debug.Log("Couldn't find '" + name + "'");
		return null;
	}
	
	private static ScriptableObjectsDatabase _scriptable;
	public static ScriptableObjectsDatabase Instance {
		get {
			if (_scriptable == null) _scriptable = GameObject.FindObjectOfType<ScriptableObjectsDatabase>();
			return _scriptable;
		}
	}
}
