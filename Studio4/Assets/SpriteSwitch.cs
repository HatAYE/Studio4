using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteSwitch : MonoBehaviour
{
    public Sprite[] spriteSheet;
    [SerializeField] BagMovement bagIndex;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bagIndex= transform.parent.transform.parent.GetComponent<BagMovement>();
        //reference the partent of the parent
    }

    private void Update()
    {
        if (bagIndex.currentPositionIndex<2)
        {
            spriteRenderer.sprite = spriteSheet[0];
        }
        else spriteRenderer.sprite = spriteSheet[1];

    }
}
