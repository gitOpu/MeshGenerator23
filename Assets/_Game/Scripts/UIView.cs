using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIView : MonoBehaviour
{

    public Button submitUserDataButton;
    public Button loadOnlineDataButton;
    public Button synchLocalDataButton;
    public Button restoreDefaultButton;

    public GameObject userDataSet1;
    public GameObject userDataSet2;
    public GameObject userDataSet3;

    public Animator warningAnimator;

  
    void Awake()
    {
        submitUserDataButton.onClick.AddListener(GenerateMesh_UserInput);
        loadOnlineDataButton.onClick.AddListener(GenerateMesh_OnlineData);
        synchLocalDataButton.onClick.AddListener(GenerateMesh_LocalData);
        restoreDefaultButton.onClick.AddListener(GenerateMesh_DefaultData);

       


    }
    void Start()
    {
        GenerateMesh_DefaultData();
    }
    void GenerateMesh_UserInput()
    {

        Vector3 point1 = new Vector3(GetNumber(userDataSet1.transform.GetChild(0).GetComponent<TMP_InputField>()), 
             GetNumber(userDataSet1.transform.GetChild(1).GetComponent<TMP_InputField>()), 
             GetNumber(userDataSet1.transform.GetChild(2).GetComponent<TMP_InputField>()));

        Vector3 point2 = new Vector3(GetNumber(userDataSet2.transform.GetChild(0).GetComponent<TMP_InputField>()),
            GetNumber(userDataSet2.transform.GetChild(1).GetComponent<TMP_InputField>()),
            GetNumber(userDataSet2.transform.GetChild(2).GetComponent<TMP_InputField>()));

        Vector3 point3 = new Vector3(GetNumber(userDataSet3.transform.GetChild(0).GetComponent<TMP_InputField>()),
            GetNumber(userDataSet3.transform.GetChild(1).GetComponent<TMP_InputField>()),
            GetNumber(userDataSet3.transform.GetChild(2).GetComponent<TMP_InputField>()));

      


        DataSet dataSet = new DataSet(point1, point2, point3);

        if (IsTriangle(dataSet)){
            Controller.self.meshController.GenerateMesh(dataSet, true);
        }
        else
        {
            warningAnimator.SetTrigger("on_warning");
        } 
    }
    void GenerateMesh_OnlineData()
    {
        Controller.self.meshController.LoadOnlineData((isSuccess) => { });
    }
    void GenerateMesh_LocalData()
    {
        Controller.self.meshController.LoadLocalData((isSuccess) => {  });
    }
    void GenerateMesh_DefaultData()
    {
        Controller.self.meshController.Restore();
        Controller.self.navigationController .Restore();
    }


    private bool IsTriangle(DataSet dataset)
    {
        bool status = false;
        float side1 = Vector3.Distance(dataset.point1, dataset.point2);
        float side2 = Vector3.Distance(dataset.point2, dataset.point3);
        float side3 = Vector3.Distance(dataset.point3, dataset.point1);
        if (side1 + side2 > side3 && side2 + side3 > side1 && side3 + side1 > side2) return status = true;
        Debug.Log($"IsTriangle {status} : " + side1 + "; " + side2 + "; " + side2);
        return status;
    }

    private float GetNumber(TMP_InputField TMP_text)
    {
        bool isNumber = float.TryParse(TMP_text.text, out float value);
        // Debug.Log("Value " + value);
        TMP_text.text = "";
        if (isNumber) return value;
       else return 0;

    }
}
