using TMPro.EditorUtilities;

public class BasePlayerObjectPhaseDependedView<T> : BasePlayerObjectView where T : BasePhase {

	private bool _phaseEntered = false;
	protected T CurPhase => CurGame.PhaseManager.CurrentPhase as T;
	
	protected override void OnGameChanged(ChangeEvent changeEvent) {
		if (CurGame.PhaseManager.CurrentPhase is T) {
			if (!_phaseEntered) {
				_phaseEntered = true;
				OnPhaseEntered();
			}

			OnGameChangedInPhase(changeEvent);
		}else{
			if (_phaseEntered) {
				_phaseEntered = false;
				OnPhaseLeave();
			}
		}
	}

	public override void OnClicked() {
		if (CurGame.PhaseManager.CurrentPhase is T) {
			OnClickedWhileInPhase();
		}
	}
	
	protected virtual void OnGameChangedInPhase(ChangeEvent changeEvent) {
		
	}

	protected virtual void OnClickedWhileInPhase() {
		
	}

	protected virtual void OnPhaseEntered() {
		
	}
	
	protected virtual void OnPhaseLeave() {
		
	}
	
}
