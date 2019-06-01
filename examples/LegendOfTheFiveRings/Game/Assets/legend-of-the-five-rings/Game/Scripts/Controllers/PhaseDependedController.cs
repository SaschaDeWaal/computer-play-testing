using UnityEngine;

public abstract class PhaseDependedController<T> : BaseController where T : BasePhase {

	protected Game CurGame => Game.Instance;
	protected T CurPhase => CurGame.PhaseManager.CurrentPhase as T;
	
	public PhaseDependedController() : base() {
		
	}

	protected virtual bool CanBeExecutedWithCorrectPhase() {
		return false;
	}
	
	public override bool CanBeExecuted() {
				
		if (CurPhase?.GetType() == typeof(T)) {
			return CanBeExecutedWithCorrectPhase();
		}
		
		return false;
	}

}

public abstract class PhaseDependedController<T, U> : BaseController where T : BasePhase where U : BasePhase {
	protected Game CurGame => Game.Instance;

	protected F CurPhase<F>() where F : BasePhase {
		return Game.Instance.PhaseManager.CurrentPhase as F;
	}

	public PhaseDependedController() : base() {
		
	}

	protected virtual bool CanBeExecutedWithCorrectPhase() {
		return false;
	}
	
	public override bool CanBeExecuted() {

		if (Game.Instance.PhaseManager.CurrentPhase is T || Game.Instance.PhaseManager.CurrentPhase is U) {
			return CanBeExecutedWithCorrectPhase();
		} else {
			return false;
		}
		
	}
	
}