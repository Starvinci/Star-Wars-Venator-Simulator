using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName="Simulation/RouteDefinition")]
public class RouteDefinition : ScriptableObject {
    public string routeName;
    public List<Transform> waypoints;
}