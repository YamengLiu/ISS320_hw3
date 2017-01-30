using UnityEngine;
using System.Collections;

public class ExperimentRunner : MonoBehaviour
{
    public string experimentDataFile = "experiment.csv";

    public GameObject stimulus1; //loaded these up either via editor gui, or via GameObject.Find
    public GameObject stimulus2;
    public GameObject stimulus3;

    string objectName; //these describe the current stimulus, use these!
    string displayTime;
    string allowUserInput;

    string[] allExperimentLines; //for CSV file
    int experimentLineIndex = -1;

    GameObject target;//current stimulus
    float Timer = 0.0f;//Time counter for each stimulus;
    float Rotation = 5.0f;//Rotation speed

    void loadCSV()
    {
        //put experiment.csv file in /Assets/StreamingAssets
        string fname = Application.streamingAssetsPath + "/" + experimentDataFile;

        string text = System.IO.File.ReadAllText(fname);

        char[] delimiterNewLine = { '\n' };
        allExperimentLines = text.Split(delimiterNewLine);
    }

    void getNextExperimentRow()
    {
        experimentLineIndex = (experimentLineIndex + 1) % allExperimentLines.Length; //increment and loop to begining if needed
        string currentRow = allExperimentLines[experimentLineIndex];

        char[] delimiterComma = { ',' };

        currentRow = currentRow.Replace("\n", ""); //some data cleaning
        currentRow = currentRow.Replace("\r", ""); //should be \r and not \\r
        currentRow = currentRow.Replace(" ", ""); //remove any spaces

        //this needs to happen after data cleaning
        string[] vals = currentRow.Split(delimiterComma); //split current line out, based on comma locations

        objectName = vals[0];
        displayTime = vals[1];
        allowUserInput = vals[2];

        Debug.Log("loaded experiment row: " + objectName + " " + displayTime + " " + allowUserInput);
    }

    // Use this for initialization
    void Start()
    {
        loadCSV();
        getNextExperimentRow(); //start off with the first stimulus
    }

    // Update is called once per frame
    void Update()
    {
        //Set current stimulus
        if (objectName == "stimulus1") { target = stimulus1;}
        else if(objectName == "stimulus2"){ target = stimulus2; }
        else if(objectName == "stimulus3") { target = stimulus3; }

        //Update timer count and displayTime
        Timer += Time.deltaTime;
        float time = float.Parse(displayTime);

        if(Timer<time)
        {
            //Show stimulus
            target.SetActive(true);
            if (target != null)
            {
                //Keyboard control
                if (allowUserInput=="true")
                {
                    if (target != stimulus1)
                    {
                        if (Input.GetKey("left"))
                        {
                            target.transform.Rotate(0, Rotation * Time.deltaTime, 0);
                        }

                        if (Input.GetKey("right"))
                        {
                            target.transform.Rotate(0, -1.0f * Rotation * Time.deltaTime, 0);
                        }
                    }


                    else
                    {
                        if (Input.GetKey("left"))
                        {
                            target.transform.Rotate(0, -1.0f*Rotation * Time.deltaTime, 0);
                        }

                        if (Input.GetKey("right"))
                        {
                            target.transform.Rotate(0, Rotation * Time.deltaTime, 0);
                        }
                    }
                    
                }
            }
                      
        }

        //if timer counter exceeds displayTime
        else
        {
            //hide stimulus
            target.SetActive(false);   
            
            //Advance to next row     
            getNextExperimentRow();
            
            //reset timer counter
            Timer = 0.0f;

        }
    }
}