using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GroupManager : MonoBehaviour {
    public static GroupManager Instance { get; private set; }
    public List<SoldierController> allSoldiers = new List<SoldierController>();
    public List<TaskDefinition> taskPool;

    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start() {
        foreach (var s in allSoldiers) Assign(s);
    }
    void Assign(SoldierController s) {
        var list = taskPool
            .Where(t => t.deploymentLocation.ToString() == s.data.currentTask?.deploymentLocation.ToString())
            .ToList();
        if (list.Count == 0) list = taskPool;
        s.StartPhase(s.data.currentPhase);
    }
    public void NotifyTaskFinished(SoldierController s) {
        Assign(s);
    }
}