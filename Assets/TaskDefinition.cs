using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName="Simulation/TaskDefinition")]
public class TaskDefinition : ScriptableObject {
    public ETaskType taskType;
    public List<RouteDefinition> possibleRoutes;
    public float duration;
    public bool needsWeaponPickup;
    public bool needsEquipmentPickup;
    public EDeployment deploymentLocation;
    public ELifecyclePhase phase;
    [TextArea]
    public string description;
}