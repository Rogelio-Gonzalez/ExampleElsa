using System.Threading;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Results;
using ExampleElsa.Models;
using Elsa.Services;
using Elsa.Services.Models;
using Elsa;
using System.Linq;
using System;

namespace ExampleElsa.Activities
{
    [ActivityDefinition(Category = "Users", Description = "Delete a User", Icon = "fas fa-user-minus", Outcomes = new[] { OutcomeNames.Done, "Not Found" })]
    public class DeleteUser : Activity
    {
        private readonly UserContext _context;

        public DeleteUser(UserContext context)
        {
            _context = context;
        }

        [ActivityProperty(Hint = "Enter an expression that evaluates to the ID of the user to activate.")]
        public WorkflowExpression<string> UserId
        {
            get => GetState<WorkflowExpression<string>>();
            set => SetState(value);
        }

        protected override async Task<ActivityExecutionResult> OnExecuteAsync(WorkflowExecutionContext context, CancellationToken cancellationToken)
        {
            var userId = await context.EvaluateAsync(UserId, cancellationToken);
            //var result = await _context.Users.DeleteOneAsync(x => x.Id == userId, cancellationToken);
            var getsito = await _context.Users.FindAsync(userId);
            if (getsito == null)
            {
                return Outcome("Not Found");
            }
            _context.Users.Remove(getsito);
            await _context.SaveChangesAsync(cancellationToken);
            return Done();
            //return result.DeletedCount == 0 ? Outcome("Not Found") : Done();
            /*var getUserId = await _context.Users.FindAsync(context.EvaluateAsync(UserId, cancellationToken));
            if (getUserId == null)
            {
                return Outcome("Not Found");
            }
            _context.Users.Remove(getUserId);
            await _context.SaveChangesAsync(cancellationToken);
            return Done();*/

        }
    }
}
