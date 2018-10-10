using UnityEngine;
using System.Collections;

public class NetBehaviour : MonoBehaviour
{
    protected NetworkSim.Simulation _simulation;
    public MeshRenderer TickIndicator;

    public void Awake()
    {
        this._simulation = this.transform.parent.GetComponent<Network>().Simulation;
    }

    public void Update()
    {
        if (this.TickIndicator != null)
            this.TickIndicator.material.color = (this._simulation.TickIndex / 50) % 2 == 0 ? Color.green : Color.blue;
    }
}
