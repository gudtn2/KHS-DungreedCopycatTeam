using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderManager : MonoBehaviour
{
    [SerializeField]
    private float       npcDetectionRadius;
    private void Awake()
    {
        StartCoroutine(CheckNPC());
    }
    private IEnumerator CheckNPC()
    {
        while(true)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, npcDetectionRadius);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("NPC"))
                {
                    GameObject scanObj = collider.gameObject;
                    float dis = Vector2.Distance(transform.position, scanObj.transform.position);

                    if (dis < npcDetectionRadius && Input.GetKeyDown(KeyCode.F))
                    {
                        UIManager.instance.OnTalk(scanObj);
                    }
                }
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, npcDetectionRadius);
    }
}
