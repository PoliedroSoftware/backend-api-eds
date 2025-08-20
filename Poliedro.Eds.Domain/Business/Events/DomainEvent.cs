namespace Poliedro.Eds.Domain.Business.Events;

public class DomainEvent<T>
{
    private List<Action<T>> Actions { get; } = [];
    public void Register(Action<T> callback)
    {
        if (Actions.Exists(o => o.Method == callback.Method))
        {
            return;
        }
        Actions.Add(callback);
    }
    public void Publish(T args)
    {
        foreach (Action<T> action in Actions)
        {
            action.Invoke(args);
        }
    }
}
