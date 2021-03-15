namespace ModulR.Example.Console
{
    public class OrderSharedService : ISharedService
    {
        private readonly string parameter;

        public OrderSharedService(string parameter) => this.parameter = parameter;

        public string GetFrom() => $"Hello from Order, with: '{this.parameter}'";
    }

    public class OrderNewService : IOrderNewService
    {
        private readonly ISomeNewService service;

        public OrderNewService(ISomeNewService service) => this.service = service;

        public string GetFrom() => this.service.GetFrom();
    }

    public interface IOrderNewService
    {
        string GetFrom();
    }
}
