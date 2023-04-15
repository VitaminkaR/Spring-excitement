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

    // ��� ������
    [SerializeField] private QuestAgentType _questAgentType;

    // ��������� ������
    public string QuestName;
    [Header("�������� ��� Customer")]
    public UnityEvent QuestRewardMethod;
    public string AdditionalQuestData;

    // ��������� ��� ��������
    // ����� �� ������� ����� ���� ��� ����� ��� ����
    [Header("�������� ��� Bring Item")] [SerializeField] private bool _bringDeleted;

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
            // ���� ��� ���� ���� ����� �� ��� ��� ��� �� ����
            if (currentQuest == null)
            {
                // ���� ����� �����
                Quest quest = new Quest();
                quest.QuestName = QuestName;
                quest.QuestRewardMethod = QuestRewardMethod;
                quest.AdditionalQuestData = AdditionalQuestData;
                col.gameObject.GetComponent<QuestManager>().quests.Add(quest);
            }
            else
            {
                // �������� ���� ������� ����
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
            // ����� ���� �������
            List<Quest> quests = col.gameObject.GetComponent<QuestManager>().quests;
            Quest q = quests.Find((Quest q) => q.QuestName == QuestName);
            if(q != null)
            {
                q.AdditionalQuestData = "Bringed";

                // ������� ������� ���� ��� ����������
                if (_bringDeleted)
                    Destroy(gameObject);
            }
        }
    }
}
