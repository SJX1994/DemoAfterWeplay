using UnityEngine;

public class Unit_UI : MonoBehaviour
{
    private GameObject ui;
    public GameObject UI
    {
        get
        {
            if(!ui)ui = transform.Find("UI").gameObject;
            return ui;
        }
    }
    private bool showing = true;
    public bool Showing
    {
        get=>showing;
        set
        {
            if(showing == value)return;
            showing = value;
            if(showing)Show();
            else Hide();
        }
    }
    private void Show()
    {
        UI.SetActive(true);
    }
    private void Hide()
    {
        UI.SetActive(false);
    }
}