using System.Runtime.Serialization;
using ComputerPlayTesting;

[DataContract]
public abstract class FiveRingsCardAction : PlayerAction {


	public virtual string GetCardId(FiveRingsGameStatus PlayTestData, int playerIndex) {
		return "";
	}

	public abstract bool IsRelevantForCard(int character);

}
