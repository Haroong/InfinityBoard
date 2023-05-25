namespace InfinityBoard.Model.Config
{
    public class ServiceAutoConfig
    {
        public static void Configure(IServiceCollection services)
        {

            var service = typeof(IService);
            var types = AppDomain.CurrentDomain.GetAssemblies().Where(w => w.GetName().Name.Equals("BigglzPetJson.Dll")).SelectMany(s => s.GetTypes()).Where(p => (service.IsAssignableFrom(p)) && p.IsClass && !p.IsAbstract).ToList();

            types.ForEach(c =>
            {
                var originInterfaces = c.GetInterfaces();
                var interfaces = originInterfaces.Where(x =>
                        x.Name != service.Name
                    ).ToList();

                interfaces.ForEach(i =>
                {
                    services.AddTransient(i, c);
                });
            });
        }
    }

    internal interface IService
    {
    }
}
