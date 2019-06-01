using System.Runtime.Serialization;

[DataContract]
public class Laser : BaseWeapon {
	public override string Name => "Laser";
	public override int Power => 8;
	public override  int LifeTime => 2;
}
