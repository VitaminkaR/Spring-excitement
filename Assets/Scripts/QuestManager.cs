using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������������� �� ������, ������ ��� ������ � ������� ������ � ���
public class QuestManager : MonoBehaviour
{
    private Player _player;

    // ������ ���� �������
    public List<Quest> Quests;

    private void Start()
    {
        _player = GetComponent<Player>();
        Quests = new List<Quest>();
    }

    public void RewardIpaitArtefact()
    {
        Debug.Log("QUEST COMPLETED");
    }
}
