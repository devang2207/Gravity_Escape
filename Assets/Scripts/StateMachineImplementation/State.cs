public abstract class State 
{
    protected MonoClass MonoClassRef;
    public State(MonoClass monoClass) 
    {
        this.MonoClassRef = monoClass;
    }
    public abstract void StartState();
    public abstract void UpdateState();
    public abstract void ExitState();

}
