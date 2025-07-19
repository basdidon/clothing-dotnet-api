using MassTransit;

namespace SharedLibrary.Masstransit
{
    public class LoggingConsumeObserver() : IConsumeObserver
    {
        public Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            Console.WriteLine($"[{typeof(T).Name}] consumed by {context.ReceiveContext.InputAddress}");
            return Task.CompletedTask;
        }

        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            Console.WriteLine($"[{typeof(T).Name}] was successfully consumed.");
            return Task.CompletedTask;
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            Console.WriteLine($"[{typeof(T).Name}] failed with exception: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
