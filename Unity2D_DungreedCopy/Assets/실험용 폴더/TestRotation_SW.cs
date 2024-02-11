using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation_SW : MonoBehaviour
{
    private float speed = 10f;
    [SerializeField]
    private GameObject  hitPrefab;
    private void Start()
    {
        StartCoroutine("SpawnHit");
    }
    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * speed);
    }

    private IEnumerator SpawnHit()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            GameObject hit = Instantiate(hitPrefab);
            hit.transform.position = transform.position;
            hit.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 180f);

        }
    }
}
