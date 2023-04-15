using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class QuestAgent : MonoBehaviour
{
    enum QuestAgentType
    {
        customer,
        bringItem,
    }

    private QuestManager _questManager;

    // тип агента
    [SerializeField] private QuestAgentType _questAgentType;

    // настройки квеста
    public string QuestName;
    public string QuestDescription;
    [Header("Настроки для Customer")]
    public UnityEvent QuestRewardMethod;
    public string AdditionalQuestData;

    // настройки для предмета
    // нужно ли удалять после того как игрок его взял
    [Header("Настроки для Bring Item")] 
    [SerializeField] private bool _bringDeleted;

    // UI
    [Header("UI")]
    [SerializeField] private GameObject _uiQuest;

    private void OnCollisionEnter(Collision collision)
    {
        if (_questAgentType == QuestAgentType.customer)
            CustomerAgentCollision(collision);
        else if (_questAgentType == QuestAgentType.bringItem)
            BringItemAgentCollision(collision);
    }

    private void CustomerAgentCollision(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if(_questManager == null)
                _questManager = col.gameObject.GetComponent<QuestManager>();

            Quest currentQuest = _questManager.Quests.Find((Quest q) => q.QuestName == QuestName);
            // если уже есть этот квест то еще раз его не даем
            if (currentQuest == null)
            {
                // UI
                _uiQuest.SetActive(true);
                _uiQuest.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = QuestName;
                _uiQuest.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = QuestDescription;

                _uiQuest.transform.GetChild(4).gameObject.GetComponent<Button>().onClick.AddListener(Accept);
                _uiQuest.transform.GetChild(5).gameObject.GetComponent<Button>().onClick.AddListener(Dismiss);
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

    public void Accept()
    {
        if(TryGetComponent<Light>(out Light component))
            Destroy(component);

        // даем новый квест
        Quest quest = new Quest();
        quest.QuestName = QuestName;
        quest.QuestDescription = QuestDescription;
        quest.QuestRewardMethod = QuestRewardMethod;
        quest.AdditionalQuestData = AdditionalQuestData;
        _questManager.Quests.Add(quest);
        Dismiss();
    }

    public void Dismiss()
    {
        _uiQuest.transform.GetChild(4).gameObject.GetComponent<Button>().onClick.RemoveListener(Accept);
        _uiQuest.transform.GetChild(5).gameObject.GetComponent<Button>().onClick.RemoveListener(Dismiss);
        _uiQuest.SetActive(false);
    }

    private void BringItemAgentCollision(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // игрок взял предмет
            List<Quest> quests = col.gameObject.GetComponent<QuestManager>().Quests;
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
