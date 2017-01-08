using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using X.OA.Common.Properties;

namespace X.OA.Common.Helper
{
    public static class WorkflowHelper
    {
        public static WorkflowApplication CreateWorkflowApplication(Activity activity,IDictionary<string,object> dict)
        {
            WorkflowApplication wfApp = new WorkflowApplication(activity, dict);
            SqlWorkflowInstanceStore sqlStore = new SqlWorkflowInstanceStore(ConfigurationManager.ConnectionStrings[Resources.ConnStr_Workflow].ConnectionString);
            wfApp.InstanceStore = sqlStore;
            wfApp.PersistableIdle = _ => PersistableIdleAction.Unload;
            wfApp.OnUnhandledException = _ => UnhandledExceptionAction.Abort;
            wfApp.Run();
            return wfApp;
        }

        public static WorkflowApplication LoadWorkflowApplication(Activity activity,Guid guid)
        {
            WorkflowApplication wfApp = new WorkflowApplication(activity);
            SqlWorkflowInstanceStore sqlStore = new SqlWorkflowInstanceStore(ConfigurationManager.ConnectionStrings[Resources.ConnStr_Workflow].ConnectionString);
            wfApp.InstanceStore = sqlStore;
            wfApp.PersistableIdle = _ => PersistableIdleAction.Unload;
            wfApp.OnUnhandledException = _ => UnhandledExceptionAction.Abort;
            wfApp.Load(guid);
            return wfApp;
        }

    }
}