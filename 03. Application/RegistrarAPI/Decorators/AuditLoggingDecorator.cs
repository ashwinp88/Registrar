using Domain.Commands;
using Newtonsoft.Json;

namespace RegistrarAPI.Decorators
{
    public class AuditLoggingDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> commandHandler;

        public AuditLoggingDecorator(ICommandHandler<TCommand> commandHandler)
        {
            this.commandHandler = commandHandler;
        }
        public async Task HandleAsync(TCommand command)
        {
            var log = JsonConvert.SerializeObject(command);
            Console.WriteLine(log);
            await  commandHandler.HandleAsync(command);
        }
    }
}
