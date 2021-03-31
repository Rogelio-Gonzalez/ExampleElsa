using ExampleElsa.Activities;
using Microsoft.Extensions.DependencyInjection;
namespace ExampleElsa.Extensions
{
    public static class UserServiceCollectionExtensions
    {
        public static IServiceCollection AddUserActivities(this IServiceCollection services)
        {
            return services
                .AddActivity<CreateUser>()
                .AddActivity<ActivateUser>()
                .AddActivity<DeleteUser>();
            
        }
    }
}
