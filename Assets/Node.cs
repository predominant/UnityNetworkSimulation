using UnityEngine;
using System.Collections;

public class Node : NetBehaviour
{
    public enum NodeType
    {
        Node,
        Switch,
    }

    public int ports = 1;
    public NodeType Type = NodeType.Node;
    public NetworkSim.Node _node;

    public new void Awake()
    {
        base.Awake();
        switch (this.Type)
        {
            case NodeType.Node:
                this._node = this._simulation.CreateNode<NetworkSim.Node>(this.ports);
                break;
            case NodeType.Switch:
                this._node = this._simulation.CreateNode<NetworkSim.Switch>(this.ports);
                break;
        }
    }

    public void Broadcast()
    {
        this._node.Broadcast(
            new NetworkSim.Packets.Packet
            {
                L2Destination = NetworkSim.Packets.Packet.L2BroadcastDestination
            }
        );
    }
}
