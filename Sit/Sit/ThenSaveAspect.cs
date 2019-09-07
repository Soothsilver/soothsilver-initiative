using Plugin.Settings;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sit
{
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, typeof(RecordableAttribute))]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, typeof(NotifyPropertyChangedAttribute))]
    [PSerializable]
    class ThenSaveAttribute : OnMethodBoundaryAspect
    {
        public override void OnSuccess(MethodExecutionArgs args)
        {
            CrossSettings.Current.AddOrUpdateValue("Encounter", MainPage.Instance.Encounter.Serialize(), "thensave.txt");
        }
    }
}
