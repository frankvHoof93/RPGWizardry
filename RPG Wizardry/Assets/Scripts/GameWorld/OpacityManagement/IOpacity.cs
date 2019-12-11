using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld.OpacityManagement
{
    public interface IOpacity
    {
        float OpacityRadius { get; }
        int OpacityPriority { get; }
        Vector2 OpacityOffset { get; }
    }
}