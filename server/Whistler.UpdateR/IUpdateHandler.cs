using GTANetworkAPI;
using System.Threading.Tasks;

namespace Whistler.UpdateR
{
    public interface IUpdateHandler<TUpdate, TTarget> : IUpdateHandlerBase 
        where TUpdate : IUpdate<TTarget>
    {
        Task Handle(Player subscriber, TUpdate update);
    }

    public interface IUpdateHandlerBase { }
}
