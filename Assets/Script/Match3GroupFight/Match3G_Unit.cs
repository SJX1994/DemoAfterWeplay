using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3G_PlayerData;
using DG.Tweening;
public class Match3G_Unit : MonoBehaviour
{
# region 数据对象
    public Match3G_Template_Unit_Numerical template_Numerical;
    public Match3G_GroupInfo.GroupType groupType;
    [SerializeField, Range(0f, 5f)]
	float disappearDuration = 0.25f;
    public TileState tileState;
    float disappearProgress;
    new Rigidbody rigidbody;
    Rigidbody Rigidbody
    {
        get
        {
            if(!rigidbody)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Collider>().enabled = true;
            }
            return rigidbody;
        }
    }
    Match3G_FloatingScore floatingScorePrefab;
    public Match3G_FloatingScore FloatingScorePrefab 
    { 
        get 
        { 
            if (floatingScorePrefab == null) 
                floatingScorePrefab = Resources.Load<Match3G_FloatingScore>("FloatingScore");
                floatingScorePrefab = Instantiate(floatingScorePrefab);
            return floatingScorePrefab; 
        } 
    }
    [System.Serializable]
	struct FallingState
	{
		public float fromY, toY, duration, progress;
	}

	FallingState falling;
# endregion 数据对象
# region 数据关系
    public void Spawn ()
	{
        // template_Numerical = Instantiate(template_Numerical);
		disappearProgress = -1f;
        falling.progress = -1f;
	}
    void Update ()
    {
        Update_Disappear();
        Update_Falling();
    }
    
# endregion 数据关系
# region 数据操作
    SingleEnegy gainScore;
    public void Gain(Match3G_Group to,int gainIndex)
    {

        gainScore = new SingleEnegy{
            positionFrom = transform.position,
            positionTo = to.Egg.transform.position,
            value = 1,
            color = Color.clear
        };
        Invoke(nameof(Gain_View),gainIndex * 0.25f);
    }
    
    void Gain_View()
    {
        Gain_Logic();
        Match3G_Group group = groupType == Match3G_GroupInfo.GroupType.GroupA ? Match3G_GroupInfo.Game.GroupA : Match3G_GroupInfo.Game.GroupB;
        Match3G_FloatingScore scoreEffect = FloatingScorePrefab.Show(gainScore.positionFrom,gainScore.positionTo,template_Numerical.unitDescribe,gainScore.value,gainScore.color);
        scoreEffect.OnMoveComplete += () => {
            group.Numerical.CurrentAC += template_Numerical.armorPower;
            group.Numerical.CurrentHP = template_Numerical.healthPower;
            group.Numerical.CurrentMP += template_Numerical.magicPower;
            switch (template_Numerical.unitType)
            {
                case Match3G_Template_Unit_Numerical.UnitType.baseAttacker:
                    group.Shake.ShakeObjectScale();
                    break;
                case Match3G_Template_Unit_Numerical.UnitType.baseDefender:
                    group.Shake.ShakeObjectScale();
                    break;
                case Match3G_Template_Unit_Numerical.UnitType.baseHealer:
                    group.Shake.ShakeObjectScale();
                    break;
                case Match3G_Template_Unit_Numerical.UnitType.baseWizard:
                    group.Shake.ShakeObjectScale();
                    break;
                default:
                    break;
            }
        };
        
    }
    void Gain_Logic()
    {
        gainScore.color = template_Numerical.color;
        if(template_Numerical.healthPower > 0)
        {
            gainScore.value = template_Numerical.healthPower;
        }else if(template_Numerical.magicPower > 0)
        {
            gainScore.value = template_Numerical.magicPower;
        }else if(template_Numerical.attackPower > 0)
        {
            gainScore.value = template_Numerical.attackPower;
        }else if(template_Numerical.armorPower > 0)
        {
            gainScore.value = template_Numerical.armorPower;
        }
    }
    public float Fall (float toY, float speed)
	{
		falling.fromY = transform.localPosition.y;
		falling.toY = toY;
		falling.duration = (falling.fromY - toY) / speed;
		falling.progress = 0f;
		enabled = true;
		return falling.duration;
	}
    public void MoveToHealthBar(Vector3 pos,Match3G_Group from)
    {
        Tween t = transform.DOMove(pos, disappearDuration-Random.Range(0.01f,disappearDuration-0.01f)).SetEase(Ease.InSine);
        t.onComplete += () => {
            from.Shake.ShakeObjectScale();
            string Particle_Hit = "Effect_Explosion";
            ParticleLoader.Instance.PlayParticleTemp(Particle_Hit,pos, new Vector3(-90,0,0));
            from.Numerical.CurrentHP = -1 -template_Numerical.attackPower;
            Camera.main.GetComponent<Shake>().ShakeObjectPosition();
        };
    }
    public float Disappear ()
	{
		disappearProgress = 0f;
		enabled = true;
		return disappearDuration;
	}
    // 待启用
    public void MakeEnergy()
    {
        Vector3 toPos = transform.position;
        if(Match3G_GroupInfo.groupTurn == Match3G_GroupInfo.GroupType.GroupA)
        {
            toPos.y -= 10f;
        }else
        {
            toPos.y += 10f;
        }
        SingleEnegy score = new SingleEnegy{
            positionFrom = transform.position,
            positionTo = toPos,
            value = 1
        };
        FloatingScorePrefab.Show(score.positionFrom,score.positionTo,score.value);
    }
    public void KnockedAway()
    {
        Rigidbody.mass = 0.5f;
        Rigidbody.AddForce(new Vector3( Random.Range(0f, 1f)*10f, Random.Range(0f, 1f)*10f,Random.Range(-0.5f, -1f)*50f));
    }
    
    void Update_Falling()
    {
        if (falling.progress >= 0f)
		{
			Vector3 position = transform.localPosition;
			falling.progress += Time.deltaTime;
			if (falling.progress >= falling.duration)
			{
				falling.progress = -1f;
				position.y = falling.toY;
				enabled = disappearProgress >= 0f;
			}
			else
			{
				position.y = Mathf.Lerp(
					falling.fromY, falling.toY, falling.progress / falling.duration
				);
			}
			transform.localPosition = position;
		}
    }
    void Update_Disappear()
    {
        if (disappearProgress >= 0f)
		{
			disappearProgress += Time.deltaTime;
			if (disappearProgress >= disappearDuration)
			{
				Destroy(gameObject);
				return;
			}
			transform.localScale = Vector3.one * (1.5f - disappearProgress / disappearDuration);
				
		}
    }
# endregion 数据操作
}
