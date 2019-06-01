using System.Net.Sockets;
using System.Text;
using ComputerPlayTesting;
using UnityEngine;

public class FiveRingsManager : PlayTestManager<FiveRingsTester, FiveRingsGameStatus, FiveRingsPlayTestObserver> {

	protected TcpClient Client = new TcpClient();
	protected NetworkStream ClientStream;
	
	private string _host;
	private int _port;
	
	public void PrepareConnection(string hostname, int port) {
		_host = hostname;
		_port = port;
		ConnectToServer(hostname, port);
	}

	public override void Tick() {
		
		if (!Client.Connected) {
			Debug.LogWarning($"Lost connection to {_host}:{_port} server, trying to reconnect...");
			ConnectToServer(_host, _port);
			return;
		}
		
		base.Tick();
	}

	protected override void CreateObjects(string testId) {		
		PlayTesters  = new FiveRingsTester[]{new FiveRingsTester(), new FiveRingsTester() };
		GameStatus        = new FiveRingsGameStatus();
		PlayTestObserver  = new FiveRingsPlayTestObserver();
	}


	protected override void SaveResult(string data) {
		SendData("insert", data);
	}
	
	private void ConnectToServer(string host, int port) {
		Debug.Log($"Connect to {host}:{port}");
			
		Client = new TcpClient();
		Client.Connect(host, port);
		ClientStream = Client.GetStream();
			
		Debug.Log($"Playtest id is {playtestID.ToLower()}");
		SendData("startTest", $"{{\"id\": \"{playtestID}\"}}");
	}
	
	private void SendData(string action, string data) {
		byte[] text = Encoding.ASCII.GetBytes($"{{\"action\": \"{action}\", \"data\": {data}}};");
		ClientStream.Write(text, 0, text.Length);
	}
}
