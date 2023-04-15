using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestAgent : MonoBehaviour
{
    enum QuestAgentType
    {
        customer,
        bringItem,
    }

    // тип агента
    [SerializeField] private QuestAgentType _questAgentType;

    // настройки квеста
    public string QuestName;
    [Header("Настроки для Customer")]
    public UnityEvent QuestRewardMethod;
    public string AdditionalQuestData;

    // настройки для предмета
    // нужно ли удалять после того как игрок его взял
    [Header("Настроки для Bring Item")] [SerializeField] private bool _bringDeleted;

    private void Update()
    {
        if (_questAgentType == QuestAgentType.customer)
            CustomerAgent();
        else if (_questAgentType == QuestAgentType.bringItem)
            BringItemAgent();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_questAgentType == QuestAgentType.customer)
            CustomerAgentCollision(collision);
        else if (_questAgentType == QuestAgentType.bringItem)
            BringItemAgentCollision(collision);
    }

    private void CustomerAgent()
    {

    }

    private void BringItemAgent()
    {

    }

    private void CustomerAgentCollision(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Quest currentQuest = col.gameObject.GetComponent<QuestManager>().quests.Find((Quest q) => q.QuestName == QuestName);
            // если уже есть этот квест то еще раз его не даем
            if (currentQuest == null)
            {
                // даем новый квест
                Quest quest = new Quest();
                quest.QuestName = QuestName;
                quest.QuestRewardMethod = QuestRewardMethod;
                quest.AdditionalQuestData = AdditionalQuestData;
                col.gameObject.GetComponent<QuestManager>().quests.Add(quest);
            }
            else
            {
                // проверка если предмет взят
                if (currentQuest.AdditionalQuestData == "Bringed")
                {
                    currentQuest.GiveReward();
                    Destroy(this);
                }
            }
        }
    }

    private void BringItemAgentCollision(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // игрок взял предмет
            List<Quest> quests = col.gameObject.GetComponent<QuestManager>().quests;
            Quest q = quests.Find((Quest q) => q.QuestName == QuestName);
            if(q != null)
            {
                q.AdditionalQuestData = "Bringed";

                // удалает предмет если это необходимо
                if (_bringDeleted)
                    Destroy(gameObject);
            }
        }
    }
}
