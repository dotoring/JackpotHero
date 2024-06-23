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

    //routeDepth만큼 y의 좌표들이 있는 리스트 (list[3] == x=3좌표의 y좌표)
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

    //루트를 여러개 만들어 하나의 그래프로 통합
    RouteGraph MakeGraph()
    {
        RouteGraph graph = new RouteGraph();
        for (int i = 0; i < routeAmount; i++)
        {
            //루트 생성
            List<int> route = MakeRoute();
            //인접리스트를 위한 이전 노드, 최초 시작 노드
            Place preNode = new Place(0, 0, false);

            //루트를 하나씩 그래프에 추가
            //최초 시작위치를 x=0으로 하기 위해 1부터
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

    //진행 가능한 위치 가져오기
    public void SetAvailableNode()
    {
        availableNode = PlayData.routeGraph.GetNeighbors(PlayData.curPlace);
    }

    public void GenerateMap()
    {
        //DFS를 이용해 모든 노드 탐색
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
                //초기위치는 생성x
                if(currentNode != startNode)
                {
                    //방문 하지 않았던 노드면 지역 프리팹 생성
                    GameObject pref = Instantiate(placePref);
                    pref.transform.SetParent(mapContent, false);
                    pref.transform.localPosition = new Vector2(-1500 + (currentNode.posX * nodeSpacingX), -240 + (currentNode.posY * nodeSpacingY));
                    pref.GetComponent<PlaceNode>().place = currentNode;

                    foreach (Place placeNode in availableNode)
                    {
                        //현재 노드가 이동 가능 노드면 상호작용 활성화
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
                        //currentNode의 이동 가능 노드들에 선 이미지 생성하기
                        //v = neighbor의 생성 위치
                        Vector3 v = new Vector3(-1500 + (neighbor.posX * nodeSpacingX), -240 + (neighbor.posY * nodeSpacingY), 0);
                        //linePos = currentNode와 neighbor의 중간 위치
                        Vector2 linePos = (curPref.transform.localPosition + v) / 2;
                        //direction = currentNode에서 neighbor로의 방향
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
