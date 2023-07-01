using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BagReset : MonoBehaviour
{
    public List <ObjectRandomizer> objectRandomizer;
    [SerializeField] GameObject resetPoint;
    [SerializeField] GameObject startPoint;
    BagMovement bagMovement;
    void Start()
    {
        bagMovement= GetComponent<BagMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("position index:" + bagMovement.currentPositionIndex);
        
        StartCoroutine(ActivateReset());
    }
    IEnumerator ActivateReset() 
    {
        if (bagMovement.currentPositionIndex >= 3)
        {
        yield return new WaitForSeconds(1);
            resetPoint.SetActive(true);
        }
        else resetPoint.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Respawn")
        {
            transform.position = startPoint.transform.position;
            bagMovement.currentPositionIndex = 0;
            DestroyGameobjects(transform);
            //destroy children of item positions
        }
    }

    void DestroyGameobjects(Transform parent)
    {
        for (int i =0; i<objectRandomizer.Count; i++)
        {
            Transform child = parent.GetChild(i);
            Transform secondChild = child.GetChild(0);
            Destroy(secondChild.gameObject); // Destroy the child GameObject
        }
    }
}
