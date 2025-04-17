using System;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeMachine
{
    [Serializable]
    public struct UpgradeMachineButtonColor
    {
        [HideInInspector] public string name;
        [HideInInspector] public UpgradeMachineButtonState state;
        public ColorBlock color;
    }
}