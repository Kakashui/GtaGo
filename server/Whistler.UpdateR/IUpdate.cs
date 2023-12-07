namespace Whistler.UpdateR
{
    public interface IUpdate<TTarget>
    {
        TTarget UpdateTarget { get; }
    }
}
