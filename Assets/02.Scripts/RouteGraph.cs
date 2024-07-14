using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PlaceType
{
    Battle,
    Event,
    Elite,
    Rest,
    Shop,
    Treasure
}

[System.Serializable]
public class Place
{
    public Place(int x, int y, bool b)
    {
        posX = x;
        posY = y;
        isHere = b;

        SetPlaceType();
    }

    public int posX;
    public int posY;

    public bool isHere;
    public PlaceType placeType;

    void SetPlaceType()
    {
        if (posX == 1)
        {
            placeType = PlaceType.Battle;
        }
        else if (posX == 3)
        {
            placeType = PlaceType.Treasure;
        }
        else if (posX == 14)
        {
            placeType = PlaceType.Rest;
        }
        else if(posX == 5)
        {
            placeType = PlaceType.Shop;
        }
        else
        {
            //0.0 ~ 1.0 사이의 난수 생성
            float v = Random.value;
            //각 확률 정의
            float[] probabilities = { 0.45f, 0.22f, 0.16f, 0.12f, 0.5f };

            float cumulativeProbability = 0.0f;

            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulativeProbability += probabilities[i];

                if (v <= cumulativeProbability)
                {
                    placeType = (PlaceType)i;
                    break;
                }
            }
        }
    }

    // Equals 메서드 재정의
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Place other = (Place)obj;
        return posX == other.posX && posY == other.posY;
    }

    // GetHashCode 메서드 재정의
    public override int GetHashCode()
    {
        return HashCode.Combine(posX, posY);
    }

    // == 연산자 오버로딩
    public static bool operator ==(Place p1, Place p2)
    {
        if (ReferenceEquals(p1, null))
        {
            return ReferenceEquals(p2, null);
        }
        return p1.Equals(p2);
    }

    // != 연산자 오버로딩
    public static bool operator !=(Place p1, Place p2)
    {
        return !(p1 == p2);
    }
}

public class RouteGraph
{
    private Dictionary<Place, List<Place>> adjacencyList;

    public RouteGraph()
    {
        adjacencyList = new Dictionary<Place, List<Place>>();
    }

    // 노드 추가 메서드
    public void AddNode(Place node)
    {
        if (!adjacencyList.ContainsKey(node))
        {
            adjacencyList[node] = new List<Place>();
        }
    }

    // 에지 추가 메서드
    public void AddEdge(Place fromNode, Place toNode)
    {
        if (!adjacencyList.ContainsKey(fromNode))
        {
            AddNode(fromNode);
        }
        if (!adjacencyList.ContainsKey(toNode))
        {
            AddNode(toNode);
        }
        if (!adjacencyList[fromNode].Contains(toNode))
        {
            adjacencyList[fromNode].Add(toNode);
        }
    }

    // 노드에 연결된 이웃 노드들 반환
    public List<Place> GetNeighbors(Place node)
    {
        if (adjacencyList.ContainsKey(node))
        {
            return adjacencyList[node];
        }
        return new List<Place>();
    }

    // 그래프 출력 (디버깅용)
    public void PrintGraph()
    {
        foreach (var node in adjacencyList)
        {
            string neighbors = string.Join(", ", node.Value);
            Debug.Log(node.Key + ": " + neighbors);
        }
    }

    //전체 노드들 반환
    public List<Place> GetNodes()
    {
        List<Place> nodes = new List<Place>();
        foreach (var node in adjacencyList)
        {
            nodes.Add(node.Key);
        }

        return nodes;
    }

    public Dictionary<Place, List<Place>> GetDictionary()
    {
        return adjacencyList;
    }

    //그래프 리셋
    public void ResetGraph()
    {
        adjacencyList.Clear();
    }
}
