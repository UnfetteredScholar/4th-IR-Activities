using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.DocumentQuestionAnswer.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using _4thIR.ProcessScope.Activities;
using QuestionAnswering.Image.LayoutLMv2;
using _4thIR.ProcessScope.Activities.Activities;

namespace _4thIR.DocumentQuestionAnswer.Activities
{
    [LocalizedDisplayName(nameof(Resources.DocumentQuestionAnswering_DisplayName))]
    [LocalizedDescription(nameof(Resources.DocumentQuestionAnswering_Description))]
    public class DocumentQuestionAnswering : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.DocumentQuestionAnswering_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentQuestionAnswering_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentQuestionAnswering_Question_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentQuestionAnswering_Question_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Question { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentQuestionAnswering_Answer_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentQuestionAnswering_Answer_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Answer { get; set; }

        #endregion


        #region Constructors

        public DocumentQuestionAnswering()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Path == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Path)));
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
                Answer.Set(ctx, task.Result);
            };
        }

        private async Task<string> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var path = Path.Get(context);
            var question = Question.Get(context);
            DocumentImageQuestionAnswerer qAnswerer = new DocumentImageQuestionAnswerer(ProcessScope.Activities.ProcessScope.GetHttpClient());

            var res = await qAnswerer.AnswerQuestion(path, question);

            return res;
        }

        #endregion
    }
}

