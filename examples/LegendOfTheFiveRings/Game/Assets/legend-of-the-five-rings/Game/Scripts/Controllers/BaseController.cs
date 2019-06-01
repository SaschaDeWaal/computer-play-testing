
public abstract class BaseController {

	public virtual bool Execute() {
		return false;
	}

	public virtual bool CanBeExecuted() {
		return false;
	}

	protected void NextPlayer() {
		Game.Instance.EndTurn();
		Game.Instance.ApplyChanges(ChangeEvent.Empty);
	}

	protected bool IsTurn(Player player) {
		return Game.Instance.TurnIndex == player.Index;
	}

}
