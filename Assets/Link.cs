using UnityEngine;
using System;
using System.Collections.Generic;

public class Link : NetBehaviour
{
    public GameObject PacketTrackerPrefab;

    public Transform NodeAPosition;
    public Transform NodeBPosition;

    public Node NodeA;
    public int NodeAPort;
    public Node NodeB;
    public int NodeBPort;
    public ulong Rate = 10;

    public NetworkSim.Devices.Link _link;

    private bool _initialized = false;
    private Dictionary<Guid, Transform> _packetTrackers = new Dictionary<Guid, Transform>();

    public void FixedUpdate()
    {
        if (!this._initialized)
        {
            if (this.NodeA == null || this.NodeA._node == null || this.NodeB == null || this.NodeB._node == null)
                return;

            this._link = this._simulation.CreateLink(
                this.NodeA._node.Ports[this.NodeAPort],
                this.NodeB._node.Ports[this.NodeBPort]);
            this._link.Rate = this.Rate;
            this._initialized = true;
        }
    }

    public new void Update()
    {
        base.Update();

        var staleTrackers = new List<Guid>();
        foreach (var k in this._packetTrackers.Keys)
            staleTrackers.Add(k);

        // NodeA -> NodeB
        foreach (var p in this._link.BufferB)
        {
            if (p.Packet.SystemPacket)
                continue;
            var tracker = this.GetPacketTracker(p.Packet.Guid, this.NodeAPosition);
            tracker.position = Vector3.Lerp(
                this.NodeAPosition.position,
                this.NodeBPosition.position,
                p.Progress);
            staleTrackers.Remove(p.Packet.Guid);
        }

        // NodeB -> NodeA
        foreach (var p in this._link.BufferA)
        {
            if (p.Packet.SystemPacket)
                continue;
            var tracker = this.GetPacketTracker(p.Packet.Guid, this.NodeAPosition);
            tracker.position = Vector3.Lerp(
                this.NodeBPosition.position,
                this.NodeAPosition.position,
                p.Progress);
            staleTrackers.Remove(p.Packet.Guid);
        }

        foreach (var k in staleTrackers)
        {
            var g = this._packetTrackers[k];
            this._packetTrackers.Remove(k);
            GameObject.Destroy(g.gameObject);
        }
    }

    public Transform GetPacketTracker(Guid guid, Transform start)
    {
        if (!this._packetTrackers.ContainsKey(guid))
        {
            var packetTracker = this.CreatePacketTracker(guid, start);
            this._packetTrackers.Add(guid, packetTracker);
            return packetTracker;
        }
        return this._packetTrackers[guid];
    }

    public Transform CreatePacketTracker(Guid guid, Transform start)
    {
        var color = this.GuidToColor(guid);
        var g = GameObject.Instantiate(
            this.PacketTrackerPrefab,
            start.position,
            this.transform.rotation);
        g.transform.parent = this.transform;
        g.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = color;
        return g.transform;
    }

    public Color GuidToColor(Guid guid)
    {
        var guidStr = guid.ToString("N");
        var c1 = Convert.ToInt32(guidStr.Substring(0, 2), 16);
        var c2 = Convert.ToInt32(guidStr.Substring(2, 2), 16);
        var c3 = Convert.ToInt32(guidStr.Substring(4, 2), 16);
        return new Color((float)c1 / 255f, (float)c2 / 255f, (float)c3 / 255f);
    }
}
