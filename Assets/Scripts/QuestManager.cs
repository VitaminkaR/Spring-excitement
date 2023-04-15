using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// накладывается на игрока, хранит все квесты и функции наград к ним
public class QuestManager : MonoBehaviour
{
    private Player _player;

    // список всех квестов
    public List<Quest> quests;

    private void Start()
    {
        _player = GetComponent<Player>();
        quests = new List<Quest>();
    }

    public void RewardIpaitArtefact()
    {
        Debug.Log("QUEST COMPLETED");
    }
}
