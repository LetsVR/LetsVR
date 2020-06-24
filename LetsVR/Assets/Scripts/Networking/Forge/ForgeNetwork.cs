using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using LetsVR.XR.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LetsVR.XR.Networking.Forge
{
	public static class ForgeNetwork
	{
		static NetWorker Server { get; set; }
		static NetWorker Client { get; set; }

		static ushort ServerPort { get; set; }

		static string ServerAddress { get; set; }

		public static bool UseMainThreadManagerForRPCs { get; set; } = true;

		public static bool GetLocalNetworkConnections { get; set; } = false;

		public static bool UseTCP { get; set; } = true;

		public static bool IsServer { get { return Server != null; } }

		public static bool IsSessionStarted { get; private set; }

		public static int MaxConnections { get; set; } = 64;

		public static void Init()
		{
			Application.quitting += Disconnect;

			if (!UseTCP)
			{
				// Do any firewall opening requests on the operating system
				NetWorker.PingForFirewall(ServerPort);
			}

			if (UseMainThreadManagerForRPCs)
				Rpc.MainThreadRunner = MainThreadManager.Instance;

			if (GetLocalNetworkConnections)
			{
				NetWorker.RefreshLocalUdpListings(ServerPort);
			}
		}

		public static void StartStopServer(string host, ushort port, GameObject NetworkManager, bool useTCP = true, IUserAuthenticator serverUserAuthenticator = null, Action callback = null)
		{
			if (!IsSessionStarted)
			{
				ServerAddress = host;
				ServerPort = port;
				UseTCP = useTCP;

				Init();
				CreateServer(w =>
				{
					if (!CreateNetworkManager(w, NetworkManager))
					{
						return;
					}

					HandleObjectsDisconnection();

					callback?.Invoke();
				}, serverUserAuthenticator);
			}
			else
			{
				Disconnect();
			}
		}

		public static void StartStopClient(string host, ushort port, GameObject NetworkManager, bool useTCP = true, IUserAuthenticator clientUserAuthenticator = null, Action successCallback = null)
		{
			if (AppState.MultiuserName == "?")
			{
				AppState.MultiuserName = Utils.GenerateClientName();
			}

			if (!IsSessionStarted)
			{
				ServerAddress = host;
				ServerPort = port;
				UseTCP = useTCP;

				Init();
				CreateClient(w =>
				{
					if (!CreateNetworkManager(w, NetworkManager))
					{
						Debug.LogError($"[ForgeNetwork] Join - Could not create network manager!");
						return;
					}

					HandleObjectsDisconnection();

					successCallback?.Invoke();
				}, clientUserAuthenticator);
			}
			else
			{
				Disconnect();
				Debug.Log("Disconnecting client... Done!");

				successCallback?.Invoke();
			}
		}

		public static void Disconnect()
		{
			if (!IsSessionStarted)
				return;

			if (GetLocalNetworkConnections)
				NetWorker.EndSession();

			if (networkManager != null)
			{
				networkManager.Disconnect();
			}

			IsSessionStarted = false;
		}

		static void CreateClient(Action<NetWorker> connectedCallback, IUserAuthenticator clientUserAuthenticator = null)
		{
			Debug.Log("[ForgeNetwork] Starting client...");

			if (UseTCP)
			{
				Client = new TCPClient();

				if (clientUserAuthenticator != null)
					Client.SetUserAuthenticator(clientUserAuthenticator);

				if (((TCPClient)Client).Connect(ServerAddress, ServerPort))
					Debug.Log("[ForgeNetwork] TCP Client started on " + ServerAddress + ":" + ServerPort);
			}
			else
			{
				Client = new UDPClient();

				if (clientUserAuthenticator != null)
					Client.SetUserAuthenticator(clientUserAuthenticator);

				if (((UDPClient)Client).Connect(ServerAddress, ServerPort))
					Debug.Log("[ForgeNetwork] UDP Client started on " + ServerAddress + ":" + ServerPort);
			}

			IsSessionStarted = true;
			connectedCallback?.Invoke(Client);
		}

		static void CreateServer(Action<NetWorker> connectedCallback, IUserAuthenticator serverUserAuthenticator = null)
		{
			Debug.Log("[ForgeNetwork] Starting host...");

			if (UseTCP)
			{
				Server = new TCPServer(MaxConnections);

				if (serverUserAuthenticator != null)
					Server.SetUserAuthenticator(serverUserAuthenticator);

				((TCPServer)Server).Connect(ServerAddress, ServerPort);
				Debug.Log("[ForgeNetwork] TCP Server started on " + ServerAddress + ":" + ServerPort);
			}
			else
			{
				Server = new UDPServer(MaxConnections);

				if (serverUserAuthenticator != null)
					Server.SetUserAuthenticator(serverUserAuthenticator);

				((UDPServer)Server).Connect(ServerAddress, ServerPort);
				Debug.Log("[ForgeNetwork] UDP Server started on " + ServerAddress + ":" + ServerPort);
			}

			IsSessionStarted = true;
			connectedCallback?.Invoke(Server);
		}

		static NetworkManager networkManager = null;

		static bool CreateNetworkManager(NetWorker networker, GameObject networkManagerPrefab)
		{
			if (!networker.IsBound)
			{
				return false;
			}

			if (networkManager == null)
			{
				networkManager = GameObject.Instantiate(networkManagerPrefab).GetComponent<NetworkManager>();
			}

			// If we are using the master server we need to get the registration data
			networkManager.Initialize(networker, "", 0, null);

			// inform server we can create objects
			NetworkObject.Flush(networker);

			return true;
		}

		static void HandleObjectsDisconnection()
		{
			//Handle disconnection
			NetworkManager.Instance.Networker.playerDisconnected += (netplayer, sender) =>
			{
				MainThreadManager.Run(() =>
				{
					var objs = new List<NetworkObject>();
					//Loop through all players and find the player who disconnected
					foreach (var no in sender.NetworkObjectList)
					{
						if (no.Owner == netplayer)
						{
							objs.Add(no);
						}

						if (!(no is ControllerSyncNetworkObject))
							continue;

						var controller = (ControllerSyncNetworkObject)no;

						if (controller.AttachedBehavior is ControllerSync attachedBehavior)
						{
							if (attachedBehavior.networkObject.playerIdentifier == netplayer.Name?.Hash()) // identify player's controllers
							{
								objs.Add(no);
							}
						}
					}

					//Remove the actual network object outside of the foreach loop, as we would modify the collection at runtime elsewise. (could also use a return, too late)
					if (objs.Count != 0)
					{
						foreach (var networkObjectToDestroy in objs)
						{
							sender.NetworkObjectList.Remove(networkObjectToDestroy);
							networkObjectToDestroy.Destroy();
						}
					}

					if (sender.NetworkObjectList.Count == 0)
					{

					}
				});
			};
		}
	}
}
