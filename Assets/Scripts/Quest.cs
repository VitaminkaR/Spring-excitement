using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Quest
{
    public string QuestName;
    public bool IsCompleted;
    public UnityEvent QuestRewardMethod;
    public string AdditionalQuestData;

    public void GiveReward()
    {
        IsCompleted = true;
        QuestRewardMethod.Invoke();
    }
}
