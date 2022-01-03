
using UnityEngine;
public interface IGameController
{
    float objectivesCompleted { get; set; }
    float objectivesTotal { get; set; }
    float time { get; set; }
    bool isGameRunning { get; set; }
    void StartGame();
    void EndGame();
}
