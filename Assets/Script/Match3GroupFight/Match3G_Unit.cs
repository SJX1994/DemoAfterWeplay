using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3G_PlayerData;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

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
    SpriteRenderer spriteRenderer_unit;
    SpriteRenderer SpriteRenderer_unit 
    { 
        get 
        {       
            return spriteRenderer_unit; 
        } 
    }
    [System.Serializable]
	struct FallingState
	{
		public float fromY, toY, duration, progress;
	}

	FallingState falling;
    SingleEnegy gainScore;
    private PostProcessVolume postProcessVolume; 
    PostProcessVolume PostProcessVolume
    {
        get
        {
            if(!postProcessVolume)
            {
                postProcessVolume = FindObjectOfType<PostProcessVolume>();
            }
            return postProcessVolume;
        }
    }
    private Bloom bloom; 
# endregion 数据对象
# region 数据关系
    public void Spawn ()
	{
        disappearDuration = 0.6f;
        // template_Numerical = Instantiate(template_Numerical);
		disappearProgress = -1f;
        falling.progress = -1f;
        spriteRenderer_unit = transform.GetComponentInChildren<SpriteRenderer>();
	}
    void Update ()
    {
        Update_Disappear();
        Update_Falling();
    }
    
# endregion 数据关系
# region 数据操作
    
    public void TurnChange()
    {
        Color trunColor = new Color(1f,1f,1f,1f);
        Color otherTurnColor = new Color(1f,1f,1f,0.5f);
        
        if(!SpriteRenderer_unit)return;
        if(Match3G_GroupInfo.Game.WhichGroupTurn == groupType)
        {
            if(SpriteRenderer_unit.color == trunColor)return;
            transform.position = new Vector3(transform.position.x,transform.position.y,0.0f);
            SpriteRenderer_unit.color = trunColor;
        }else
        {
            if(SpriteRenderer_unit.color == otherTurnColor)return;
            transform.position = new Vector3(transform.position.x,transform.position.y,0.7f);
            SpriteRenderer_unit.color = otherTurnColor;
        }
    }
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
    public void HideAfterOccupied()
    {
        Match3G_GroupInfo.Game.Match.OnTurnSwitch -= TurnChange;
        Tween scaleTween = transform.DOScale(1.3f,0.5f).SetEase(Ease.InBounce);
        scaleTween.onComplete += () => {
            DestoryUnit();
        };
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
    Tween tween_PostProcess;
    Tween backTween_PostProcess;
    public int MoveToHealthBar(Vector3 pos,Match3G_Group to,int index = 0)
    {
        int attackValue = 1 + template_Numerical.attackPower;
        Tween t = transform.DOMove(pos, disappearDuration-Random.Range(0.01f,disappearDuration-0.2f)).SetEase(Ease.InSine);
        t.onComplete += () => {
            to.Shake.ShakeObjectScale();
            string Particle_Hit = "Effect_Explosion";
            ParticleLoader.Instance.PlayParticleTemp(Particle_Hit,pos, new Vector3(-90,0,0));
            to.Numerical.CurrentHP = -attackValue;
            Shake shakeCam = Camera.main.GetComponent<Shake>();
            shakeCam.Shake_strength += 0.005f * (float)index;
            shakeCam.ShakeObjectPosition();
            string Explosion = "Match3G_wav/Explosion";
                Sound.Instance.PlaySoundTemp(Explosion);
        };
        PostProcessVolume.profile.TryGetSettings(out bloom);
        if (!bloom)return 0;
        tween_PostProcess?.Kill();
        backTween_PostProcess?.Kill();
        float initialIntensity = 0f;
        float targetIntensity = 6f + index * 0.5f;
        float duration = 1f;
        bloom.intensity.value = initialIntensity;
        bloom.color.value = Match3G_GroupInfo.groupTurn == Match3G_GroupInfo.GroupType.GroupA? Color.blue + Color.white*0.6f: Color.red + Color.white*0.6f;
        tween_PostProcess = DOTween.To(() => bloom.intensity.value,
                                 value => bloom.intensity.value = value,
                                 targetIntensity,
                                 duration/2);
        tween_PostProcess.SetEase(Ease.OutBounce);
        tween_PostProcess.onComplete += () => {
            backTween_PostProcess = DOTween.To(() => bloom.intensity.value,
                                 value => bloom.intensity.value = value,
                                 initialIntensity,
                                 duration/2);
            backTween_PostProcess.SetEase(Ease.InSine); 
        };
        
        return attackValue;
        
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
        string Jelly_destroy_01or02 = "Match3G_wav/Jelly_destroy_0"+Random.Range(1,3).ToString();
        Sound.Instance.PlaySoundTemp(Jelly_destroy_01or02,1,Random.Range(0.1f,0.5f));
        Rigidbody.mass = 0.5f;
        Rigidbody.AddForce(new Vector3( Random.Range(0f, 1f)*10f, Random.Range(0f, 1f)*10f,Random.Range(-0.5f, -1f)*50f));
        Destroy(gameObject,3f);
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

                string drop3or4 = "Match3G_wav/drop"+Random.Range(3,5).ToString();
                Sound.Instance.PlaySoundTemp(drop3or4);
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
    void DestoryUnit()
    {
        Destroy(gameObject);
    }
# endregion 数据操作
}
