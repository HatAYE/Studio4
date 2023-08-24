using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QualityChange : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    void Start()
    {
        dropdown.onValueChanged.AddListener(ChangeQualityLevel);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("quality right now is "+ QualitySettings.GetQualityLevel());
    }
    public void ChangeQualityLevel(int qualityIndex)
    {
        //0,3,5. from lowest to highest
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
