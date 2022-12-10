using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using QuestionAnswering.RobertaBaseSquad2;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.QuestionAnsweringRBS2_DisplayName))]
    [LocalizedDescription(nameof(Resources.QuestionAnsweringRBS2_Description))]
    public class QuestionAnsweringRBS2 : FourthIRActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.Timeout_DisplayName))]
        [LocalizedDescription(nameof(Resources.Timeout_Description))]
        public InArgument<int> TimeoutMS { get; set; } = 60000;

        [LocalizedDisplayName(nameof(Resources.QuestionAnsweringRBS2_Context_DisplayName))]
        [LocalizedDescription(nameof(Resources.QuestionAnsweringRBS2_Context_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Context { get; set; }

        [LocalizedDisplayName(nameof(Resources.QuestionAnsweringRBS2_Question_DisplayName))]
        [LocalizedDescription(nameof(Resources.QuestionAnsweringRBS2_Question_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Question { get; set; }

        [LocalizedDisplayName(nameof(Resources.QuestionAnsweringRBS2_Answer_DisplayName))]
        [LocalizedDescription(nameof(Resources.QuestionAnsweringRBS2_Answer_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Answer { get; set; }

        [LocalizedDisplayName(nameof(Resources.QuestionAnsweringRBS2_Score_DisplayName))]
        [LocalizedDescription(nameof(Resources.QuestionAnsweringRBS2_Score_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<double> Score { get; set; }

        #endregion


        #region Constructors

        public QuestionAnsweringRBS2()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Context == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Context)));
            if (Question == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Question)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var timeout = TimeoutMS.Get(context);


            // Set a timeout on the execution
            var task = ExecuteWithTimeout(context, cancellationToken);
            if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) != task) throw new TimeoutException(Resources.Timeout_Error);

            // Outputs
            return (ctx) => {
                Answer.Set(ctx, task.Result.Item1);
                Score.Set(ctx, task.Result.Item2);
            };
        }

        private async Task<Tuple<string, double>> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var question = Question.Get(context);
            var qcontext = Context.Get(context);
            QuestionAnswererRBS2 questionAns = new QuestionAnswererRBS2(ProcessScope.GetHttpClient());

            var res = await questionAns.AnswerQuestion(question, qcontext);

            return res;
        }

        #endregion
    }
}

