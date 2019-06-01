using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeclareConflictView : BasePlayerObjectPhaseDependedView<ConflictPhase> {

	[SerializeField] private CameraView _cameraView = null;
	[SerializeField] private TextMesh _textMesh = null;

	private ConflictType _conflictType = ConflictType.None;
	private ElementType _elementType = ElementType.None;
	private Province _province = null;
	private List<Character> _attackingCharacter = new List<Character>();
	
	private Vector3 _originalScale;
	private bool isDeclaring = false;

	private void Start() {
		_originalScale = transform.localScale;
		transform.localScale = Vector3.zero;

		_cameraView.OnSelect += new SelectionEventHandler(OnSelect);
	}

	public override void OnClicked() {
		if (!isDeclaring) {
			_conflictType = ConflictType.None;
			_elementType = ElementType.None;
			_province = null;
			_attackingCharacter = new List<Character>();
			isDeclaring = true;
		}

		if (IsReady()) {
			Controllers.Run(new DeclareConflictController(_conflictType, _elementType, _attackingCharacter.ToArray(),
				Owner, _province));
		}
	}
	
	protected override void OnGameChangedInPhase(ChangeEvent changeEvent) {
		bool playersTurn = (CurGame.PlayerInTurn == Owner) && CurPhase.DeclaredConflict == false && (CurPhase.ElementOwner.Count < 4);
		transform.localScale = (playersTurn) ? _originalScale : Vector3.zero;

		if (!playersTurn) {
			isDeclaring = false;
			_textMesh.text = "Declare conflict";
		}
	}

	protected override void OnPhaseEntered() {
		transform.localScale = _originalScale;
		
	}
	
	protected override void OnPhaseLeave() {
		transform.localScale = Vector3.zero;
	}

	private void OnSelect(ISelectable selectable) {

		if (!isDeclaring) {
			return;
		}

		if (selectable is ElementTokenView) {
			_elementType = ((ElementTokenView) selectable).Element;
		}

		if (selectable is CharacterInPlayView) {
			CharacterInPlayView characterInPlayView = (CharacterInPlayView) selectable;
			Character character = characterInPlayView.GetCard().As<Character>();

			if (character.Owner == Owner && character.Bowed == false) {
				if (_attackingCharacter.Contains(character)) {
					_attackingCharacter.Remove(character);
					Debug.Log("remove");
				}
				else {
					_attackingCharacter.Add(character);
					Debug.Log("add");
				}
			}
		}

		if (selectable is ProvincePlayerObjectView) {
			Province province = ((ProvincePlayerObjectView) selectable).GetCard().As<Province>();
			if (province.Standing && province.Owner != Owner) {
				_province = province;
			}
		}

		IsReady();
	}

	private bool IsReady() {

		
		// Select conflict type
		if (_conflictType == ConflictType.None) {
			QuestionPanelView.Instance.AskQuestion("What kind of battle?", new []{"Political", "Military"}, i => {
				_conflictType = (i == 0) ? ConflictType.Political : ConflictType.Military;
			});

			return false;
		}
		
		// Select element
		if (_elementType == ElementType.None) {
			_textMesh.text = "Element?";
			return false;
		}

		// Who is attacking?
		if (_attackingCharacter.Count == 0) {
			_textMesh.text = "Who Attacking?";
			return false;
		}
		
		// What is attacked?
		if (_province == null) {
			_textMesh.text = "What Attacking?";
			return false;
		}

		_textMesh.text = "start conflict";
		return true;
	}
	
}
