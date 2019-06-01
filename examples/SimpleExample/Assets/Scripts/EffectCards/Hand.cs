using System.Runtime.Serialization;

[DataContract]
public class Hand : BaseWeapon {
	public override string Name => "Hand";
	public override int Power => 1;
	public override  int LifeTime => 8;
}
