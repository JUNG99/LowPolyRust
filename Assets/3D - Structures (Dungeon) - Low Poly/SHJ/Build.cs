using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class Build : MonoBehaviour
{
    private GameObject[] matter;
    [SerializeField] private int index = 0;
    private Material _matterMaterial;
    private GameObject _curPreviewObj;
    private GameObject _previewObj;

    private Vector3[] _snapPos;
    private Vector3 _buildPos;
    private Collider _collider;
    private bool _isChangeObj = false;
    [SerializeField] private float _gridSize;

    public bool onBuild = false;
    public bool onCollision; // �΋Hġ�� �ִ��� Ȯ��
    private float _collisionSize;
    public bool buildCondition; // ���� ����

    private Quaternion rotate = Quaternion.identity; 
    private Quaternion roofRotate = Quaternion.identity;

    private float _scroll;

    private GameObject _lastHitObj = null;  // ������ �� ������Ʈ�� �����ϴ� ����

    private void Start()
    {
        matter = Resources.LoadAll<GameObject>("Build");
        matter = matter.OrderBy(x => ExtractNumber(x.name)).ThenBy(x => x.name).ToArray();
    }

    private void Update()
    {
        OnPreview();
        if (onBuild)
        {
            Preview();
            CollisionCheck();
            OnBuild();
            UpdateObj();
        }
        MakeRoof();

    }
    // ������ ���� ���� �и�
    int ExtractNumber(string name)
    {
        Match match  = Regex.Match(name, @"\d+");
        return match.Success ? int.Parse(match.Value) : int.MaxValue;
    }

    void UpdateObj()
    {
        if (_previewObj != null)
        {
            // ũ�� ���� ó��
            if (Input.GetKey(KeyCode.Alpha3))
            {
                _scroll = Input.GetAxis("Mouse ScrollWheel");
                _previewObj.transform.localScale += new Vector3(1, 0, 0) * _scroll;
                _isChangeObj = true;
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                _scroll = Input.GetAxis("Mouse ScrollWheel");
                _previewObj.transform.localScale += new Vector3(0, 1, 0) * _scroll;
                _isChangeObj = true;
            }
            if (Input.GetKey(KeyCode.Alpha1))
            {
                _scroll = Input.GetAxis("Mouse ScrollWheel");
                _previewObj.transform.localScale += new Vector3(0, 0, 1) * _scroll;
                _isChangeObj = true;
            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                _previewObj.transform.localScale = matter[index].transform.localScale;
                _isChangeObj = true;
            }
            // ȸ�� ���� ó��
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_previewObj.CompareTag("Roof"))
                {
                    roofRotate = Quaternion.Euler(90, 0, 0);
                    _previewObj.transform.rotation *= roofRotate;
                }
                else
                {
                    rotate = Quaternion.Euler(0, 90, 0);
                    _previewObj.transform.rotation *= rotate;
                }

                _isChangeObj = true;
            }
            // _previewObj ����
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (index < matter.Length - 1)
                    index++;
                else
                    index = 0;
                PreviewSet();
                _isChangeObj = true;
            }

        }
    }

    //�׽�Ʈ��
    void MakeRoof()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit, 15f, LayerMask.GetMask("Build"));
            if (hit.collider != null)
            {
                if (!hit.collider.gameObject.CompareTag("Floor")) return;
                List<GameObject> list = GetConnectedFloorTile(hit.collider.gameObject);
                //�׽�Ʈ1
                foreach (GameObject obj in list)
                {
                    GameObject instance = Instantiate(obj, obj.transform.position + Vector3.up * 6f, Quaternion.identity);
                    instance.tag = "Roof";
                }
            }
        }
    }

    void CreateSnapPos(GameObject hitObj)
    {
        Vector3 center = hitObj.GetComponent<Collider>().bounds.center;
        Vector3 size = hitObj.GetComponent<Collider>().bounds.extents;

        _snapPos = new Vector3[6]
        {
                // ���� (Top)
                 new Vector3(center.x, center.y + size.y, center.z),  // Y�� �������� +size.y��ŭ �̵�
                 // �Ʒ��� (Bottom)
                 new Vector3(center.x, center.y - size.y, center.z),  // Y�� �������� -size.y��ŭ �̵�
                 // ������ (Right)
                 new Vector3(center.x + size.x, center.y, center.z),  // X�� �������� +size.x��ŭ �̵�
                 // ���� (Left)
                 new Vector3(center.x - size.x, center.y, center.z),  // X�� �������� -size.x��ŭ �̵�
                 // ���� (Front)
                 new Vector3(center.x, center.y, center.z + size.z),  // Z�� �������� +size.z��ŭ �̵�
                 // ���� (Back)
                 new Vector3(center.x, center.y, center.z - size.z)   // Z�� �������� -size.z��ŭ �̵�
        };

    }

    void Preview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 15f, LayerMask.GetMask("Ground", "Build")))
        {
            _curPreviewObj = hit.collider.gameObject;

            if ((LayerMask.GetMask("Build") & (1 << hit.collider.gameObject.layer)) != 0 && (hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Roof"))) //�΋Hģ ������Ʈ�� Layer�� Build�̰ų� �±װ� Floor�Ǵ�Wall�� ��
            {
                if (_curPreviewObj != _lastHitObj || _isChangeObj)
                {
                    _isChangeObj = false;
                    CreateSnapPos(hit.collider.gameObject);
                    _lastHitObj = _curPreviewObj;
                }

                float minDistance = float.MaxValue;
                foreach (var pos in _snapPos)
                {
                    float distance = Vector3.Distance(hit.point, pos);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        _buildPos = pos;
                    }
                }

                if (_previewObj.CompareTag("Floor"))
                {
                    Vector3 distancePreview = (_buildPos - hit.collider.transform.position).normalized;

                    if (!Mathf.Approximately(distancePreview.x, 0))
                    {
                        _buildPos += new Vector3(distancePreview.x * _collider.bounds.size.x * 0.5f, 0, 0);
                    }
                    if (!Mathf.Approximately(distancePreview.y, 0))
                    {
                        _buildPos += new Vector3(0, distancePreview.y * _collider.bounds.size.y * 0.5f, 0);
                    }
                    if (!Mathf.Approximately(distancePreview.z, 0))
                    {
                        _buildPos += new Vector3(0, 0, distancePreview.z * _collider.bounds.size.z * 0.5f);
                    }
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                        _buildPos += new Vector3(0, _collider.bounds.extents.y, 0);
                }
            }
            else
            {
                _buildPos = new Vector3(Mathf.Round(hit.point.x / _gridSize) * _gridSize, Mathf.Round(hit.point.y / _gridSize) * _gridSize, Mathf.Round(hit.point.z / _gridSize) * _gridSize);
            }

            buildCondition = BuildCondition(hit.collider);

        }
        _previewObj.transform.position = _buildPos;
        _matterMaterial.color = onCollision && buildCondition ? new Color(0, 0, 1, 0.2f) : new Color(1f, 0, 0, 0.2f);
    }

    bool BuildCondition(Collider hitCollider)
    {
        if (_previewObj.CompareTag("Floor"))
        {
            return true;  // �ٴ��� �׻� ����
        }

        if (hitCollider.CompareTag(_previewObj.tag))
        {
            return true;  // ���� ���������� ����
        }
        if (hitCollider.CompareTag("Floor") && _previewObj.CompareTag("Wall"))
        {
            return true;  // ���� �ٴڿ��� ����
        }

        if (hitCollider.CompareTag("Wall") && _previewObj.CompareTag("Roof"))
        {
            return true;  // ������ ������ ����
        }

        return false;  // �� ���� ���� �Ұ���
    }
    // ������Ʈ�� �΋Hġ�� Floor�±׸� ���� ������Ʈ Ž��
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
    // ������Ʈ�� ����� ��� Floor�±׸� ���� ������Ʈ Ž��
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

    // ������Ʈ�� ����� ��� Floor�±׸� ���� ������Ʈ���� ���� �浹�Ͽ��� ��� ���� ����
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
    //// ������ �ִ� ����
    //float GetMaxWallHeight(List<GameObject> walls)
    //{
    //    float maxHeight = 0f;
    //    foreach (GameObject wall in walls)
    //    {
    //        Ray ray = new Ray(wall.transform.position, Vector3.up);
    //        RaycastHit[] hits =  Physics.RaycastAll(ray, 100, LayerMask.GetMask("Floor"));

    //        float fathesHeight = hits.Max
    //    }
    //    return maxHeight;
    //}

    void CollisionCheck()
    {
        Collider[] colliders = Physics.OverlapBox(_collider.bounds.center, _collider.bounds.extents * _collisionSize);
        colliders = colliders.Where(collider => collider.gameObject != _previewObj).ToArray();

        if (_previewObj.CompareTag("Floor"))
        {
            _collisionSize = 0.9999f;
            colliders = colliders.Where(collider => !(collider.gameObject.layer == LayerMask.NameToLayer("Ground"))).ToArray();
        }
        else if (_previewObj.CompareTag("Wall"))
        {
            _collisionSize = 0.8f;
            colliders = colliders.Where(collider => !(collider.CompareTag("Floor") || collider.CompareTag("Wall") || (collider.CompareTag("Roof")))).ToArray();
        }
        else if (_previewObj.CompareTag("Roof"))
        {
            _collisionSize = 0.9999f;
            colliders = colliders.Where(collider => !collider.CompareTag("Wall")).ToArray();
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            Debug.Log(colliders[i].gameObject.name);
        }
        if (colliders.Length == 0)
            onCollision = true;
        else
            onCollision = false;
    }

    public void SetTransparent(Material targetMaterial)
    {
        if (targetMaterial == null) return;

        targetMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        targetMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        targetMaterial.SetInt("_ZWrite", 0);
        targetMaterial.SetFloat("_Mode", 3);
        targetMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        targetMaterial.EnableKeyword("_ALPHABLEND_ON");
        targetMaterial.DisableKeyword("_ALPHATEST_ON");
        targetMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    }

    void OnPreview()
    {
        if (Input.GetMouseButtonDown(0) && !onBuild && matter[index])
        {
            PreviewSet();
        }
    }
    void PreviewSet()
    {
        if (_previewObj != null)
            Destroy(_previewObj);
        onBuild = true;
        _previewObj = Instantiate(matter[index]);
        _previewObj.layer = LayerMask.NameToLayer("Preview");
        _matterMaterial = _previewObj.GetComponent<Renderer>().material;
        SetTransparent(_matterMaterial);
        _collider = _previewObj.GetComponent<Collider>();
        if (_collider != null)
        {
            _collider.isTrigger = true;
        }
    }
    void OnBuild()
    {
        if (Input.GetMouseButtonDown(1) && onBuild)
        {
            if (onCollision && buildCondition)
            {
                GameObject obj = Instantiate(matter[index], _buildPos, Quaternion.Euler(_previewObj.transform.eulerAngles));
                obj.transform.localScale = _previewObj.transform.localScale;
            }
            Destroy(_previewObj);
            onBuild = false;
        }
    }
}
