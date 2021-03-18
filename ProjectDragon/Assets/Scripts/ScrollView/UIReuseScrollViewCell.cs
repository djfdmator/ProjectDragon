using UnityEngine;

public abstract class UIReuseScrollViewCell : MonoBehaviour 
{
    public virtual int Index
    {
        get
        {
            Debug.LogWarning("override me");
            return 0;
        }
    }
	public abstract void UpdateData( IReuseCellData CellData );
}