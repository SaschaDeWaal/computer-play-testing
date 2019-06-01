



using System;
using System.Linq;
using UnityEngine;

public class SelectObjectInGame {

	private CameraView _cameraView;
	private Type[] _types;
	private Action<ISelectable> _callback;
	private bool _singleSelect;
	private int _madeChanges;
	
	public SelectObjectInGame() {
		_cameraView = Camera.main.GetComponent<CameraView>();
		
		_cameraView.OnSelect += new SelectionEventHandler(OnSelect);
	}

	public void SelectOption<T>(Action<ISelectable> callback) where T : ISelectable {
		_types = new []{ typeof(T) };
		_callback = callback;
		_madeChanges = Game.Instance.MadeChanges;
	}
	
	public void SelectOption(Type[] types, Action<ISelectable> callback, bool singleSelect = true) {
		_types = types;
		_callback = callback;
		_singleSelect = singleSelect;
		_madeChanges = Game.Instance.MadeChanges;
	}

	private void OnSelect(ISelectable selectable) {
		if (_callback == null || _madeChanges != Game.Instance.MadeChanges) {
			return;
		}
		
		if (_types.Any(t => t == selectable.GetType() || selectable.GetType().IsSubclassOf(t))) {
			_callback(selectable);

			if (_singleSelect) {
				_callback = null;
			}
		}
	}

	private static SelectObjectInGame _instance = null;
	public static SelectObjectInGame Instance {
		get {
			if (_instance == null) {
				_instance = new SelectObjectInGame();
			}

			return _instance;
		}
	}
	
}
