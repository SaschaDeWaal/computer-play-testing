using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeclareConflictDefenceView : BasePlayerObjectPhaseDependedView<ConflictPhase> {
	
	[SerializeField] private CameraView _cameraView = null;
	[SerializeField] private TextMesh _textMesh = null;
	
	private List<Character> _defendingCharacters = new List<Character>();
	
	private Vector3 _originalScale;
	private bool isDeclaring;

	private void Start() {
		_originalScale = transform.localScale;
		transform.localScale = Vector3.zero;
		
		_cameraView.OnSelect += new SelectionEventHandler(OnSelect);
	}
	
	protected override void OnGameChangedInPhase(ChangeEvent changeEvent) {
		bool canDeclareDefence = (CurGame.PlayerInTurn.Index == Owner.Index) && CurPhase.DeclaredConflict == true && CurPhase.DeclaredDefence == false;
		transform.localScale = (canDeclareDefence) ? _originalScale : Vector3.zero;

		if (!canDeclareDefence) {
			isDeclaring = false;
			_textMesh.text = "Declare Defence";
		}
	}
	
	protected override void OnPhaseEntered() {
		transform.localScale = _originalScale;
	}
	
	protected override void OnPhaseLeave() {
		transform.localScale = Vector3.zero;
	}

	public override void OnClicked() {
		if (!isDeclaring) {
			_textMesh.text = "Select characters";
			isDeclaring = true;
		}else if(_defendingCharacters.Count > 0){
			Controllers.Run(new DeclareDefenceController(Owner, _defendingCharacters.ToArray()));
		}
	}

	private void OnSelect(ISelectable selectable) {

		if (!isDeclaring) {
			return;
		}
		
		if (selectable is CharacterInPlayView) {
			CharacterInPlayView characterInPlayView = (CharacterInPlayView) selectable;
			Character character = characterInPlayView.GetCard().As<Character>();

			if (character.Owner.Index == Owner.Index && character.Bowed == false) {
				if (_defendingCharacters.Contains(character)) {
					_defendingCharacters.Remove(character);
				}
				else {
					_defendingCharacters.Add(character);
				}
			}

			if (_defendingCharacters.Count > 0) {
				_textMesh.text = "Declare";
			} else {
				_textMesh.text = "Select characters";
			}
		}

	}

}
