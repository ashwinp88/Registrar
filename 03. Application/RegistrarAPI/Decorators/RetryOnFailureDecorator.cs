using Domain.Commands;

namespace RegistrarAPI.Decorators
{
    public class RetryOnFailureDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> commandHandler;

        public RetryOnFailureDecorator(ICommandHandler<TCommand> commandHandler)
        {
            this.commandHandler = commandHandler;
        }
        public async Task HandleAsync(TCommand command)
        {
            for (int i = 0;i < 3; i++)
            {
                try
                {
                    await commandHandler.HandleAsync(command);
                    break;
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
