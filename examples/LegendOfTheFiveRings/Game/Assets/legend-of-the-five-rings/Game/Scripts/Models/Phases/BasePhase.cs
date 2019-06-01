using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

[DataContract]
[KnownType("DerivedTypes")]
public abstract class BasePhase {

	protected Game CurGame => Game.Instance;
	protected PhaseManager PhaseManager => Game.Instance.PhaseManager;
	
	public BasePhase(Game curGame, PhaseManager phaseManager) {		
		Game.OnGameUpdated += new GameUpdatedEventHandler(OnGameUpdated);
	}

	public virtual void OnStartPhase() {
		
	}

	public virtual void OnGameUpdated(ChangeEvent changeEvent) {
		
	}

	public virtual void OnEndPhase() {
		Game.OnGameUpdated -= new GameUpdatedEventHandler(OnGameUpdated);
	}

	public virtual void OnResumePhase(Game game) {
		Game.OnGameUpdated += new GameUpdatedEventHandler(OnGameUpdated);
	}
	
	private static Type[] DerivedTypes() {
		return Assembly.GetExecutingAssembly().GetTypes().Where(_ => _.IsSubclassOf(typeof(BasePhase))).ToArray();
	}

}
