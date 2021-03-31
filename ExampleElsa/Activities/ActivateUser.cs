using System.Threading;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Results;
using ExampleElsa.Models;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.EntityFrameworkCore;
using Elsa;
namespace ExampleElsa.Activities
{
    [ActivityDefinition(Category = "Users", Description = "Activate a User", Icon = "fas fa-user-check", Outcomes = new[] { OutcomeNames.Done, "Not Found" })]
    public class ActivateUser : Activity
    {
        private readonly UserContext _context;
        public ActivateUser(UserContext context)
        {
            _context = context;
        }
        [ActivityProperty(Hint = "Enter an expression that evaluates to the ID of the user to activate.")]
        public WorkflowExpression<string> UserId
            //UserModel
        {
            get => GetState<WorkflowExpression<string>>();
            set => SetState(value);
        }

        protected override async Task<ActivityExecutionResult> OnExecuteAsync(WorkflowExecutionContext context, CancellationToken cancellationToken)
        {
            var userId = await context.EvaluateAsync(UserId, cancellationToken);
            var user = await _context.Users.AsQueryable().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
                return Outcome("Not Found");

            user.IsActive = true;
            //user.Email = user.Email;
            //user.Name = user.Name;
            //user.Id = user.Id;
            await _context.SaveChangesAsync(cancellationToken: cancellationToken);
            return Done();
        }
    }
}