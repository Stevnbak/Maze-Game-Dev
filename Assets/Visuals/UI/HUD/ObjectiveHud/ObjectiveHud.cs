using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveHud : MonoBehaviour
{
    ObjectiveCounter objCounter;
    public GameObject uiTemplate;
    public Transform parent;
    public ObjectiveCounter.ObjectiveEntry[] objectiveList;
    public List<ObjectiveHUDElement> uiElements;

    private void Start()
    {
        objCounter = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectiveCounter>();
    }

    void Update()
    {
        objectiveList = objCounter.objectives;
        for (int i = 0; i < objectiveList.Length; i++)  {
            if (i >= uiElements.ToArray().Length) createUIItem(i);
            uiElements[i].objName = objectiveList[i].name;
            uiElements[i].count = objectiveList[i].count;
            uiElements[i].goal = objectiveList[i].goal;
            uiElements[i].completed = objectiveList[i].completed;
        }
    }

    void createUIItem(int index)
    {
        GameObject uiItem = Instantiate(uiTemplate, parent);
        ObjectiveHUDElement script = uiItem.GetComponent<ObjectiveHUDElement>();
        uiElements.Add(script);
        uiItem.transform.localPosition = new Vector3(0, 125 - (index * 90), 0);
        script.objName = objectiveList[index].name;
        script.count = objectiveList[index].count;
        script.goal = objectiveList[index].goal;
        script.completed = objectiveList[index].completed;
    }
}
