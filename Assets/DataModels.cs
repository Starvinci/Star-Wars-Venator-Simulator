public enum EUnitType    { StandardTrooper, Pilot, Officer, CargoUnit }
public enum ERoleInGroup { Commander, Unit, Support, Backup }
public enum ETaskType    { DeliverItem, CatchShip, PatrolMobile, PatrolStatic, ReactorCheck, Training }
public enum EDeployment  { Kommandozentrale1, Kommandozentrale2, MainHangar, HangarLeft, HangarRight, HangarLow, ControlRoom, ReactorRoom, TrainingStation }
public enum EIdleState   { Canteen, Quarters }
public enum ELifecyclePhase { Work, Rest }

[System.Serializable]
public class SoldierData {
    public EUnitType unitType;
    public ERoleInGroup role;
    public TaskDefinition currentTask;
    public RouteDefinition currentRoute;
    public ELifecyclePhase currentPhase;
    public EIdleState idleState;
    public int health = 100;
    public int experience = 0;
    public float hunger = 0f;
    public float phaseStartTime;
}