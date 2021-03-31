using System.Threading;
using System.Threading.Tasks;
using ExampleElsa.Models;
using Elsa.Scripting.Liquid.Messages;
using Fluid;
using MediatR;


namespace ExampleElsa.Handlers
{
    public class LiquidConfigurationHandler : INotificationHandler<EvaluatingLiquidExpression>
    { 
        public Task Handle(EvaluatingLiquidExpression notification, CancellationToken cancellationToken)
        {
            var context = notification.TemplateContext;
            context.MemberAccessStrategy.Register<UserModel>();
            context.MemberAccessStrategy.Register<RegistrationModel>();

            return Task.CompletedTask;
        }
    }
}
