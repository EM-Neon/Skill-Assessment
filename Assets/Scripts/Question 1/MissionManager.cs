using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private List<Mission> missions;
    private MissionInstance[] currentMissions = new MissionInstance[3];

    private int multiplier = 1;

    void Start()
    {
        if (PlayerPrefs.HasKey("Multiplier"))
            multiplier = PlayerPrefs.GetInt("Multiplier");

        if (PlayerPrefs.HasKey("Missions"))
        {
            for (int i = 0; i < currentMissions.Length; i++)
            {
                foreach (var mission in missions)
                {
                    if (mission.missionID == PlayerPrefs.GetInt("Mission " + i))
                    {
                        currentMissions[i] = new MissionInstance(mission);
                        currentMissions[i].progress = PlayerPrefs.GetInt("Mission Progress " + i);

                        SubscribeEvent(currentMissions[i]);
                    }
                }
            }
        }

        else
        {
            PlayerPrefs.SetInt("Missions", 1);

            // Select 3 random missions from the list
            List<Mission> missionsCopy = missions;
            for (int i = 0; i < 3; i++)
            {
                int randomIndex = Random.Range(0, missionsCopy.Count);
                currentMissions[i] = new MissionInstance(missionsCopy[randomIndex]);
                missionsCopy.RemoveAt(randomIndex);

                SubscribeEvent(currentMissions[i]);

                PlayerPrefs.SetInt("Mission " + i, currentMissions[i].mission.missionID);
                PlayerPrefs.SetInt("Mission Progress " + i, currentMissions[i].progress);
            }
        }
        
    }

    void SubscribeEvent(MissionInstance missionInstance)
    {
        switch (missionInstance.mission.missionType)
        {
            case MissionType.BoxHit:
                Events.OnBoxHit += () => UpdateMissionProgress(missionInstance);
                break;
            case MissionType.EnemyDeath:
                Events.OnEnemyDeath += () => UpdateMissionProgress(missionInstance);
                break;
            case MissionType.LevelComplete:
                Events.OnLevelComplete += () => UpdateMissionProgress(missionInstance);
                break;
        }
    }
    
    // Check mission progress on event invocation
    void UpdateMissionProgress(MissionInstance currentMission)
    {
        currentMission.progress++;
        PlayerPrefs.SetInt("Mission Progress " + System.Array.IndexOf(currentMissions, currentMission), currentMission.progress);
        // increase multiplier when all 3 missions completed
        if (currentMission.progress >= currentMission.mission.missionProgress)
        {
            currentMission.isComplete = true;
            Debug.Log("Mission Completed: " + currentMission.mission.missionType);
            foreach (var mission in currentMissions)
            {
                if (mission.isComplete == false)
                {
                    return;
                }
            }
            Debug.Log("All missions completed! Increase multiplier!");
            multiplier++;
            PlayerPrefs.SetInt("Multiplier", multiplier);
        }
    }
}
