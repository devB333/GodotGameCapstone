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
    /**
     * This is the entry method that an interactable button will use for a simple state change
     * Like clicking a door and just opening or closing it
     */
    public abstract void parentTriggerStateChange();
}
