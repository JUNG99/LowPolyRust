using UnityEngine;

public class Build : MonoBehaviour
{
    public GameObject matter;
    private GameObject _previewObj;

    public bool onBuild = false;
    private bool _canBuild;
    private Vector3 _objHeight;
    private Vector3 censor;

    private void Update()
    {
        OnPreview();
        if(onBuild)
            UpdatePreview();
        RotatePreviewObj();
        OnBuild();
    }
    
    void UpdatePreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit , 5f))
        {
            Material mat = _previewObj.GetComponent<Renderer>().material;
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    _canBuild = _previewObj.CompareTag("Floor");
                }
                else if (hit.collider.CompareTag("Floor"))
                {
                    _canBuild = _previewObj.CompareTag("Wall");
                }
                else if (hit.collider.CompareTag("Wall"))
                {
                    _canBuild = _previewObj.CompareTag("Roof");
                }
                else
                    _canBuild = false;
            }
            mat.color = _canBuild ? new Color(0, 0, 1, 0.2f) : new Color(1, 0,0, 0.2f);

            _previewObj.transform.position = hit.point;
        }
    }
    void RotatePreviewObj()
    {
        if(Input.GetMouseButtonDown(2))
        {
            _previewObj.transform.eulerAngles += new Vector3(0, 30, 0);
        }
    }
    public void OnPreview()
    {
        if (Input.GetMouseButtonDown(0) && !onBuild && matter)
        {
            onBuild = !onBuild;
            _previewObj = Instantiate(matter);

            Collider col = _previewObj.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }

    public void OnBuild()
    {
        if(Input.GetMouseButtonDown(1) && onBuild)
        {
            if(_canBuild)
            {
                Instantiate(matter, _previewObj.transform.position,Quaternion.Euler(_previewObj.transform.eulerAngles));
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
