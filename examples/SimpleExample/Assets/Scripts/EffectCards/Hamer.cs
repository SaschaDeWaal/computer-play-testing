using System.Runtime.Serialization;

[DataContract]
public class Hamer : BaseWeapon {
	public override string Name => "Hamer";
	public override int Power => 1;
	public override  int LifeTime => 12;
}
