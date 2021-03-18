//#define LOG // 디버깅시 필요한 로그를 뿌려주고 싶을 때 활성화 시키자.
using UnityEngine;
using System.Collections.Generic;


public class UIReuseGrid : UIWidgetContainer
{
	public UIReuseScrollView	m_ReuseScrollView;
    public GameObject 			m_ScrollViewPrefab;

	public int m_cellWidth;
    public int m_cellHeight;
	public bool m_InitCreateData;
    
	public int m_Column		= 1;

    private int 		m_maxLine;
    private UIReuseScrollViewCell[] 	m_cellList;
    private UIReuseScrollView 			m_ScrollView;
    private float lastPos = -1;

	private List<IReuseCellData> m_listData;
    private Vector3 defaultPos;

	public delegate GameObject Func();
	Func create_fn;

	#region Unity Fucntions
    void Awake()
    {
		if( m_ReuseScrollView == null )
		{
			Debug.LogError( "m_ReuseScrollView == null" );
			return;
		}
		if( m_ReuseScrollView.movement != UIReuseScrollView.Movement.Horizontal &&
		    m_ReuseScrollView.movement != UIReuseScrollView.Movement.Vertical )
		{
			Debug.LogError( "Check... m_ReuseScrollView.movement" );
			return;
		}

		if( m_cellWidth != 0 && m_InitCreateData )
			InitData( null );
    }

	void Update()
	{
		if( m_ScrollView == null )
			return;

		if( m_ReuseScrollView.movement == UIReuseScrollView.Movement.Vertical )
		{
            float PosY = m_ScrollView.transform.localPosition.y;
			if( PosY != lastPos )
			{
                if (!m_ReuseScrollView.disableDragIfFits)
                {
                    // 화면내 출력개수보다 적은 상태인 데... 스크롤 처리되면 갱신처리를 할 이유가 없다.
                    // 패스처리.
                    if (m_Column > 1)   
                    {
                        if (m_listData.Count < m_maxLine * m_Column) // Ver Mulit
                        {
                            lastPos = m_ScrollView.transform.localPosition.y;
                            return;
                        }
                    }
                    else
                    {
                        if (m_listData.Count < m_maxLine) // Ver Single
                        {
                            lastPos = m_ScrollView.transform.localPosition.y;
                            return;
                        }
                    }
                }

                // DEV NOTE: 15/07/13
                //  스크롤바로 바로 이동이 가능할 때 갱신을 전부 하지 않아서 생기는 문제 해결을 위해서...
                //  마지막 좌표와 비교하여 셀크기를 넘어가면 전부 업데이트 하게 처리했다.
                //  그냥 조금씩 움직일 때는 이 처리가 무의미 하니까 셀 크기만큼 처리하도록 수정했다.
                float Height;
                if (lastPos > PosY)
                    Height = lastPos - PosY;
                else
                    Height = PosY - lastPos;
                if (Height > m_cellHeight)
                {
                    Validate(true, true);
                }
                else
                {
                    Validate(true, false);
                }
				lastPos = m_ScrollView.transform.localPosition.y;
			}
		}
		else
		{
			if( m_ScrollView.transform.localPosition.x != lastPos )
			{
                float PosX = m_ScrollView.transform.localPosition.x;
                if (PosX != lastPos)
                {
                    if (!m_ReuseScrollView.disableDragIfFits)
                    {
                        // 화면내 출력개수보다 적은 상태인 데... 스크롤 처리되면 갱신처리를 할 이유가 없다.
                        // 패스처리.
                        if (m_listData.Count < m_maxLine)
                        {
                            lastPos = m_ScrollView.transform.localPosition.x;
                            return;
                        }
                    }

                    float Width;
                    if (lastPos > PosX)
                        Width = lastPos - PosX;
                    else
                        Width = PosX - lastPos;
                    if (Width > m_cellWidth)
                    {
                        Validate(true, true);
                    }
                    else
                    {
                        Validate(true, false);
                    }
                    lastPos = m_ScrollView.transform.localPosition.x;
                }
			}
		}
	}
	#endregion

	public IReuseCellData GetCellData( int index )
	{
		if( m_listData.Count == 0 )
			return null;
		else
			return m_listData[ index ];
	}

	public int MaxCellData{
		get{
            if (m_listData != null)
                return m_listData.Count;
            else
                return 0;
		}
	}

    public List<IReuseCellData> DataList
    {
        get { return m_listData; }
    }

    public UIReuseScrollViewCell[] CellListData
    {
        get
        {
            return m_cellList;
        }
    }
    public int MaxCellList
    {
        get
        {
            return CellListData.Length;
        }
    }

	public void SendMessageFromCellList(string methodName)
    {
        for (int i = 0; i < m_cellList.Length; ++i)
            m_cellList[i].SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
    }

    public void SendMessageFromCellList(string methodName, int idx)
    {
        if (idx >= m_cellList.Length && idx < 0)
            return;

        m_cellList[idx].SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
    }
    
    /// <summary>
    /// 데이터 초기화 처리. 
    /// </summary>
    /// <code>
    /// m_UIReuseGrid.InitData( () => 
    /// { 
    ///     GameObject go = Instantiate(m_ScrollViewPrefab) as GameObject;
    ///     CustomReuseCellData custom = go.GetComponent&lt;CustomReuseCellData&gt;();
    ///     // 추가적으로 해야할 것들 처리.
    ///     return go;
    /// });
    /// </code>
    /// <param name="function">이름</param>
    public void InitData( Func function )
	{
		if (IsInit())
            return;		
        
        create_fn = function;

		if( m_listData == null )
			m_listData	= new List<IReuseCellData>();

        if (m_ScrollView == null)
        {
            if (m_ReuseScrollView != null)
                m_ScrollView = m_ReuseScrollView;
            else
                m_ScrollView = NGUITools.FindInParents<UIReuseScrollView>(gameObject);
        }
		
		Repostion();

		if( m_Column > 1 )
			m_cellList = new UIReuseScrollViewCell[m_maxLine*m_Column];
		else
			m_cellList = new UIReuseScrollViewCell[m_maxLine];
		CreateListItem();
	}

	public bool IsInit()
	{
		if( m_cellList == null )
			return false;
		else
			return true;
	}

	void Repostion()
	{
		if( m_ReuseScrollView.movement == UIReuseScrollView.Movement.Vertical )
		{
			defaultPos 		= m_ScrollView.transform.localPosition;
			defaultPos.y	-= m_cellHeight;
						
			float height 	= m_ScrollView.panel.height;
			m_maxLine 		= Mathf.CeilToInt(height / m_cellHeight) + 1;
		}
		else
		{
			defaultPos 		= m_ScrollView.transform.localPosition;
			defaultPos.x   += m_cellWidth;
			
			float width 	= m_ScrollView.panel.width;
			m_maxLine 		= Mathf.CeilToInt(width / m_cellWidth) + 1;
		}
	}

    private void UpdateBounds(int count)
    {
        Vector3 vMin = new Vector3();
		if( m_ReuseScrollView.movement == UIReuseScrollView.Movement.Vertical )
		{
	        vMin.x = -transform.localPosition.x;
	        vMin.y = transform.localPosition.y - count * m_cellHeight;
	        vMin.z = transform.localPosition.z;
		}
		else
		{
			vMin.x = transform.localPosition.x + count * m_cellWidth;
			vMin.y = -transform.localPosition.y;
			vMin.z = transform.localPosition.z;
		}

		Bounds b = new Bounds(vMin, Vector3.one);
		b.Encapsulate(transform.localPosition);
        m_ScrollView.bounds = b;
        m_ScrollView.UpdateScrollbars(true);
        m_ScrollView.RestrictWithinBounds(true);
    }

    public void ChangeItem(List<IReuseCellData> list)
    {
        m_listData = list;
    }

    /// <summary>
    /// 데이터를 추가한다. 한 번에 넣을 때는 Update를 false로 넘기고 마지막에 UpdateAllCellData()를 호출해주자.
    //// </summary>
    /// <param name="ListData">데이터</param>
    /// <param name="Update">셀을 업데이트 할 지 여부. true: 갱신</param>
    public void AddItem(IReuseCellData ListData, bool Update)
    {
        if (m_listData == null)
            Debug.LogWarning("m_listData null");

		m_listData.Add(ListData);

		if( Update )
			UpdateAllCellData();
    }

    /// <summary>
    /// 데이터를 해당 인덱스에 추가한다. 한 개만 넣을 때 외에는 반드시 Update를 false로 하고 마지막에 UpdateAllCellData()를 호출한다.
    /// </summary>
    /// <param name="idx">리스트 인덱스</param>
    /// <param name="ListData">데이터</param>
    /// <param name="Update">셀을 업데이트 할 지 여부. true: 갱신</param>
	public void InsertItem(int idx, IReuseCellData ListData, bool Update)
    {
        m_listData.Insert(idx, ListData);

        if( Update )
			UpdateAllCellData();
    }	
    
    public void RemoveItem(IReuseCellData ListData, bool Update)
	{
		if( ListData == null )
			return;

		m_listData.Remove(ListData);
		if( Update )
			UpdateAllCellData();
	}

	public void ClearItem(bool Update)
	{
		if (m_listData == null)
            return;		
        
        m_listData.Clear();
		if( Update )
			UpdateAllCellData();
	}

    public void RemoveCellAll()
    {
        if( m_cellList == null )
            return;

        if (m_listData != null)
            m_listData.Clear();

        for (int i = 0; i < m_cellList.Length; ++i)
        {
            Object.Destroy(m_cellList[i].gameObject);
        }
        m_cellList = null;
    }

	public void UpdateAllCellData()
	{
        if (m_Column > 1)
        {
            int Count = m_listData.Count / m_Column;
            if (m_listData.Count % m_Column != 0)
                ++Count;
            UpdateBounds(Count);
        }
        else
        {
            UpdateBounds(m_listData.Count);
        }

        Repostion();
        UpdateListVer(false,true);
	}

    /// <summary>
    /// m_listData를 직접 수정한 경우 이 함수로 업데이트 시킬 수 있다.
    /// </summary>
    /// <param name="idx"></param>
    public void UpdateCellData(int idx)
    {
        for (int i = 0; i < m_listData.Count; ++i)
        {
            if (m_listData[i].Index == idx)
            {
                for (int k = 0; k < m_cellList.Length; ++k)
                {
                    if (m_cellList[k].Index == idx)
                    {
                        m_cellList[k].UpdateData(m_listData[i]);
                        return;
                    }
                }
                break;
            }
        }
    }

    /// <summary>
    /// 셀 중 한개의 넘긴 셀데이터로 갱신한다.
    /// </summary>
    /// <param name="idx">Cell의 Index</param>
    /// <param name="cellData">바꿀 데이터</param>
    public void UpdateCellData(int idx, IReuseCellData cellData)
    {
        for (int k = 0; k < m_cellList.Length; ++k)
        {
            if (m_cellList[k].Index == idx)
            {
                m_cellList[k].UpdateData(cellData);
                return;
            }
        }
    }

    /// <summary>
    /// CellList의 인덱스 번호의 위치로 이동시키는 기능.
    /// 예를 들면 Hor에서 인덱스 1로 넘기면 2번째 셀이 좌측에 표시되게 된다.
    /// REF: GUITestScrollView 인스펙터에서 테스트 가능하다.
    /// </summary>
    /// <param name="idx"></param>
    public void SetPostion(int idx)
    {
        if (m_ReuseScrollView.movement == UIReuseScrollView.Movement.Vertical)
        {
            if (m_Column > 1)
            {
                // Multi
                int LastSub = m_listData.Count % m_Column;
                if (LastSub == 0)
                {
                    // 딱 떨어진다.
                    int Sub = (m_maxLine - 1) * m_Column;
                    if (idx > m_listData.Count - Sub)
                    {
                        // 범위를 벗어나면 자동으로 마지막 라인으로 셋팅하자.
                        idx = m_listData.Count - Sub;
#if LOG
                        Debug.Log(idx.ToString());
#endif
                    }
                }
                else
                {
                    // 나머지가 있다. 그 개수만큼 빼주고 계산해줘야 한다.
                    int Sub = (m_maxLine - 2) * m_Column;
                    Sub += LastSub;
                    if (idx > m_listData.Count - Sub)
                    {
                        idx = m_listData.Count - Sub;
#if LOG
                        Debug.Log(idx.ToString());
#endif
                    }
                }
                int RealIdx = idx / m_Column;
#if LOG
                Debug.Log(RealIdx.ToString());
#endif
                float PosY = RealIdx * m_cellHeight;

                if (m_ReuseScrollView.transform.localPosition.y == PosY)
                    return;
                else
                {
                    m_ReuseScrollView.panel.clipOffset = new Vector2(0f, -PosY);
                    m_ReuseScrollView.transform.localPosition = new Vector3(0f, PosY, 0f);
                    UpdateAllCellData();
                }
            }
            else
            {
                if (idx > m_listData.Count - (m_maxLine - 1))
                {
                    idx = m_listData.Count - (m_maxLine - 1);
                    if (idx < 0)
                        idx = 0;
                }

                float PosY = idx * m_cellHeight;
                if (m_ReuseScrollView.transform.localPosition.y == PosY)
                    return;

                m_ReuseScrollView.panel.clipOffset = new Vector2(0f, -PosY);
                m_ReuseScrollView.transform.localPosition = new Vector3(0f, PosY, 0f);
                UpdateAllCellData();
            }
        }
        else
        {
            if (m_Column > 1)
            {
                // Multi
                int LastSub = m_listData.Count % m_Column;
                if (LastSub == 0)
                {
                    // 딱 떨어진다.
                    int Sub = (m_maxLine - 1) * m_Column;
                    if (idx > m_listData.Count - Sub)
                    {
                        // 범위를 벗어나면 자동으로 마지막 라인으로 셋팅하자.
                        idx = m_listData.Count - Sub;
#if LOG
                        Debug.Log(idx.ToString());
#endif
                    }
                }
                else
                {
                    // 나머지가 있다. 그 개수만큼 빼주고 계산해줘야 한다.
                    int Sub = (m_maxLine - 2) * m_Column;
                    Sub += LastSub;
                    if (idx > m_listData.Count - Sub)
                    {
                        idx = m_listData.Count - Sub;
#if LOG
                        Debug.Log(idx.ToString());
#endif
                    }
                }
                int RealIdx = idx / m_Column;
#if LOG
                Debug.Log(RealIdx.ToString());
#endif
                float PosX = RealIdx * m_cellWidth;

                if (m_ReuseScrollView.transform.localPosition.x == PosX)
                    return;
                else
                {
                    m_ReuseScrollView.panel.clipOffset = new Vector2(PosX, 0f);
                    m_ReuseScrollView.transform.localPosition = new Vector3(-PosX, 0f, 0f);
                    UpdateAllCellData();
                }
            }
            else
            {
                if (idx > m_listData.Count - (m_maxLine - 1))
                {
                    idx = m_listData.Count - (m_maxLine - 1);
                }

                // Hor
                float PosX = idx * m_cellWidth;
                if (m_ReuseScrollView.transform.localPosition.x == PosX)
                    return;

                m_ReuseScrollView.panel.clipOffset = new Vector2(PosX, 0f);
                m_ReuseScrollView.transform.localPosition = new Vector3(-PosX, 0f, 0f);
                UpdateAllCellData();
            }
        }
    }

    private void Validate(bool Scroll, bool RefreshAll)
    {
		if( m_ReuseScrollView.movement == UIReuseScrollView.Movement.Vertical )
		{
			if( m_Column > 1 )
                UpdateListVerColumn(Scroll, RefreshAll);
			else
                UpdateListVer(Scroll, RefreshAll);
		}
		else
		{
            if (m_Column > 1)
                UpdateListHorColumn(Scroll, RefreshAll);
            else
                UpdateListHor(Scroll, RefreshAll);
		}
    }

	void UpdateListVer( bool scroll, bool RefreshAll )
	{
		if( RefreshAll )
		{
			for( int i=0; i<m_cellList.Length; ++i )
				m_cellList[i].gameObject.SetActive(false);
		}

        Vector3 position = m_ScrollView.panel.transform.localPosition;
        if (!scroll && RefreshAll)
        {
            // 움직이고 있으면 계산이 어긋나므로... 스크롤을 강제로 멈추게 한다.
            m_ScrollView.StopScroll();
            if (m_cellList.Length < m_maxLine)
            {
                if (position.y != 0f)
                {
                    position.y = 0f;
                    m_ScrollView.panel.clipOffset.Set(0f, 0f);
#if LOG
                    Debug.Log(string.Format("maxLine:{0} cellList.Length:{1}", m_maxLine, m_cellList.Length));
#endif
                }
            }
        }

		float _ver 		= Mathf.Max(position.y, 0);
		int startIndex  = Mathf.FloorToInt(_ver / m_cellHeight);
        int endIndex 	= Mathf.Min(m_listData.Count, startIndex + m_maxLine);
		float Value = startIndex * m_cellHeight;
		float Max 	= -((m_maxLine * m_cellHeight) + Value);
		float Min	= -(Value - m_cellHeight);
        float Min1   = -(Value);
#if LOG
		Debug.Log( string.Format( "Min:{0} Max:{1}", Min, Max ) );
		Debug.Log( string.Format( "Start/End Index: {0} / {1} maxLine:{2} cellList.Length:{3}", startIndex, endIndex, m_maxLine, m_cellList.Length ) );
#endif
        
		
		UIReuseScrollViewCell cell;
		int index  = startIndex%m_maxLine;
		if( index >= m_maxLine )
			index = 0;
#if LOG
		Debug.Log( string.Format( "cur Index: {0}", index ) );
#endif
		for (int i = startIndex; i < startIndex + m_maxLine; ++i)
		{
			cell = m_cellList[index];
						
			if (i < endIndex)
			{
                // 2015.07.06
                // 중간, 삽입 삭제처리하다가 오브젝트가 비활성화되는 경우가 생기는 것에 대한 대비 코드.
                // 정확한 오류현상을 파악하지 못해서... 임시로 여기서 매번 체크하게 바꿈.
                if (!cell.gameObject.activeSelf)
                    cell.gameObject.SetActive(true);

				if( RefreshAll )
				{
					m_listData[i].Index = i;
					cell.UpdateData( m_listData[i] );
				}
				else
				{
					if( i == startIndex && cell.transform.localPosition.y == Max )
					{
#if LOG
						Debug.Log( string.Format("Update Top... index:{0}", i ) );
#endif
						m_listData[i].Index = i;
						cell.UpdateData( m_listData[i] );
					}
                    else if (i == startIndex && cell.transform.localPosition.y == Min1)
                    {
                        m_listData[i].Index = i;
                        cell.UpdateData(m_listData[i]);
                    }
					else if( i == endIndex-1 && cell.transform.localPosition.y == Min )
					{
#if LOG
						Debug.Log( string.Format("Update Down... index:{0}", i ) );
#endif
						m_listData[i].Index = i;
						cell.UpdateData( m_listData[i] );
					}
				}
				
				cell.transform.localPosition = new Vector3(0, i * -m_cellHeight, 0);
				
			}
			//else
			//{
				//Debug.Log( string.Format("Defalut Pos... index:{0}", i ) );
				//cell.transform.localPosition = defaultPos;
			//}
			
			++index;
			if( index >= m_maxLine )
				index = 0;
		}
	}

    void UpdateListVerColumn(bool scroll, bool RefreshAll)
	{
		if( RefreshAll )
		{
			for( int i=0; i<m_cellList.Length; ++i )
				m_cellList[i].gameObject.SetActive(false);
		}

		// cell index
		Vector3 position = m_ScrollView.panel.transform.localPosition;
        if (!scroll && RefreshAll)
        {
            // 움직이고 있으면 계산이 어긋나므로... 스크롤을 강제로 멈추게 한다.
            m_ScrollView.StopScroll();
            if (m_cellList.Length < m_maxLine)
            {
                if (position.y != 0f)
                {
                    position.y = 0f;
                    m_ScrollView.panel.clipOffset.Set(0f, 0f);
#if LOG
                    Debug.Log(string.Format("maxLine:{0} cellList.Length:{1}", m_maxLine, m_cellList.Length));
#endif
                }
            }
        }
		float _ver 		= Mathf.Max(position.y, 0);
		int startIndex 	= Mathf.FloorToInt( _ver / m_cellHeight );
		int Count=m_listData.Count/m_Column;
		if( m_listData.Count%m_Column != 0 )
			++Count;
		int endIndex 	= Mathf.Min(Count, startIndex + m_maxLine);

		// min & max
		float Value = startIndex * m_cellHeight;
		float Max 	= -((m_maxLine * m_cellHeight) + Value);
		float Min	= -(Value - m_cellHeight);
        float Min1	= -(Value);

#if LOG
		Debug.Log( string.Format( "Start/End Index: {0} / {1}", startIndex, endIndex ) );
		Debug.Log( string.Format( "Min:{0} Max:{1}", Min, Max ) );
#endif

		// CellData index
		UIReuseScrollViewCell cell;
		int index  = startIndex%m_maxLine;
		if( index >= m_maxLine )
			index = 0;
		else
			index *= m_Column;
		int maxIndex = m_maxLine*m_Column;

		// init
		int DataIndex 	= 0;
		int MaxIndex = m_listData.Count;
		Vector3 pos;

		for( int i=startIndex; i<startIndex+m_maxLine; ++i )
		{
#if LOG
			bool checkT = true;
			bool checkB = true;
#endif

			// 가로측 컬럼 개수만큼 데이터를 갱신한다.
			for( int c=0; c<m_Column; ++c )
			{
				DataIndex = i*m_Column+c;
				if( DataIndex < MaxIndex )
				{
					cell = m_cellList[index];
					if (i < endIndex)
					{
						m_listData[DataIndex].Index = DataIndex;

						if( RefreshAll )
						{
							cell.UpdateData( m_listData[DataIndex] );
						}
						else
						{
							if( i == startIndex && cell.transform.localPosition.y == Max )
							{
#if LOG
								if( checkT )
								{
									Debug.Log( string.Format("Update Top... index:{0}", i ) );
									checkT = false;
								}
#endif
								cell.gameObject.SetActive(true);
								cell.UpdateData( m_listData[DataIndex] );
							}
                            else if (i == startIndex && cell.transform.localPosition.y == Min1)
                            {
#if LOG
								if( checkT )
								{
									Debug.Log( string.Format("Update Top... index:{0}", i ) );
									checkT = false;
								}
#endif
                                cell.gameObject.SetActive(true);
                                cell.UpdateData(m_listData[DataIndex]);
                            }
                            else if (i == endIndex - 1 && cell.transform.localPosition.y == Min)
                            {
#if LOG
								if( checkB )
								{
									Debug.Log( string.Format("Update Down... index:{0}", i ) );
									checkB = false;
								}
#endif
                                cell.gameObject.SetActive(true);
                                cell.UpdateData(m_listData[DataIndex]);
                            }
						}
						if( !cell.gameObject.activeSelf )
							cell.gameObject.SetActive(true);
						pos = cell.transform.localPosition;
						pos.x = c * m_cellWidth; 
						pos.y = i * -m_cellHeight;
						cell.transform.localPosition = pos;
					}
					else
					{
						cell.transform.localPosition = defaultPos;
						if( DataIndex>=Max )
						{
							if( cell.gameObject.activeSelf )
								cell.gameObject.SetActive(false);
						}
					}

					++index;
					if( index >= maxIndex )
						index = 0;
				}
			}
		}
	}

    void UpdateListHor(bool scroll, bool RefreshAll)
	{
        // 스프링 처리가 될 때. 그 방향이 Left라면 갱신할 이유가 없음.
        // 1. Grid Horizotal Sigle를 시작한 후 좌측으로 드래그 할 때 데이터 갱신을 안 한다는 뜻.
        if (m_ScrollView.dragEffect == UIReuseScrollView.DragEffect.MomentumAndSpring &&
            m_ScrollView.SpringDirection == UIReuseScrollView.ESPRING_DIRECTION.Left)
        {
            return;
        }

		if( RefreshAll )
		{
			for( int i=0; i<m_cellList.Length; ++i )
				m_cellList[i].gameObject.SetActive(false);
		}

		Vector3 position = m_ScrollView.panel.transform.localPosition;
        if (!scroll && RefreshAll)
        {
            // 움직이고 있으면 계산이 어긋나므로... 스크롤을 강제로 멈추게 한다.
            m_ScrollView.StopScroll();
            if (m_cellList.Length < m_maxLine)
            {
                if (position.y != 0f)
                {
                    position.y = 0f;
                    m_ScrollView.panel.clipOffset.Set(0f, 0f);
#if LOG
                    Debug.Log(string.Format("maxLine:{0} cellList.Length:{1}", m_maxLine, m_cellList.Length));
#endif
                }
            }
        }

		float _ver = Mathf.Max( Mathf.Abs(position.x), 0 );
		int startIndex = Mathf.FloorToInt( _ver / m_cellWidth );
		int endIndex = Mathf.Min(m_listData.Count, startIndex + m_maxLine);
        
		float Value = startIndex * m_cellWidth;
		float Max 	= (m_maxLine * m_cellWidth) + Value;
		float Min	= Value - m_cellWidth;
#if LOG
		//Debug.Log( string.Format( "Min:{0} Max:{1}", Min, Max ) );
        //Debug.Log(string.Format("Begin:{0} End:{1}", startIndex, startIndex + m_maxLine));
#endif
		
		UIReuseScrollViewCell cell;
		int index  = startIndex%m_maxLine;
		if( index >= m_maxLine )
			index = 0;

		for (int i = startIndex; i < startIndex + m_maxLine; ++i)
		{
			cell = m_cellList[index];
			
			if (i < endIndex)
			{
                if (!cell.gameObject.activeSelf)
                    cell.gameObject.SetActive(true);
				if( RefreshAll )
				{
					m_listData[i].Index = i;
					cell.UpdateData( m_listData[i] );
				}
				else
				{
//					if( cell.name == "ListItem0" )
//						Debug.Log( string.Format("index:{0} pos:{1} / start:{2} end:{3} Max:{4}", 
//						                         i, cell.transform.localPosition.x,
//						                         startIndex, endIndex, Max) );
					if( i == startIndex && cell.transform.localPosition.x == Max )
					{
#if LOG
						//Debug.Log( string.Format("Update Left... index:{0}", i ) );
#endif
						m_listData[i].Index = i;
						cell.UpdateData( m_listData[i] );
					}
					else if( i == endIndex-1 && cell.transform.localPosition.x == Min )
					{
#if LOG
						//Debug.Log( string.Format("Update Right... index:{0}", i ) );
#endif
                        m_listData[i].Index = i;
                        cell.UpdateData(m_listData[i]);
					}
				}
				cell.transform.localPosition = new Vector3(i*m_cellWidth, 0, 0);
			}
			//else
			//{
			//	cell.transform.localPosition = defaultPos;
			//}
			
			++index;
			if( index >= m_maxLine )
				index = 0;
		}
	}

    void UpdateListHorColumn(bool scroll, bool RefreshAll)
    {
        if (m_ScrollView.dragEffect == UIReuseScrollView.DragEffect.MomentumAndSpring &&
            m_ScrollView.SpringDirection == UIReuseScrollView.ESPRING_DIRECTION.Left)
        {
            return;
        }

        if (RefreshAll)
        {
            for (int i = 0; i < m_cellList.Length; ++i)
                m_cellList[i].gameObject.SetActive(false);
        }

        Vector3 position = m_ScrollView.panel.transform.localPosition;
        if (!scroll && RefreshAll)
        {
            m_ScrollView.StopScroll();
            if (m_cellList.Length < m_maxLine)
            {
                if (position.x != 0f)
                {
                    position.x = 0f;
                    m_ScrollView.panel.clipOffset.Set(0f, 0f);
                }
            }
        }

        float _ver = Mathf.Max(Mathf.Abs(position.x), 0);
        int startIndex = Mathf.FloorToInt(_ver / m_cellWidth);

        int Count = m_listData.Count / m_Column;
        if (m_listData.Count % m_Column != 0)
            ++Count;

        int endIndex = Mathf.Min(Count, startIndex + m_maxLine);

        float Value = startIndex * m_cellWidth;
        float Max = (m_maxLine * m_cellWidth) + Value;
        float Min = Value - m_cellWidth;
        float Min1 = Value;

        UIReuseScrollViewCell cell;
        int index = startIndex % m_maxLine;
        if (index >= m_maxLine)
            index = 0;
        else
            index *= m_Column;
        int maxIndex = m_maxLine * m_Column;

        int DataIndex = 0;
        int MaxIndex = m_listData.Count;
        Vector3 pos;

        for (int i = startIndex; i < startIndex + m_maxLine; ++i)
        {
            for (int c = 0; c < m_Column; ++c)
            {
                DataIndex = i * m_Column + c;
                if (DataIndex < MaxIndex)
                {
                    cell = m_cellList[index];
                    if (i < endIndex)
                    {
                        m_listData[DataIndex].Index = DataIndex;

                        if (RefreshAll)
                        {
                            if (!cell.gameObject.activeSelf)
                            {
                                cell.gameObject.SetActive(true);
                            }
                            cell.UpdateData(m_listData[DataIndex]);
                        }
                        else
                        {
                            if (i == startIndex && cell.transform.localPosition.x == Max)
                            {
                                cell.gameObject.SetActive(true);
                                cell.UpdateData(m_listData[DataIndex]);
                            }
                            else if (i == startIndex && cell.transform.localPosition.x == Min1)
                            {
                                cell.gameObject.SetActive(true);
                                cell.UpdateData(m_listData[DataIndex]);
                            }
                            else if (i == endIndex - 1 && cell.transform.localPosition.x == Min)
                            {
                                cell.gameObject.SetActive(true);
                                cell.UpdateData(m_listData[DataIndex]);
                            }
                        }
                        if (!cell.gameObject.activeSelf)
                            cell.gameObject.SetActive(true);

                        pos = cell.transform.localPosition;
                        pos.x = i * m_cellWidth;
                        pos.y = c * -m_cellHeight;
                        cell.transform.localPosition = pos;
                    }
                    else
                    {
                        cell.transform.localPosition = defaultPos;
                        if (DataIndex >= Max)
                        {
                            if (cell.gameObject.activeSelf)
                                cell.gameObject.SetActive(false);
                        }
                    }

                    ++index;
                    if (index >= maxIndex)
                        index = 0;
                }
            }
        }
    }

    private void CreateListItem()
    {
		if( m_Column > 1 )
		{
			int index=0;
			for( int i = 0; i < m_maxLine; ++i )
			{
				for( int c=0; c<m_Column; ++c )
				{
					GameObject go = null;
					if( create_fn == null )
						go = Instantiate(m_ScrollViewPrefab) as GameObject;
					else
						go = create_fn();
					
					go.transform.parent 	= transform;

                    if( go.layer != gameObject.layer)
                    {
                        go.layer = gameObject.layer;
                        NGUITools.SetChildLayer(go.transform, gameObject.layer);
                    }
                   
					go.transform.localScale = Vector3.one;
					go.transform.localPosition = Vector3.zero;
					
					//go.SetActive(false);
					go.name = string.Format( "{0}: {1}x{2}", go.name, c, i );
					
					UIReuseScrollViewCell item = go.GetComponent<UIReuseScrollViewCell>();
					m_cellList[index] = item;
					++index;
				}
			}
		}
		else
		{
			for( int i = 0; i < m_maxLine; ++i )
	        {
				GameObject go = null;
				if( create_fn == null )
					go = Instantiate(m_ScrollViewPrefab) as GameObject;
				else
					go = create_fn();

	            go.transform.parent 	= transform;

                if (go.layer != gameObject.layer)
                {
                    go.layer = gameObject.layer;
                    NGUITools.SetChildLayer(go.transform, gameObject.layer);
                }

	            go.transform.localScale = Vector3.one;
				go.transform.localPosition = Vector3.zero;

	            go.SetActive(false);
				go.name = string.Format( "{0}: {1}", go.name, i );

	            UIReuseScrollViewCell item = go.GetComponent<UIReuseScrollViewCell>();
	            m_cellList[i] = item;
	        }
		}
        create_fn = null;   // 더 이상 쓸 일이 없다.
    }
}