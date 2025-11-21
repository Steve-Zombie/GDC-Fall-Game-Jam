using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OrganizingManager : MonoBehaviour
{
    public FolderBin recycleBin;
    public FolderBin documentsFolder;

    public Button checker;
    public Button resetButton;

    public Button permRessetButton;

    private bool hasToTryAgain = false;
    void Start()
    {
        Debug.Log("checked disabled");
        checker.gameObject.SetActive(false);
        checker.onClick.AddListener(CheckIfCorrect);
        resetButton.onClick.AddListener(Reset);
        permRessetButton.onClick.AddListener(Reset);
        resetButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(checkIfFilled(recycleBin) && checkIfFilled(documentsFolder))
        {
            if (hasToTryAgain)
            {
                checker.gameObject.SetActive(false);
                resetButton.gameObject.SetActive(true);
                permRessetButton.gameObject.SetActive(false);

            }

            else
            {
                checker.gameObject.SetActive(true);
                resetButton.gameObject.SetActive(false);
                permRessetButton.gameObject.SetActive(false);

            }
            
        }

        else
        {
            checker.gameObject.SetActive(false);
            resetButton.gameObject.SetActive(false);
            permRessetButton.gameObject.SetActive(true);

        }
    }


    private bool checkIfFilled(FolderBin eitherFolderOrBin)
    {
        for(int i = 0; i < eitherFolderOrBin.files.Length; i++)
        {
            if (eitherFolderOrBin.files[i] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void CheckIfCorrect()
    {
        if (recycleBin.checkIfMatching() && documentsFolder.checkIfMatching())
        {
            Debug.Log("YOU WIN!");
            hasToTryAgain = false;
        }

        else
        {
            Debug.Log("You Were Incorrect, Try Again!");
            hasToTryAgain = true;

        }
    }

    public void Reset()
    {
        hasToTryAgain = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
