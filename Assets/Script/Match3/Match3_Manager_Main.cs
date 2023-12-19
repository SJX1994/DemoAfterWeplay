using UnityEngine;
using Match3_PlayerData;
public class Match3_Manager_Main : MonoBehaviour
{
    private Match3_Manager_View view;
    public Match3_Manager_View View 
    { 
        get 
        { 
            if (view == null)view = FindObjectOfType<Match3_Manager_View>();
            return view; 
        } 
    }
    private Match3_Manager_Logic logic;
    public Match3_Manager_Logic Logic 
    { 
        get 
        { 
            if (logic == null) logic = FindObjectOfType<Match3_Manager_Logic>();
            return logic; 
        } 
    }
    [SerializeField]
	bool automaticPlay;
    Vector3 dragStart;
    bool isDragging;
    void Awake ()
	{
		View.StartNewGame();
	}
    void Update ()
	{
		if (View.IsPlaying)
		{
			if(!View.IsBusy)HandleInput();
			View.DoWork();
		}
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			View.StartNewGame();
		}
	}

    void HandleInput ()
	{
        if (automaticPlay)
		{
			View.DoAutomaticMove();
		}
		else if (!isDragging && Input.GetMouseButtonDown(0))
		{
			dragStart = Input.mousePosition;
			isDragging = true;
		}
		else if (isDragging && Input.GetMouseButton(0))
		{
			isDragging = View.EvaluateDrag(dragStart, Input.mousePosition);
		}
		else
		{
			isDragging = false;
		}
    }
}
