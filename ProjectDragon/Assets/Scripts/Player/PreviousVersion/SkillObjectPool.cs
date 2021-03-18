//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SkillObjectPool
{
    [SerializeField]
    private List<GameObject> skillPool = new List<GameObject>();
    public string SkillName = null;
    public GameObject skillpref;
    public int poolCount = 0;

    public void Initialize(Transform parent=null)
    {
        for(int i=0; i>=poolCount; ++i)
        {
           skillPool.Add(CreateSkill(parent));
        }
    }
    public void PushSkill_IntoSkillPool(GameObject skill, Transform parent)
    {
        skill.transform.SetParent(parent);
        skill.SetActive(false);
        skillPool.Add(skill);
    }
    public GameObject PopSkill_GetOutSkillPool(Transform parent)
    {
        if(skillPool.Count ==0)
        {
          skillPool.Add(CreateSkill(parent));
        }
        GameObject skill = skillPool[0];
        skillPool.RemoveAt(0);
        return skill;
    }
    public GameObject CreateSkill(Transform parent)
    {
        GameObject skill = Object.Instantiate(skillpref) as GameObject;
        skill.name = SkillName;
        skill.transform.SetParent(parent);
        skill.SetActive(false);
        return skill;
    }
       

       
}
