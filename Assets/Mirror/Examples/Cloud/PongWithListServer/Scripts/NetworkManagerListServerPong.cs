using UnityEngine;

namespace Mirror.Cloud.Example
{
    public sealed class NetworkManagerListServerPong : NetworkManagerListServer
    {
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            Debug.Assert(startPositions.Count == 2, "Pong Scene should have 2 start Poitions");

            GameObject player = InstantiatePlayer();

            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }
}
