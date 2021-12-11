using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIView : MonoBehaviour
{

    public Button submitUserDataButton;
    public Button restoreDefaultButton;
    public TMP_InputField dataSetText;
    public Animator warningAnimator;

    

    private string fileName = "dataset.json";
    

    public List<DataSet> ListOfDataSet = new List<DataSet>();
    public DataSet[] arrayOfDatasets;
    void Awake()
    {
        submitUserDataButton.onClick.AddListener(GenerateMesh_UserInput);
        restoreDefaultButton.onClick.AddListener(GenerateMesh_DefaultData);
    }
    void GenerateMesh_UserInput()
    {
        try
        {
            arrayOfDatasets = JsonHelper.FromJson<DataSet>(dataSetText.text);
            ListOfDataSet.AddRange(arrayOfDatasets);

            FileHandler.SaveToJson<DataSet>(ListOfDataSet, fileName, (response, isSuccess) =>
            {
                if (isSuccess)
                {
                    Debug.Log($"Data save successfully");
                    Controller.self.meshController.GenerateMesh(ListOfDataSet);
                }
                else warningAnimator.SetTrigger("on_warning");
            });
        }
        catch
        {
            Debug.Log("Data formate is not correct");
            warningAnimator.SetTrigger("on_warning");
        }

       

    }

    void GenerateMesh_DefaultData()
    {
      Controller.self.meshController.Restore();
      Controller.self.navigationController .Restore();
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
