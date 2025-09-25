using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour {
    public static TaskManager Instance { get; private set; }
    public List<TaskDefinition> allTasks;

    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public TaskDefinition PickTask(ELifecyclePhase phase, SoldierData data) {
        var list = allTasks
            .Where(t => t.phase == phase)
            .ToList();
        return list[Random.Range(0, list.Count)];
    }
}