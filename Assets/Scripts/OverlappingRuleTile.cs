using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class OverlappingRuleTile: RuleTile
{

    public enum SiblingGroup
    {
        One,
        Two,
        Three,
        Ivy = 100
    }
    public SiblingGroup siblingGroup;

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is RuleOverrideTile)
            other = (other as RuleOverrideTile).m_InstanceTile;

        switch (neighbor)
        {
            case TilingRule.Neighbor.This:
                {
                    return other is OverlappingRuleTile
                        && (other as OverlappingRuleTile).siblingGroup == this.siblingGroup;
                }
            case TilingRule.Neighbor.NotThis:
                {
                    return !(other is OverlappingRuleTile
                        && (other as OverlappingRuleTile).siblingGroup == this.siblingGroup);
                }
        }

        return base.RuleMatch(neighbor, other);
    }
}