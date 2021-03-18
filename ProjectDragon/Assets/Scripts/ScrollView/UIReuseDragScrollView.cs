using UnityEngine;


public class UIReuseDragScrollView : MonoBehaviour
{
	public UIReuseScrollView scrollView;

    [HideInInspector]
    [SerializeField]
    UIReuseScrollView draggablePanel;

	Transform mTrans;
	UIReuseScrollView mScroll;
	bool mAutoFind = false;

	/// <summary>
	/// scrollView 값을 넣을 때 꼭 이 함수로 호출할 것.
	/// </summary>
	public void SetScrollView( UIReuseScrollView scroll )
	{
		if( scroll == null )
		{
			Debug.LogWarning( "scroll == null" );
			return;
		}

		scrollView 	= scroll;
		mScroll		= scroll;
		mAutoFind 	= false;
	}

	void OnEnable ()
	{
		mTrans = transform;

        if (scrollView == null && draggablePanel != null)
		{
			scrollView = draggablePanel;
			draggablePanel = null;
		}
		FindScrollView();
	}

	void FindScrollView ()
	{

        UIReuseScrollView sv = NGUITools.FindInParents<UIReuseScrollView>(mTrans);

		if (scrollView == null)
		{
			scrollView = sv;
			mAutoFind = true;
		}
		else if (scrollView == sv)
		{
			mAutoFind = true;
		}
		mScroll = scrollView;
	}


	void Start () { FindScrollView(); }


	void OnPress (bool pressed)
	{
		if (mAutoFind && mScroll != scrollView)
		{
			mScroll = scrollView;
			mAutoFind = false;
		}

		if (scrollView && enabled && NGUITools.GetActive(gameObject))
		{
			scrollView.Press(pressed);
			
			if (!pressed && mAutoFind)
			{
                scrollView = NGUITools.FindInParents<UIReuseScrollView>(mTrans);
				mScroll = scrollView;
			}
		}
	}


	void OnDrag (Vector2 delta)
	{
		if (scrollView && NGUITools.GetActive(this))
			scrollView.Drag();
	}

	void OnScroll (float delta)
	{
		if (scrollView && NGUITools.GetActive(this))
			scrollView.Scroll(delta);
	}
}
