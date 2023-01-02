using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.Custom.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using QuestionAnswering.Bert.Large;

namespace _4thIR.Custom.Activities
{
    [LocalizedDisplayName(nameof(Resources.DocumentQuestionAnsweringBertLarge_DisplayName))]
    [LocalizedDescription(nameof(Resources.DocumentQuestionAnsweringBertLarge_Description))]
    public class DocumentQuestionAnsweringBertLarge : FourthIRActivity
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

        [LocalizedDisplayName(nameof(Resources.DocumentQuestionAnsweringBertLarge_DocumentPath_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentQuestionAnsweringBertLarge_DocumentPath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> DocumentPath { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentQuestionAnsweringBertLarge_Question_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentQuestionAnsweringBertLarge_Question_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Question { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentQuestionAnsweringBertLarge_Answer_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentQuestionAnsweringBertLarge_Answer_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Answer { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentQuestionAnsweringBertLarge_Score_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentQuestionAnsweringBertLarge_Score_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<double> Score { get; set; }

        [LocalizedDisplayName(nameof(Resources.DocumentQuestionAnsweringBertLarge_Context_DisplayName))]
        [LocalizedDescription(nameof(Resources.DocumentQuestionAnsweringBertLarge_Context_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Context { get; set; }

        #endregion


        #region Constructors

        public DocumentQuestionAnsweringBertLarge()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (DocumentPath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(DocumentPath)));
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
                Context.Set(ctx, task.Result.Item3);
            };
        }

        private async Task<Tuple<string,double,string>> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var documentPath = DocumentPath.Get(context);
            var question = Question.Get(context);

            QuestionAnswererBert questionAnswerer = new QuestionAnswererBert(ProcessScope.GetHttpClient());

            var res = await questionAnswerer.AnswerQuestion(documentPath, question);

            return res;

        }

        #endregion
    }
}

