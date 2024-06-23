using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Place
{
    public Place(int x, int y, bool b)
    {
        posX = x;
        posY = y;
        isHere = b;
    }

    public int posX;
    public int posY;

    public bool isHere;

    // Equals �޼��� ������
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Place other = (Place)obj;
        return posX == other.posX && posY == other.posY;
    }

    // GetHashCode �޼��� ������
    public override int GetHashCode()
    {
        return HashCode.Combine(posX, posY);
    }

    // == ������ �����ε�
    public static bool operator ==(Place p1, Place p2)
    {
        if (ReferenceEquals(p1, null))
        {
            return ReferenceEquals(p2, null);
        }
        return p1.Equals(p2);
    }

    // != ������ �����ε�
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

    // ��� �߰� �޼���
    public void AddNode(Place node)
    {
        if (!adjacencyList.ContainsKey(node))
        {
            adjacencyList[node] = new List<Place>();
        }
    }

    // ���� �߰� �޼���
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

    // ��忡 ����� �̿� ���� ��ȯ
    public List<Place> GetNeighbors(Place node)
    {
        if (adjacencyList.ContainsKey(node))
        {
            return adjacencyList[node];
        }
        return new List<Place>();
    }

    // �׷��� ��� (������)
    public void PrintGraph()
    {
        foreach (var node in adjacencyList)
        {
            string neighbors = string.Join(", ", node.Value);
            Debug.Log(node.Key + ": " + neighbors);
        }
    }

    //��ü ���� ��ȯ
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

    //�׷��� ����
    public void ResetGraph()
    {
        adjacencyList.Clear();
    }
}
