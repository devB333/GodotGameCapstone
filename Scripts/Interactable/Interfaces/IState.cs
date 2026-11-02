using Godot;
using System;


public  abstract partial class IState: Node
{
 public Node node = null;
 public string name = "";
 
 public abstract void Enter();
 public abstract void Exit();
 public abstract void Process();

 public void switchState(string name)
 {
  if (node is IStateParent stateParent)
  {
    stateParent.activeState.Exit();
    stateParent.switch_state(name);
  }
 }
}

public interface IOptionalTriggerStateChange
{
    /**
    * This method is used if you need a trigger that just changes state
    */
    public abstract void triggerStateChange();
}

public interface IOptionalInput
{
    /**
     * This method is used if there is an input associated with a specific state. 
     */
    public abstract void input(InputEvent evt);
}
