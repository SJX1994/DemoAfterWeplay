using UnityEngine;

[CreateAssetMenu(menuName = "DemoAfterWeplay/Match3G_Template_Unit_Numerical")]
public class Match3G_Template_Unit_Infomation : ScriptableObject
{
    public enum UnitType
    {
        baseAttacker,
        baseDefender,
        baseHealer,
        baseWizard
    }
    public UnitType unitType;
    public Color color = Color.white;
    public string unitDescribe;
    public string unitDescribe_detailed;
    [Tooltip("攻击加成")]
    [SerializeField]
    public int attackPower = 1;
    [Tooltip("护甲加成")]
    [SerializeField]
    public int armorPower = 1;
    [Tooltip("生命加成")]
    [SerializeField]
    public int healthPower = 1;
    [Tooltip("魔法加成")]
    [SerializeField]
    public int magicPower = 1;
}