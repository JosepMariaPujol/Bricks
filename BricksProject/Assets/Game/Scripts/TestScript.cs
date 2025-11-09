using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class TestScript : MonoBehaviour
{
    public UIDocument UIdoc;
    private Button m_button;
    
    public string FinalPath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement v_Root = UIdoc.rootVisualElement;
        m_button = v_Root.Q<Button>("ImportButton"); //BEM
        //m_button.RegisterCallback<ClickEvent>(LoadFile);
    }

    public void TestFunction(ClickEvent evt)
    {
        Debug.Log("Hello World");
    }


    
    private void Selectfile()
    {
        //var paths = StandaloneFi1eBrowser.OpenFi1ePanel("Open File", "", extensionFilters, false);
        //Debug.Log(paths[0]);
    }
    
    public string selectedFile;

    [ContextMenu("Pick File")]
    public void PickFile()
    {
        string path = EditorUtility.OpenFilePanel("Select a file", "", "");
        if (!string.IsNullOrEmpty(path))
        {
            selectedFile = path;
            Debug.Log("Selected file: " + selectedFile);
        }
        else
        {
            Debug.Log("File selection cancelled.");
        }
    }
    
    
    /*public void LoadFile(ClickEvent evt) DOESNT WORK
    {
        string FileType = NativeFilePicker.ConvertExtensionToFileType("*");

        NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                Debug.Log("Operation cancelled");
            }
            else
            {
                FinalPath = path;
                Debug.Log("Picked file: " + FinalPath);
            }
        }, new string[] { FileType });
    }*/
}