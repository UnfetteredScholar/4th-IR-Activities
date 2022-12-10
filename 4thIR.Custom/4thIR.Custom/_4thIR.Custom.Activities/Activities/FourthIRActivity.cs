using System.Activities;
using UiPath.Shared.Activities;
using _4thIR.Custom.Activities.Properties;

namespace _4thIR.Custom.Activities
{
    public abstract class FourthIRActivity: ContinuableAsyncCodeActivity
    {
        protected FourthIRActivity()
        {
            Constraints.Add(ActivityConstraints.HasParentType<FourthIRActivity, ProcessScope>(string.Format(Resources.ValidationScope_Error, typeof(ProcessScope).Name)));
        }
    }

    public abstract class FourthIRCodeActivity:CodeActivity
    {
        protected FourthIRCodeActivity()
        {
            Constraints.Add(ActivityConstraints.HasParentType<FourthIRCodeActivity, ProcessScope>(string.Format(Resources.ValidationScope_Error, typeof(ProcessScope).Name)));
        }
    }

}
