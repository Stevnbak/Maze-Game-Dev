using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveCounter : MonoBehaviour
{
    //public List<KeyValuePair<string, float>> objectives = new List<KeyValuePair<string, float>>();
    [System.Serializable]
    public class ObjectiveEntry
    {
        public string name;
        public float count = 0;
        public float goal = 0;
        public bool completed;
    }

    public ObjectiveEntry[] objectives;
    IGameController gameController;

    private void Start()
    {
        gameController = GetComponent<IGameController>();
    }

    private void Update()
    {
        gameController.objectivesTotal = objectives.Length;
        foreach (var entry in objectives)
        {
            if(entry.count >= entry.goal && !entry.completed)
            {
                gameController.objectivesCompleted += 1;
                entry.completed = true;
            }
        }
    }

    public void countObjective(string type)
    {
        bool foundObjective = false;
        foreach (var entry in objectives)
        {
            if(entry.name == type)
            {
                entry.count += 1;
                foundObjective = true;
            }
        }
        if (!foundObjective) Debug.LogError("Objective named " + type + " wasn't found");
    }

    public void addObjective(string type)
    {
        bool foundObjective = false;
        foreach (var entry in objectives)
        {
            if (entry.name == type)
            {
                entry.goal += 1;
                foundObjective = true;
            }
        }
        if (!foundObjective) Debug.LogError("Objective named " + type + " wasn't found");
    }
}
