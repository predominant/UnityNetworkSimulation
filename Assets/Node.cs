using UnityEngine;
using System.Collections;
using NetworkSim.Packets;

public class Node : NetBehaviour
{
    public enum NodeType
    {
        Node,
        Switch,
    }

    public int ports = 1;
    public NodeType Type = NodeType.Node;
    public NetworkSim.Devices.Node _node;
    public string IPAddress = "127.0.0.1";

    public new void Awake()
    {
        base.Awake();
        switch (this.Type)
        {
            case NodeType.Node:
                this._node = this._simulation.CreateNode<NetworkSim.Devices.Node>(this.ports);
                break;
            case NodeType.Switch:
                this._node = this._simulation.CreateNode<NetworkSim.Devices.Switch>(this.ports);
                break;
        }
        
        this._node.Ports[0].IPAddress.Add(this.IPAddress);
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

    public void Send(int nodeTarget)
    {
        var ipPart = nodeTarget + 10;
        this._node.Ports[0].Send(new Packet
        {
            L3Destination = $"10.0.0.{ipPart}",
            Size = 1000,
        });
    }
}
