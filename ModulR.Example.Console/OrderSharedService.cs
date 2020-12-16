namespace ModulR.Example.Console
{
    public class OrderSharedService : ISharedService
    {
        private readonly string parameter;

        public OrderSharedService(string parameter) => this.parameter = parameter;

        public string GetFrom() => $"Hello from Order, with: '{this.parameter}'";
    }
}
