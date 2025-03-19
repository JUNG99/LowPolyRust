using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildMakeRoof : MonoBehaviour
{
    //테스트용
    public void MakeRoof()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit, 15f, LayerMask.GetMask("Build"));
            if (hit.collider != null)
            {
                if (!hit.collider.gameObject.CompareTag("Floor")) return;
                List<GameObject> list = GetConnectedFloorTile(hit.collider.gameObject);
                GameObject wall = GetMaxWallHeight(GetConnectedWalls(hit.collider.gameObject));
                if (wall == null) return;
                Vector3 height = wall.transform.position + wall.GetComponent<Collider>().bounds.size;

                foreach (GameObject obj in list)
                {
                    GameObject instance = Instantiate(obj, new Vector3(obj.transform.position.x, height.y, obj.transform.position.z), Quaternion.identity);
                    instance.tag = "Roof";
                }
            }
        }
    }
    // 오브젝트와 부딫치고 Floor태그를 가진 오브젝트 탐색
    List<GameObject> GetNeighbors(GameObject obj)
    {
        HashSet<GameObject> neighbors = new();
        Collider col = obj.GetComponent<Collider>();
        if (col != null)
        {
            Collider[] collider = Physics.OverlapBox(col.bounds.center, col.bounds.extents, Quaternion.identity, LayerMask.GetMask("Build"));
            for (int i = 0; i < collider.Length; i++)
            {
                if (collider[i].gameObject.CompareTag("Floor") && collider[i].gameObject != obj)
                    neighbors.Add(collider[i].gameObject);
            }
        }
        return neighbors.ToList();
    }
    // 오브젝트와 연결된 모든 Floor태그를 가진 오브젝트 탐색
    List<GameObject> GetConnectedFloorTile(GameObject startFloor)
    {
        HashSet<GameObject> connectFloor = new();
        Queue<GameObject> queue = new();
        queue.Enqueue(startFloor);

        while (queue.Count > 0)
        {
            GameObject floor = queue.Dequeue();
            if (!connectFloor.Contains(floor))
            {
                connectFloor.Add(floor);
                foreach (var obj in GetNeighbors(floor))
                {
                    if (!connectFloor.Contains(obj))
                    {
                        queue.Enqueue(obj);
                    }
                }
            }
        }
        return connectFloor.ToList();
    }

    // 오브젝트와 연결된 모든 Floor태그를 가진 오브젝트에서 벽과 충돌하였을 경우 벽을 저장
    List<GameObject> GetConnectedWalls(GameObject startFloor)
    {
        List<GameObject> connectedFloors = GetConnectedFloorTile(startFloor);
        HashSet<GameObject> connectedWalls = new();

        foreach (GameObject floor in connectedFloors)
        {
            Collider[] colliders = Physics.OverlapBox(floor.transform.position, floor.GetComponent<Collider>().bounds.extents, Quaternion.identity, LayerMask.GetMask("Build"));
            foreach (Collider col in colliders)
            {
                if (col.gameObject.CompareTag("Wall"))
                {
                    connectedWalls.Add(col.gameObject);
                }
            }
        }
        return connectedWalls.ToList();
    }
    // 가장 높이 있는 벽
    GameObject GetMaxWallHeight(List<GameObject> walls)
    {
        if (walls.Count == 0 || walls == null)
        {
            return null;
        }
        GameObject highWall = null;
        float maxHeight = 0;
        float fathesHeight = 0;
        foreach (GameObject wall in walls)
        {
            Ray ray = new Ray(wall.transform.position, Vector3.up);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100, LayerMask.GetMask("Build"));
            if (hits.Length == 0)
            {
                if (maxHeight == 0)
                {
                    highWall = wall;
                }
            }
            else
                fathesHeight = hits.Max(h => h.distance);

            if (fathesHeight > maxHeight)
            {
                maxHeight = fathesHeight;
                highWall = hits.Last().collider.gameObject;  // 마지막 히트 객체를 사용
            }
        }
        return highWall;
    }

}
