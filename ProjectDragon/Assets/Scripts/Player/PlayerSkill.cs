//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{

    //플레이어 이전 스테이터스
    State Player_CurrentState;
    public UISprite My_button;
    public UISprite My_skill_icon;
    public UISprite My_skill_ring;
    public UIButton My_button_subFrame;
    public UILabel My_Label;
    public Player My_Player;
    public GameObject skill;
    Ray ray;
    RaycastHit raycastHit;
    IEnumerator co;
    IEnumerator sk;


//Projectile
    Projectile projectile;
    TargetPoint pointProjectile;
    // public RuntimeAnimatorController[] projectileAnimator;

    private void Awake()
    {
        My_button = gameObject.GetComponent<UISprite>();
        My_button.gameObject.GetComponent<UIButton>().tweenTarget = null;
        My_Label = gameObject.transform.GetChild(0).GetComponent<UILabel>();
        My_skill_icon = gameObject.transform.GetChild(1).GetComponent<UISprite>();
        My_skill_ring = gameObject.transform.GetChild(2 ).GetComponent<UISprite>();
        My_skill_ring.fillAmount = 1;
        My_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        My_Player.StopMaxTime = 1f;
        co = CoolTime(3);
        sk = SkillDamaged();
        projectile= new Projectile();
        pointProjectile = new TargetPoint();
        //skill = GameObject.Find("TestSkill").GetComponent<Skill>();
    }
    IEnumerator CoolTime(float cool)
    {
        My_button.spriteName = "ingameui_44";
        Color32 origin = My_skill_icon.color;
        print("쿨타임 코루틴");
        float i = 0;
        My_Player.StopPlayer = true;
        My_skill_ring.enabled = true;
        while (cool > i)
        {
            My_Label.gameObject.SetActive(true);
            i += Time.deltaTime;
            My_skill_ring.fillAmount = ((i / cool));
            My_skill_icon.color = new Color32(120,120,120,255);
            My_button.gameObject.GetComponent<UIButton>().isEnabled = false;
            My_Label.text = Mathf.FloorToInt(1 + (cool - i)).ToString();
            yield return new WaitForFixedUpdate();
        }
        // print("쿨타임 코루틴 완료");
        My_button.gameObject.GetComponent<UIButton>().isEnabled = true;
        My_skill_icon.color = origin;
        My_Label.gameObject.SetActive(false);
        StopCoroutine(co);
        StopCoroutine(sk);
        My_button.spriteName = "ingameui_56";
        My_skill_ring.enabled = false;
    }
    public void OnClick()
    { SoundManager.Inst.Ds_EffectPlayerDB(12);
        if (My_Player.mp / 10 > 0)
        {
            My_Player.isSkillActive = true;
            float _swordAttackangle = My_Player.enemy_angle;
            if(My_Player.EnemyArray.Count ==0)
            {
               _swordAttackangle =My_Player.current_angle;
            }
            My_Player.MPChanged(5);

            
            if(My_Player.attackType==AttackType.ShortRange)
            {
                 //Create Projectile 
                 Vector2 offset = new Vector2(0.0f, 0.0f);
                  float radius = 0.2f;
                  projectile.Create(My_Player.projectileTargetList, offset, radius, _swordAttackangle, 3.0f, 17, My_Player.projectileAnimator[0], "ProjectileObj", true, My_Player.transform.position);
            }
            else if (My_Player.attackType == AttackType.LongRange)
            {
                 Vector2 offset = new Vector2(0.0f, 0.5f);
                pointProjectile .Create(My_Player.projectileTargetList,offset,0.7f,17,My_Player.projectileAnimator[1],"TargetPoint", My_Player.TempEnemy.transform.position);
                //Create Projectile 
                //projectile.Create(My_Player.projectileTargetList, offset, radius, _swordAttackangle, 3.0f, 10, My_Player.projectileAnimator[1], "ProjectileObj", true, My_Player.transform.position);
            }
            co = CoolTime(3);
            sk = SkillDamaged();
            StartCoroutine(sk);
            // Invoke("PlayerStop",0.5f);
        }
        else
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
    // 눌렀을때, 유지햇을때 스킬 발싸
    // public void OnPress(bool isPressed)
    // {
    //    if(isPressed)
    //     {
    //        Debug.Log("TMLQM");
    //     } 
    // }
    public void PlayerStop()
    {
        My_Player.isSkillActive = false;
        My_Player.CurrentState = State.Idel;
        My_Player.StopPlayer= false;
    }

    IEnumerator SkillDamaged()
    {
        skill.gameObject.SetActive(true);
        StartCoroutine(co);
        yield return new WaitForSeconds(0.2f);
        skill.gameObject.SetActive(false);
    }
    void MPControll()
    {
        float maxMp = My_Player.maxMp;
        float myMp = My_Player.mp;
        float Per = myMp / maxMp;
        Debug.Log(myMp+">>>"+maxMp+":"+Per);
    }
}