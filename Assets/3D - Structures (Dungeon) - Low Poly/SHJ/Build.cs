using Unity.Content;
using UnityEngine;

public class Build : MonoBehaviour
{
    public GameObject matter;
    private Material _matterMaterial;
    private GameObject _curPreviewObj;
    private GameObject _previewObj;

    private Vector3[] _snapPos;
    private Vector3 _buildPos;
    private Collider _collider;
    private bool _isChangeObj = false;
    [SerializeField] private float _gridSize;

    public bool onBuild = false;
    public bool onCollision; // 부딫치고 있는지 확인
    public bool buildCondition; // 파츠 조건

    private float _scroll;

    private GameObject _lastHitObj = null;  // 이전에 본 오브젝트를 추적하는 변수

    private void Update()
    {
        OnPreview();
        if(onBuild)
        {
            Preview();
            OnBuild();
            UpdateObj();
        }
    }

    void UpdateObj()
    {
        if (_previewObj != null)
        {
            if (Input.GetKey(KeyCode.Alpha1))
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
            if (Input.GetKey(KeyCode.Alpha3))
            {
                _scroll = Input.GetAxis("Mouse ScrollWheel");
                _previewObj.transform.localScale += new Vector3(0, 0, 1) * _scroll;
                _isChangeObj = true;
            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                _previewObj.transform.localScale = matter.transform.localScale;
                _isChangeObj = true;
            }
            if(Input.GetMouseButtonDown(2))
            {
                _previewObj.transform.rotation *= Quaternion.Euler(0, 90, 0);
                _isChangeObj = true;
            }
        }
    }

    void CreateSnapPos(GameObject hitObj)
    {
        Vector3 center = hitObj.GetComponent<Collider>().bounds.center;
        Vector3 size = hitObj.GetComponent<Collider>().bounds.extents;

        _snapPos = new Vector3[6]
        {
            // 위쪽 (Top)
             new Vector3(center.x, center.y + size.y, center.z),  // Y축 방향으로 +size.y만큼 이동
             // 아래쪽 (Bottom)
             new Vector3(center.x, center.y - size.y, center.z),  // Y축 방향으로 -size.y만큼 이동
             // 오른쪽 (Right)
             new Vector3(center.x + size.x, center.y, center.z),  // X축 방향으로 +size.x만큼 이동
             // 왼쪽 (Left)
             new Vector3(center.x - size.x, center.y, center.z),  // X축 방향으로 -size.x만큼 이동
             // 앞쪽 (Front)
             new Vector3(center.x, center.y, center.z + size.z),  // Z축 방향으로 +size.z만큼 이동
             // 뒤쪽 (Back)
             new Vector3(center.x, center.y, center.z - size.z)   // Z축 방향으로 -size.z만큼 이동
        };
    }

    void Preview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 15f, LayerMask.GetMask("Ground","Build")))
        {
            _curPreviewObj = hit.collider.gameObject;

            if ((LayerMask.GetMask("Build") & (1 << hit.collider.gameObject.layer)) != 0 && (hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Wall"))) //부딫친 오브젝트의 Layer가 Build이거나 태그가 Floor또는Wall일 때
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
                }
            }
            else
            {
                _buildPos = new Vector3(Mathf.Round(hit.point.x / _gridSize) * _gridSize, Mathf.Round(hit.point.y / _gridSize) * _gridSize, Mathf.Round(hit.point.z / _gridSize) * _gridSize);
            }

            buildCondition = BuildCondition(hit.collider);

        }
        _previewObj.transform.position = _buildPos;
        _matterMaterial.color = onCollision && buildCondition ? new Color(0, 0, 1, 0.2f) : new Color(1, 0, 0, 0.2f);
    }

    bool BuildCondition(Collider hitCollider)
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

    void OnPreview()
    {
        if (Input.GetMouseButtonDown(0) && !onBuild && matter)
        {
            onBuild = true;
            _previewObj = Instantiate(matter);
            _previewObj.AddComponent<PreviewCollisionCheck>();
            _previewObj.layer = LayerMask.NameToLayer("Preview");
            _matterMaterial = _previewObj.GetComponent<Renderer>().material;
            _collider = _previewObj.GetComponent<Collider>();
            if (_collider != null)
            {
                _collider.isTrigger = true;
            }
        }
    }
    void OnBuild()
    {
        if (Input.GetMouseButtonDown(1) && onBuild)
        {
            if (onCollision)
            {
                GameObject obj = Instantiate(matter, _buildPos, Quaternion.Euler(_previewObj.transform.eulerAngles));
                obj.transform.localScale = _previewObj.transform.localScale;
            }
            Destroy(_previewObj);
            onBuild = false;
        }
    }
}
