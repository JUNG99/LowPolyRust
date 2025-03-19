using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class Build : MonoBehaviour
{
    [SerializeField] BuildMakeRoof buildMakeRoof;
    [SerializeField] SetRenderTrans renderTrans;
    [SerializeField] BuildCriteria buildCriteria;

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
    public bool onCollision; // 부딫치고 있는지 확인
    public bool buildCondition; // 파츠 조건

    private Quaternion rotate = Quaternion.identity;
    private Quaternion roofRotate = Quaternion.identity;

    //private float _scroll;

    public bool buildMode = false;

    private GameObject _lastHitObj = null;  // 이전에 본 오브젝트를 추적하는 변수

    private void Start()
    {
        buildMakeRoof = GetComponent<BuildMakeRoof>();
        renderTrans = GetComponent<SetRenderTrans>();
        buildCriteria = GetComponent<BuildCriteria>();

        matter = Resources.LoadAll<GameObject>("Build");
        matter = matter.OrderBy(x => ExtractNumber(x.name)).ThenBy(x => x.name).ToArray();
    }

    private void Update()
    {
        changeMode();
        if ((buildMode))
        {
            OnPreview();
            if (onBuild)
            {
                Preview();
                onCollision = buildCriteria.CollisionCheck(_previewObj);
                OnBuild();
                UpdateObj();
            }
            buildMakeRoof.MakeRoof();
        }
    }
    // 정렬을 위해 숫자 분리
    int ExtractNumber(string name)
    {
        Match match = Regex.Match(name, @"\d+");
        return match.Success ? int.Parse(match.Value) : int.MaxValue;
    }

    void changeMode()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            buildMode = !buildMode;
        }
    }
    void UpdateObj()
    {
        if (_previewObj != null)
        {
            //// 크기 변동 처리
            //if (Input.GetKey(KeyCode.Alpha3))
            //{
            //    _scroll = Input.GetAxis("Mouse ScrollWheel");
            //    _previewObj.transform.localScale += new Vector3(1, 0, 0) * _scroll;
            //    _isChangeObj = true;
            //}
            //if (Input.GetKey(KeyCode.Alpha2))
            //{
            //    _scroll = Input.GetAxis("Mouse ScrollWheel");
            //    _previewObj.transform.localScale += new Vector3(0, 1, 0) * _scroll;
            //    _isChangeObj = true;
            //}
            //if (Input.GetKey(KeyCode.Alpha1))
            //{
            //    _scroll = Input.GetAxis("Mouse ScrollWheel");
            //    _previewObj.transform.localScale += new Vector3(0, 0, 1) * _scroll;
            //    _isChangeObj = true;
            //}
            //if (Input.GetKey(KeyCode.Alpha4))
            //{
            //    _previewObj.transform.localScale = matter[index].transform.localScale;
            //    _isChangeObj = true;
            //}
            // 회전 변동 처리
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
            // _previewObj 변경
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

        if (Physics.Raycast(ray, out RaycastHit hit, 15f, LayerMask.GetMask("Ground", "Build")))
        {
            _curPreviewObj = hit.collider.gameObject;

            if ((LayerMask.GetMask("Build") & (1 << hit.collider.gameObject.layer)) != 0 && (hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Roof"))) //부딫친 오브젝트의 Layer가 Build이거나 태그가 Floor또는Wall일 때
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

            buildCondition = buildCriteria.BuildCondition(_previewObj, hit.collider);

        }
        _previewObj.transform.position = _buildPos;
        _matterMaterial.color = onCollision && buildCondition ? new Color(0, 0, 1, 0.2f) : new Color(1f, 0, 0, 0.2f);
    }

    void PreviewSet()
    {
        if (_previewObj != null)
            Destroy(_previewObj);
        onBuild = true;
        _previewObj = Instantiate(matter[index]);
        _previewObj.layer = LayerMask.NameToLayer("Preview");
        _matterMaterial = _previewObj.GetComponent<Renderer>().material;
        renderTrans.SetTransparent(_matterMaterial);
        _collider = _previewObj.GetComponent<Collider>();
        if (_collider != null)
        {
            _collider.isTrigger = true;
        }
    }

    void OnPreview()
    {
        if (Input.GetMouseButtonDown(0) && !onBuild && matter[index])
        {
            PreviewSet();
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
