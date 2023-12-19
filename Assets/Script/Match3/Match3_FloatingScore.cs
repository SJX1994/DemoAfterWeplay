using TMPro;
using UnityEngine;

public class Match3_FloatingScore : MonoBehaviour
{
    [SerializeField]
	TextMeshPro displayText;
    [SerializeField, Range(0.1f, 1f)]
	float displayDuration = 0.5f;
    [SerializeField, Range(0f, 4f)]
	float riseSpeed = 2f;
    float age;
    PrefabInstancePool<Match3_FloatingScore> pool;
    public void Show (Vector3 position, int value)
	{
		Match3_FloatingScore instance = pool.GetInstance(this);
		instance.pool = pool;
		instance.displayText.SetText("{0}", value);
		instance.transform.localPosition = position;
		instance.age = 0f;
	}
    void Update ()
	{
		age += Time.deltaTime;
		if (age >= displayDuration)
		{
			pool.Recycle(this);
		}
		else
		{
			Vector3 p = transform.localPosition;
			p.y += riseSpeed * Time.deltaTime;
			transform.localPosition = p;
		}
	}
}