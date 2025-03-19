using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildCriteria : MonoBehaviour
{
    public bool BuildCondition(GameObject previewObj, Collider hitCollider)
    {
        if (previewObj.CompareTag("Floor"))
        {
            return true;  // �ٴ��� �׻� ����
        }

        if (hitCollider.CompareTag(previewObj.tag))
        {
            return true;  // ���� ���������� ����
        }
        if (hitCollider.CompareTag("Floor") && previewObj.CompareTag("Wall"))
        {
            return true;  // ���� �ٴڿ��� ����
        }

        if (hitCollider.CompareTag("Wall") && previewObj.CompareTag("Roof"))
        {
            return true;  // ������ ������ ����
        }

        return false;  // �� ���� ���� �Ұ���
    }


    public bool CollisionCheck(GameObject previewObj)
    {
        Collider collider = previewObj.GetComponent<Collider>();

        Collider[] colliders = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents * 0.9999f);
        colliders = colliders.Where(collider => collider.gameObject != previewObj).ToArray();

        if (previewObj.CompareTag("Floor"))
        {
            colliders = colliders.Where(collider => !(collider.gameObject.layer == LayerMask.NameToLayer("Ground"))).ToArray();
        }
        else if (previewObj.CompareTag("Wall"))
        {
            colliders = colliders.Where(collider => !(collider.CompareTag("Floor") || collider.CompareTag("Wall") || (collider.CompareTag("Roof")))).ToArray();
        }
        else if (previewObj.CompareTag("Roof"))
        {
            colliders = colliders.Where(collider => !collider.CompareTag("Wall")).ToArray();
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            Debug.Log(colliders[i].gameObject.name);
        }
        if (colliders.Length == 0)
            return true;
        else
            return false;
    }
}
