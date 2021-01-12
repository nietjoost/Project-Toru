using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ConditionHandlerDelegate(LevelCondition condition);

public class LevelCondition
{
    public LevelCondition()
    {

    }

    // Name instance
    private string _name = "";
    public string name
    {
        get
        {
            return _name;
        }
        set
        {
            if (_name != "")
            {
                Debug.LogWarning("LevelCondition name cannot be changed, define a new one if needed. -> '" + value + "'");
                return;
            }

            if (value == "")
            {
                Debug.LogWarning("LevelCondition name can not be empty.");
                return;
            }

            if (LevelManager.Condition(value) != null)
            {
                Debug.LogWarning("LevelCondition name already used. -> '" + value + "'");
                return;
            }

            _name = value;
        }
    }

    protected bool _fullfilled = false;
    public bool fullfilled
    {
        get
        {
            return _fullfilled;
        }
    }

    protected bool _failed = false;
    public bool failed
    {
        get
        {
            return _failed;
        }
    }

    public ConditionHandlerDelegate fullfillHandler = null;
    public ConditionHandlerDelegate failHandler = null;

    public virtual void Fullfill()
    {	
		if (_fullfilled == true)
			return;
			
        Debug.Log(name + " fullfilled");
        _fullfilled = true;

        fullfillHandler?.Invoke(this);
    }

    public void Fail()
    {	
		if (_failed == true)
			return;
			
        Debug.Log(name + " failed");
        _failed = true;

        failHandler?.Invoke(this);
    }

    public void Revoke()
    {
        LevelManager.RemoveCondition(this.name);
    }
}

public class LevelConditionInt : LevelCondition
{
    public int value;
    public int targetValue = -1;

    public override void Fullfill()
    {
        value++;
        if (value == targetValue)
        {
            _fullfilled = true;
            fullfillHandler?.Invoke(this);
        }
    }
}