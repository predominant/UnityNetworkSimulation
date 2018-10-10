using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkSim;

public class Network : MonoBehaviour
{
    public Simulation Simulation = new Simulation();

    private void Awake()
    {
        ConsoleRedirector.Redirect();
        this.Simulation.Name = this.name;
    }

    private void FixedUpdate()
    {
        this.Simulation.Tick();
    }
}
