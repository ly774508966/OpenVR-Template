using UnityEngine;
using System.Collections;

public class Network_GameLogic : Photon.MonoBehaviour
{
  public GameObject PlayerPrefab;
  bool joinedLobby = false;

  void Awake()
  {
    //Connect to the main photon server. This is the only IP and port we ever need to set(!)
    if (!PhotonNetwork.connected)
      PhotonNetwork.ConnectUsingSettings("v1.0"); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)

    //Load name from PlayerPrefs
    PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));
    
  }

  IEnumerator Start()
  {
    Status.Set("Connecting");
    while (!PhotonNetwork.connected)
    {
      // wait until connected
      Status.Append(".", Status.Blip.BLIP);
      yield return new WaitForSeconds(0.2f);
    }
    Status.Set("connected", Status.Blip.GOOD);


    Status.Set("waiting to enter lobbby");
    while(!PhotonNetwork.insideLobby)
    {
      Status.Append(".", Status.Blip.BLIP);
      yield return new WaitForSeconds(0.2f);
    }
    Status.Set("connected", Status.Blip.GOOD);

    // find how many room there are
    if (PhotonNetwork.GetRoomList().Length == 0)
    {
      // create main room
      PhotonNetwork.CreateRoom("Main", new RoomOptions() { maxPlayers = 4 }, TypedLobby.Default);
    }
    else
    {

      // join main room
      PhotonNetwork.JoinRoom("Main");
    }
    
    
  }

  void OnJoinedRoom()
  {
    Debug.Log("Joined room");
    StartGame();
  }


  IEnumerator OnLeftRoom()
  {
    //Easy way to reset the level: Otherwise we'd manually reset the camera

    //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
    while (PhotonNetwork.room != null || PhotonNetwork.connected == false)
      yield return 0;

    Debug.Log("OnLeftRoom");

  }

  void StartGame()
  {
    // Spawn our local player
    //PhotonNetwork.Instantiate(PlayerPrefab.name, transform.position, Quaternion.identity, 0);
  }

  void OnDisconnectedFromPhoton()
  {
    Debug.LogWarning("OnDisconnectedFromPhoton");
  }

  public void OnConnectedToMaster()
  {
    // this method gets called by PUN, if "Auto Join Lobby" is off.
    // this demo needs to join the lobby, to show available rooms!

    PhotonNetwork.JoinLobby();  // this joins the "default" lobby
  }
}