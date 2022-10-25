using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.FormulaRecOV.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using FormulaRecognition;

namespace _4thIR.FormulaRecOV.Activities
{
    [LocalizedDisplayName(nameof(Resources.FormulaRecognition_DisplayName))]
    [LocalizedDescription(nameof(Resources.FormulaRecognition_Description))]
    public class FormulaRecognition : ContinuableAsyncCodeActivity
    {
        private static readonly FormulaRecognizerOV _recognizer=new FormulaRecognizerOV();

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

        [LocalizedDisplayName(nameof(Resources.FormulaRecognition_Path_DisplayName))]
        [LocalizedDescription(nameof(Resources.FormulaRecognition_Path_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Path { get; set; }

        [LocalizedDisplayName(nameof(Resources.FormulaRecognition_FormulaType_DisplayName))]
        [LocalizedDescription(nameof(Resources.FormulaRecognition_FormulaType_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<FormulaType> FormulaType { get; set; }

        [LocalizedDisplayName(nameof(Resources.FormulaRecognition_Formula_DisplayName))]
        [LocalizedDescription(nameof(Resources.FormulaRecognition_Formula_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Formula { get; set; }

        [LocalizedDisplayName(nameof(Resources.FormulaRecognition_Score_DisplayName))]
        [LocalizedDescription(nameof(Resources.FormulaRecognition_Score_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<double> Score { get; set; }

        #endregion


        #region Constructors

        public FormulaRecognition()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Path == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Path)));
            if (FormulaType == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(FormulaType)));

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
                Formula.Set(ctx, task.Result.Item1);
                Score.Set(ctx, task.Result.Item2);
            };
        }

        private async Task<Tuple<string,double>> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var path = Path.Get(context);
            var formulaType = FormulaType.Get(context);
            var res =await _recognizer.DetectFormula(path, formulaType);

            return res;
        }

        #endregion
    }
}

