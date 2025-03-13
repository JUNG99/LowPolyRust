using UnityEngine;

public class Build : MonoBehaviour
{
    public GameObject matter;
    private GameObject _curPreviewObj;
    private GameObject _previewObj;

    private Vector3[] _snapPos;
    private Vector3 buildPos;
    private Vector3 boundSize;
    [SerializeField] private float gridSize;

    public bool onBuild = false;
    private bool _canBuild;

    private float _scroll;

    private GameObject lastHitObj = null;  // 이전에 본 오브젝트를 추적하는 변수

    private void Update()
    {
        OnPreview();
        if (onBuild)
            UpdatePreview();
        OnRotatePreviewObj();
        UpdateScale();
        OnBuild();
    }

    void CreateSnapPos(GameObject hitObj)
    {
        Vector3 center = hitObj.GetComponent<Collider>().bounds.center;
        Vector3 size = hitObj.GetComponent<Collider>().bounds.extents;

        _snapPos = new Vector3[12]
        {
            // 위쪽 (Top)
            new Vector3(center.x + size.x, center.y + size.y, center.z),
            new Vector3(center.x - size.x, center.y + size.y, center.z),
            new Vector3(center.x, center.y + size.y, center.z + size.z),
            new Vector3(center.x, center.y + size.y, center.z - size.z),

            // 아래쪽 (Bottom)
            new Vector3(center.x + size.x, center.y - size.y, center.z),
            new Vector3(center.x - size.x, center.y - size.y, center.z),
            new Vector3(center.x, center.y - size.y, center.z + size.z),
            new Vector3(center.x, center.y - size.y, center.z - size.z),

            // 오른쪽 (Right)
            new Vector3(center.x + size.x, center.y, center.z + size.z),
            new Vector3(center.x + size.x, center.y, center.z - size.z),

            // 왼쪽 (Left)
            new Vector3(center.x - size.x, center.y, center.z + size.z),
            new Vector3(center.x - size.x, center.y, center.z - size.z)
        };
        foreach(var a in _snapPos)
        {
            Debug.Log(a);
        }
    }

    void UpdateScale()
    {
        if(_previewObj != null)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                _scroll = Input.GetAxis("Mouse ScrollWheel");
                _previewObj.transform.localScale += new Vector3(1, 0, 0) * _scroll;
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                _scroll = Input.GetAxis("Mouse ScrollWheel");
                _previewObj.transform.localScale += new Vector3(0, 1, 0) * _scroll;
            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                _scroll = Input.GetAxis("Mouse ScrollWheel");
                _previewObj.transform.localScale += new Vector3(0, 0, 1) * _scroll;
            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                _previewObj.transform.localScale = matter.transform.localScale;
            }
        }
    }

    void UpdatePreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 15f, ~LayerMask.GetMask("Player")))
        {
            _curPreviewObj = hit.collider.gameObject;

            Material mat = _previewObj.GetComponent<Renderer>().material;

            if ((LayerMask.GetMask("Ground") & (1 << hit.collider.gameObject.layer)) != 0 && (hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Roof")))
            {
                if (_curPreviewObj != lastHitObj)
                {
                    CreateSnapPos(hit.collider.gameObject);
                    lastHitObj = _curPreviewObj;
                }

                float minDistance = float.MaxValue;
                foreach (var pos in _snapPos)
                {
                    float distance = Vector3.Distance(hit.point, pos);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        buildPos = pos;
                    }
                }
            }
            else
            {
                buildPos = new Vector3(Mathf.Round(hit.point.x / gridSize) * gridSize, Mathf.Round(hit.point.y / gridSize) * gridSize, Mathf.Round(hit.point.z / gridSize) * gridSize);
            }

            Collider[] colliders = Physics.OverlapBox(buildPos, boundSize, Quaternion.Euler(_previewObj.transform.eulerAngles), ~(LayerMask.GetMask("Ground") | LayerMask.GetMask("Player")));
            if (colliders.Length == 0)
            {
                _canBuild = CanBuildOn(hit.collider);
            }
            else
                _canBuild = false;

            _previewObj.transform.localPosition = buildPos;
            mat.color = _canBuild ? new Color(0, 0, 1, 0.2f) : new Color(1, 0, 0, 0.2f);
        }
    }

    bool CanBuildOn(Collider hitCollider)
    {
        if (_previewObj.CompareTag("Floor"))
        {
            return true;  // 바닥은 항상 가능
        }

        if (hitCollider.CompareTag(_previewObj.tag))
        {
            return true;  // 같은 종류끼리는 가능
        }

        if (hitCollider.CompareTag("Floor") && _previewObj.CompareTag("Wall"))
        {
            return true;  // 벽은 바닥에서 가능
        }

        if (hitCollider.CompareTag("Wall") && _previewObj.CompareTag("Roof"))
        {
            return true;  // 지붕은 벽에서 가능
        }

        return false;  // 그 외의 경우는 불가능
    }

    void OnRotatePreviewObj()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _previewObj.transform.eulerAngles += new Vector3(0, 45, 0);
        }
    }

    public void OnPreview()
    {
        if (Input.GetMouseButtonDown(0) && !onBuild && matter)
        {
            onBuild = !onBuild;
            _previewObj = Instantiate(matter);
            _previewObj.transform.localScale = matter.transform.localScale;
            Collider col = _previewObj.GetComponent<Collider>();
            if (col != null)
            {
                boundSize = col.bounds.size * 0.5f;
                col.enabled = false;
            }
        }
    }

    public void OnBuild()
    {
        if (Input.GetMouseButtonDown(1) && onBuild)
        {
            if (_canBuild)
            {
                GameObject obj = Instantiate(matter, buildPos, Quaternion.Euler(_previewObj.transform.eulerAngles));
                obj.transform.localScale = _previewObj.transform.localScale;
                Destroy(_previewObj);
                onBuild = false;
            }
            else
            {
                Destroy(_previewObj);
                onBuild = false;
            }
        }
    }
}
