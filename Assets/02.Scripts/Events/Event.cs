using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game Event", order = 51)]
public class Event : ScriptableObject
{
    [TextArea]
    public string description; // 이벤트 설명 텍스트
    public List<Choice> choices;
    [TextArea]
    public List<string> resultTexts;

    [Serializable]
    public class Choice
    {
        public string choiceText;
        public UnityEvent action;
    }
}