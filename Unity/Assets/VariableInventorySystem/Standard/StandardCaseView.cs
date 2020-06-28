using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VariableInventorySystem
{
    public class StandardCaseView : StandardStashView
    {
        public override void OnDroped(bool isDroped)
        {
            base.OnDroped(isDroped);
            ReApply();
        }
    }
}