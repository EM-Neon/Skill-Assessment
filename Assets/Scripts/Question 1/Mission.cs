using UnityEngine;
using UnityEngine.Events;

// Define different types of missions
public enum MissionType
{
    BoxHit,
    EnemyDeath,
    LevelComplete
}

[CreateAssetMenu(fileName = "Mission", menuName = "Scriptable Objects/Mission")]
public class Mission : ScriptableObject
{
    public MissionType missionType;
    public int missionProgress;

    public int missionID;
}

public class MissionInstance
{
    public Mission mission;
    public int progress;
    public bool isComplete = false; 

    public MissionInstance(Mission mission)
    {
        this.mission = mission;
        this.progress = 0;
    }
}

