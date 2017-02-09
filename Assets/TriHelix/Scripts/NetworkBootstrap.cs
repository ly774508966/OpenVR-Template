using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class NetworkBootstrap : MonoBehaviour {


	
	public string appId;
	public string clientScene;
	public string serverScene;
    void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Failed to create room." + codeAndMsg[0] + "-" + (string)codeAndMsg[1]);
    }
    IEnumerator Start () {
		yield return null;
		PhotonNetwork.PhotonServerSettings.AppID = appId;		

		if (ServerDiscovery.Instance.isServer) {
			Status.Set("Initializing Server");
			//Choose which machine to run the server on
			PhotonNetwork.PhotonServerSettings.ServerAddress = "127.0.0.1";
//			PhotonNetwork.PhotonServerSettings.ServerAddress = "192.168.0.4";
			PhotonNetwork.PhotonServerSettings.ServerPort = ServerDiscovery.Instance.serverPort;
			PhotonNetwork.ConnectUsingSettings("dev");

			while (!PhotonNetwork.connectedAndReady) {
				Status.Append(".", Status.Blip.BLIP);
				yield return new WaitForSeconds(0.2f);
			}

			Status.Append("Done", Status.Blip.GOOD);
            PhotonNetwork.playerName = "MasterClient";
            PhotonNetwork.JoinLobby();

            while(PhotonNetwork.insideLobby == false)
                yield return null;

            if (PhotonNetwork.GetRoomList().Any(a => a.name == "Main"))
                Debug.LogError("The Masterclient cannot create the game room, ensure there are no other masterclients or there are no clients still connected in standby");

            while (PhotonNetwork.GetRoomList().Any(a => a.name == "Main"))
            {
                //Debug.Log(PhotonNetwork.GetRoomList().Aggregate("", (a, b) => a + b.name + "(Open:" + b.open + " PlayerCount:" + b.playerCount + " MaxPlayers:" + b.maxPlayers + ");"));
                yield return new WaitForSeconds(0.2f);
            }


            PhotonNetwork.CreateRoom("Main");

            while (PhotonNetwork.inRoom == false)
                yield return null;


			SceneManager.LoadScene(serverScene);
			yield break;
		}
        
        //android Wi-Fi sleep work around
        ServerDiscovery.Instance.enableBroadcast = true;

		while (ServerDiscovery.Instance.ServerCount == 0) {
			Status.Append(".", Status.Blip.BLIP);
			yield return new WaitForSeconds(0.2f);
		}

		var info = ServerDiscovery.Instance.AnyServer;
		Status.Set("Attempting to connect to " + info.GetUniqueName(), Status.Blip.GOOD);
		PhotonNetwork.PhotonServerSettings.ServerAddress = info.address.ToString();
		PhotonNetwork.PhotonServerSettings.ServerPort = info.port;

		PhotonNetwork.playerName = SystemInfo.deviceUniqueIdentifier;
		#if UNITY_EDITOR || UNITY_STANDALONE
			PhotonNetwork.playerName = "PCPlayer";
		#endif
		PhotonNetwork.ConnectUsingSettings("dev");

		while (!PhotonNetwork.connectedAndReady) {
			Status.Append(".", Status.Blip.BLIP);
			yield return new WaitForSeconds(0.2f);
		}

		PhotonNetwork.JoinRandomRoom();

		Status.Set("Joining");
		while (!PhotonNetwork.inRoom) {
			Status.Append(".", Status.Blip.BLIP);
			yield return new WaitForSeconds(0.2f);
		}

		Status.Append("Done", Status.Blip.GOOD);
		
		SceneManager.LoadScene(clientScene);
	}

	void OnJoinedRoom () {

        if (!ServerDiscovery.Instance.isServer)
        {
            PhotonNetwork.isMessageQueueRunning = false;

        } else
        {

            PhotonGameSettings.BuildDevicePairs();
        }
        ServerDiscovery.Instance.enableBroadcast = false;
    }


}
