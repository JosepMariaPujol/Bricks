using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class RuntimeFileReader : MonoBehaviour
{
    public UIDocument UIdoc;
    private Button m_button;
    private TextField selectedFile;
    private TextField fileContents;
    
    void Start()
    {
        VisualElement v_Root = UIdoc.rootVisualElement;
        m_button = v_Root.Q<Button>("ImportButton"); //BEM
        m_button.RegisterCallback<ClickEvent>(OnSelectFileClicked);
    }
    
    private void OnSelectFileClicked(ClickEvent evt)
    {
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Select a text file", "", "*");
        
        Debug.Log(path);
        
#else
        filePathLabel.text = "⚠️ File picking not supported at runtime.";
        Debug.LogWarning("EditorUtility.OpenFilePanel is Editor-only.");
#endif
    }
}
