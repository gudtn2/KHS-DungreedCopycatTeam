using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTresureBox : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabTresureBox;

    private PoolManager tresureBoxPoolManager;
    private void Awake()
    {
        tresureBoxPoolManager = new PoolManager(prefabTresureBox);
    }
    private void OnApplicationQuit()
    {
        tresureBoxPoolManager.DestroyObjcts();
    }

    public void CreateBox()
    {
        GameObject box = tresureBoxPoolManager.ActivePoolItem();
        box.transform.position = transform.position;
        box.transform.rotation = transform.rotation;
        box.GetComponent<BoxPool>().Setup(tresureBoxPoolManager);
        Destroy(this.gameObject);
    }
}
