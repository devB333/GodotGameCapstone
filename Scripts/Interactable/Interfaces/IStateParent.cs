using Godot;
using System;
using System.Collections.Generic;

public interface IStateParent
{
    public Guid id { get; }
    [Export]
    public Godot.Collections.Array<IState> states { get; }
    public Dictionary<string, IState> stateNameMap { get; }
    public IState activeState { get; set; }
    
    /**
     * This will switch the state on the state parent
     */
    public void switch_state(string stateName);

}

public interface IParentTriggerStateChange
{
    public abstract void parentTriggerStateChange();
}
