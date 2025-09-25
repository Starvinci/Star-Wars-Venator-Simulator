using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class SoldierController : MonoBehaviour {
    public SoldierData data;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private int waypointIndex;

    [Header("Player Control")]
    public bool isPlayerControlled = false;
    public MonoBehaviour[] playerControlScripts; // e.g. MouseLook, CharacterMovement, PlayerInteraction
    public Camera playerCamera;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        // ensure upright orientation always
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        ApplyControlMode(isPlayerControlled);
    }

    void Start() {
        if (!isPlayerControlled)
            StartPhase(ELifecyclePhase.Work);
    }

    void Update() {
        if (isPlayerControlled) return;
        // AI behavior
        if (data.currentTask == null) return;
        float elapsed = Time.time - data.phaseStartTime;
        if (data.currentPhase == ELifecyclePhase.Work) {
            data.hunger += Time.deltaTime * 0.1f;
            data.experience += Mathf.FloorToInt(Time.deltaTime * 1f);
        }
        if (elapsed >= data.currentTask.duration) {
            EndTask();
            return;
        }
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            MoveNext();
        }
    }

    public void StartPhase(ELifecyclePhase phase) {
        if (isPlayerControlled) return;
        data.currentPhase = phase;
        data.phaseStartTime = Time.time;
        var task = TaskManager.Instance.PickTask(phase, data);
        AssignTask(task);
    }

    void AssignTask(TaskDefinition task) {
        data.currentTask = task;
        data.currentRoute = task.possibleRoutes[Random.Range(0, task.possibleRoutes.Count)];
        data.idleState = (task.phase == ELifecyclePhase.Rest) ? EIdleState.Quarters : EIdleState.Canteen;
        waypointIndex = 0;
        HandlePickup(task);
        MoveNext();
    }

    void HandlePickup(TaskDefinition task) {
        if (data.currentPhase == ELifecyclePhase.Work) {
            if (task.needsWeaponPickup) GoToTag("WeaponStation");
            if (task.needsEquipmentPickup) GoToTag("EquipmentStation");
        }
    }

    void GoToTag(string tag) {
        var obj = GameObject.FindWithTag(tag);
        if (obj) agent.SetDestination(obj.transform.position);
    }

    void MoveNext() {
        if (data.currentRoute == null || data.currentRoute.waypoints.Count == 0) return;
        agent.SetDestination(data.currentRoute.waypoints[waypointIndex].position);
        waypointIndex = (waypointIndex + 1) % data.currentRoute.waypoints.Count;
    }

    void EndTask() {
        data.currentTask = null;
        if (data.currentPhase == ELifecyclePhase.Work)
            StartPhase(ELifecyclePhase.Rest);
        else
            StartPhase(ELifecyclePhase.Work);
    }

    void OnValidate() {
        // respond to inspector toggle
        ApplyControlMode(isPlayerControlled);
    }

    void ApplyControlMode(bool playerControl) {
        isPlayerControlled = playerControl;
        // NavMeshAgent
        agent.enabled = !playerControl;
        agent.updatePosition = !playerControl;
        agent.updateRotation = !playerControl;
        
        if (playerControl) {
            // Player mode: physics-driven
            rb.isKinematic = false;
            rb.useGravity = true;
        } else {
            // AI mode: kinematic so NavMesh moves it, gravity off
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        
        // ensure rotation constraints always
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // toggle player control scripts
        foreach (var script in playerControlScripts) {
            if (script != null)
                script.enabled = playerControl;
        }
        // toggle camera
        if (playerCamera != null)
            playerCamera.enabled = playerControl;
    }
}