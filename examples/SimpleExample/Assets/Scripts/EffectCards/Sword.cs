using System.Runtime.Serialization;

[DataContract]
public class Sword : BaseWeapon {
	public override string Name => "Sword";
	public override int Power => 5;
	public override  int LifeTime => 5;
}
