namespace InventoryManagementSystem.CommandHandlers
{
    public interface ICommandHandler<TCommand>
    {
        Task Handle(TCommand command);
    }
}
