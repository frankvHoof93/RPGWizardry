using UnityEngine;

namespace nl.SWEG.Willow.GameWorld.OpacityManagement
{
    public interface IOpacity
    {
        float OpacityRadius { get; }
        int OpacityPriority { get; }
        Vector2 OpacityOffset { get; }
    }
}