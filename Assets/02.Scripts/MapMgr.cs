using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class MapMgr : MonoBehaviour
{
    public int routeWidth;
    public int routeDepth;
    public int routeAmount;
    public int nodeSpacingX = 200;
    public int nodeSpacingY = 80;

    public GameObject placePref;

    public Transform mapContent;

    public List<Place> availableNode = new List<Place>();

    public GameObject linePref;

    public RectTransform mapScroll;

    void Start()
    {
        if(PlayData.routeGraph == null)
        {
            PlayData.routeGraph = MakeGraph();
        }
        SetAvailableNode();
        GenerateMap();

        SetCurrentMapPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //routeDepth��ŭ y�� ��ǥ���� �ִ� ����Ʈ (list[3] == x=3��ǥ�� y��ǥ)
    List<int> MakeRoute()
    {
        List<int> route = new List<int>();
        route.Add(Random.Range(0, routeWidth));

        for (int i = 0; i < routeDepth; i++)
        {
            int r;
            if (route[i] == 0)
            {
                r = Random.Range(0, 2);
            }
            else if (route[i] == routeWidth - 1)
            {
                r = Random.Range(-1, 1);
            }
            else
            {
                r = Random.Range(-1, 2);
            }
            route.Add(route[i] + r);
        }
        return route;
    }

    //��Ʈ�� ������ ����� �ϳ��� �׷����� ����
    RouteGraph MakeGraph()
    {
        RouteGraph graph = new RouteGraph();
        for (int i = 0; i < routeAmount; i++)
        {
            //��Ʈ ����
            List<int> route = MakeRoute();
            //��������Ʈ�� ���� ���� ���, ���� ���� ���
            Place preNode = new Place(0, 0, false);

            //��Ʈ�� �ϳ��� �׷����� �߰�
            //���� ������ġ�� x=0���� �ϱ� ���� 1����
            for (int j = 1; j < route.Count; j++)
            {
                Place placeNode = new Place(j, route[j], false);

                graph.AddEdge(preNode, placeNode);
                preNode = placeNode;
            }
        }

        PlayData.curPlace = new Place(0, 0, false);
        return graph;
    }

    //���� ������ ��ġ ��������
    public void SetAvailableNode()
    {
        availableNode = PlayData.routeGraph.GetNeighbors(PlayData.curPlace);
    }

    public void GenerateMap()
    {
        //DFS�� �̿��� ��� ��� Ž��
        Dictionary<Place, List<Place>> dic = PlayData.routeGraph.GetDictionary();
        Place startNode = new Place(0, 0, false);
        if (!dic.ContainsKey(startNode)) return;

        Stack<Place> stack = new Stack<Place>();
        HashSet<Place> visited = new HashSet<Place>();

        stack.Push(startNode);

        while (stack.Count > 0)
        {
            Place currentNode = stack.Pop();

            if (!visited.Contains(currentNode))
            {
                GameObject curPref = null;
                //�ʱ���ġ�� ����x
                if(currentNode != startNode)
                {
                    //�湮 ���� �ʾҴ� ���� ���� ������ ����
                    GameObject pref = Instantiate(placePref);
                    pref.transform.SetParent(mapContent, false);
                    pref.transform.localPosition = new Vector2(-1500 + (currentNode.posX * nodeSpacingX), -240 + (currentNode.posY * nodeSpacingY));
                    pref.GetComponent<PlaceNode>().place = currentNode;

                    foreach (Place placeNode in availableNode)
                    {
                        //���� ��尡 �̵� ���� ���� ��ȣ�ۿ� Ȱ��ȭ
                        if (placeNode == currentNode)
                        {
                            pref.GetComponent<Button>().interactable = true;
                        }
                    }

                    curPref = pref;
                }

                visited.Add(currentNode);

                foreach (Place neighbor in dic[currentNode])
                {
                    if(curPref != null)
                    {
                        //currentNode�� �̵� ���� ���鿡 �� �̹��� �����ϱ�
                        //v = neighbor�� ���� ��ġ
                        Vector3 v = new Vector3(-1500 + (neighbor.posX * nodeSpacingX), -240 + (neighbor.posY * nodeSpacingY), 0);
                        //linePos = currentNode�� neighbor�� �߰� ��ġ
                        Vector2 linePos = (curPref.transform.localPosition + v) / 2;
                        //direction = currentNode���� neighbor���� ����
                        Vector3 direction = v - curPref.transform.localPosition;

                        GameObject line = Instantiate(linePref);
                        line.transform.SetParent(mapContent, false);
                        line.transform.localPosition = linePos;
                        if (direction.y > 0)
                        {
                            line.transform.rotation = Quaternion.Euler(0, 0, 120);
                        }
                        else if (direction.y < 0)
                        {
                            line.transform.rotation = Quaternion.Euler(0, 0, 60);
                        }
                    }

                    if (!visited.Contains(neighbor))
                    {
                        stack.Push(neighbor);
                    }
                }
            }
        }
    }

    void SetCurrentMapPosition()
    {
        Vector2 mapPosition = new Vector2(-350 + (PlayData.curPlace.posX * -200), 360);
        Debug.Log(mapPosition);
        mapScroll.anchoredPosition = mapPosition;
    }
}
