
public abstract class BaseSate<T>
{
    public abstract void EnterState(T script);
    public abstract void Update(T script);
}
