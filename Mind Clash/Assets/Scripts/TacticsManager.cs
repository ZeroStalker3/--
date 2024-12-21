using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsManager : MonoBehaviour
{
    private Unit selectedUnit;

    public void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
    }

    public void AttackTactic()
    {
        if (selectedUnit != null)
        {
            selectedUnit.SetTactic(Unit.Tactic.Attack);
        }
    }

    public void DefenseTactic()
    {
        if (selectedUnit != null)
        {
            selectedUnit.SetTactic(Unit.Tactic.Defense);
        }
    }

    public void RetreatTactic()
    {
        if (selectedUnit != null)
        {
            selectedUnit.SetTactic(Unit.Tactic.Retreat);
        }
    }
}
