using UnityEngine;
using System.Collections.Generic;

public class GroupSpawner : MonoBehaviour {
    [System.Serializable]
    public class SpawnConfig {
        public EUnitType type;
        public EDeployment deployment;
        public int groupCount;
        public int unitsPerGroup;
    }
    public List<SpawnConfig> spawnConfigs;
    public GameObject soldierPrefab;

    void Start() {
        foreach (var cfg in spawnConfigs)
            for (int g = 0; g < cfg.groupCount; g++)
                for (int i = 0; i < cfg.unitsPerGroup; i++)
                    Spawn(cfg);
    }

    void Spawn(SpawnConfig cfg) {
        var quarters = GameObject.Find($"Quarters_{cfg.deployment}");
        Vector3 pos = quarters ? quarters.transform.position : Vector3.zero;
        var go = Instantiate(soldierPrefab, pos, Quaternion.identity, transform);
        var ctrl = go.GetComponent<SoldierController>();
        ctrl.data.unitType = cfg.type;
        ctrl.data.role = ERoleInGroup.Unit;
        GroupManager.Instance.allSoldiers.Add(ctrl);
    }
}