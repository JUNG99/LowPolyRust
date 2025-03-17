using Unity.Content;
using UnityEngine;

public class Build : MonoBehaviour
{
    public GameObject matter;
    private Material matterMaterial;
    private GameObject _curPreviewObj;
    private GameObject _previewObj;

    private Vector3[] _snapPos;
    private Vector3 buildPos;
    private Vector3 boundSize;
    [SerializeField] private float gridSize;

    public bool onBuild = false;
    public bool onCollision; // �΋Hġ�� �ִ��� Ȯ��
    private bool buildCondition; // ���� ����

    private float _scroll;

    private GameObject lastHitObj = null;  // ������ �� ������Ʈ�� �����ϴ� ����

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
                boundSize += new Vector3(1, 0, 0) * _scroll*2;
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                _scroll = Input.GetAxis("Mouse ScrollWheel");
                _previewObj.transform.localScale += new Vector3(0, 1, 0) * _scroll;
                boundSize += new Vector3(0, 1, 0) * _scroll*2;
            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                _scroll = Input.GetAxis("Mouse ScrollWheel");
                _previewObj.transform.localScale += new Vector3(0, 0, 1) * _scroll;
                boundSize += new Vector3(0, 0, 1) * _scroll*2;
            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                _previewObj.transform.localScale = matter.transform.localScale;
                boundSize = matter.transform.localScale;
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

        if (Physics.Raycast(ray, out RaycastHit hit, 15f, LayerMask.GetMask("Ground","Build")))
        {
            _curPreviewObj = hit.collider.gameObject;
            CreateSnapPos(hit.collider.gameObject);

            if ((LayerMask.GetMask("Build") & (1 << hit.collider.gameObject.layer)) != 0 && (hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Wall")))
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
                Vector3 distancePreview = (buildPos - hit.collider.transform.position).normalized;
                if (_previewObj.CompareTag("Floor"))
                {
                    if (!Mathf.Approximately(distancePreview.y, 0))
                    {
                        buildPos += new Vector3(0, distancePreview.y * boundSize.y * 0.5f, 0);
                    } 
                    if (!Mathf.Approximately(distancePreview.x, 0))
                    {
                        buildPos += new Vector3(distancePreview.x * boundSize.x * 0.5f, 0, 0);
                    }

                    if (!Mathf.Approximately(distancePreview.z, 0))
                    {
                        buildPos += new Vector3(0, 0, distancePreview.z * boundSize.z * 0.5f);
                    }
                }
            }
            else
            {
                buildPos = new Vector3(Mathf.Round(hit.point.x / gridSize) * gridSize, Mathf.Round(hit.point.y / gridSize) * gridSize, Mathf.Round(hit.point.z / gridSize) * gridSize);
            }

            buildCondition = BuildCondition(hit.collider);

        }
        _previewObj.transform.position = buildPos;
        matterMaterial.color = onCollision && buildCondition ? new Color(0, 0, 1, 0.2f) : new Color(1, 0, 0, 0.2f);
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

    void OnPreview()
    {
        if (Input.GetMouseButtonDown(0) && !onBuild && matter)
        {
            onBuild = true;
            _previewObj = Instantiate(matter);
            _previewObj.AddComponent<PreviewCollisionCheck>();
            _previewObj.layer = LayerMask.NameToLayer("Preview");
            matterMaterial = _previewObj.GetComponent<Renderer>().material;
            Collider col = _previewObj.GetComponent<Collider>();
            if (col != null)
            {
                col.isTrigger = true;
                boundSize = col.bounds.size;
            }
        }
    }
    void OnBuild()
    {
        if (Input.GetMouseButtonDown(1) && onBuild)
        {
            if (onCollision)
            {
                GameObject obj = Instantiate(matter, buildPos, Quaternion.Euler(_previewObj.transform.eulerAngles));
                obj.transform.localScale = _previewObj.transform.localScale;
            }
            Destroy(_previewObj);
            onBuild = false;
        }
    }
}
