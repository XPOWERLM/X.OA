using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.OA.Model;
using X.OA.IBLL;
using Microsoft.Practices.Unity;
using static X.OA.Common.Helper.UnityHelper;
using static X.OA.Common.Helper.JsonHelper;
using X.OA.Common.Helper;
using X.OA.Workflow;
using System.Activities;
using X.OA.Workflow.Model;

namespace X.OA.Web.Controllers
{
    public class WFInstanceController : BaseController
    {

        #region Create required objects
        IWF_InstanceBLL wfInstanceBLL = container.Resolve<IWF_InstanceBLL>();
        IWF_TempBLL wfTempBLL = container.Resolve<IWF_TempBLL>();
        IWF_StepInfoBLL wfStepBLL = container.Resolve<IWF_StepInfoBLL>();
        IUserInfoBLL userBLL = container.Resolve<IUserInfoBLL>();
        #endregion
        // GET: WFInstance
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StartWorkflow(int id)
        {
            WF_Temp entityTemplate = wfTempBLL.Retrieve(i => i.ID == id).FirstOrDefault();
            ViewBag.WFTemplate = entityTemplate;
            IEnumerable<SelectListItem> userList = userBLL.Retrieve(u => true).Select(u => new SelectListItem { Selected = false, Text = u.UName, Value = u.ID.ToString() });
            ViewBag.userList = userList;
            return View();
        }

        [HttpPost]
        public ActionResult StartWorkflow(WF_Instance model)
        {
            // Workflow parameter
            IDictionary<string, object> dict = new Dictionary<string, object>
            {
                { "bookMarkName","Manager approval"}
            };
            WorkflowApplication wfApp = WorkflowHelper.CreateWorkflowApplication(new FinancialActivity(), dict);

            // Set value
            model.ApplicationId = wfApp.Id;
            model.StartedBy = userInfo.ID;
            model.SubTime = DateTime.Now;
            model.Status = 0;
            //model.WF_TempID = templateId;

            // response
            wfInstanceBLL.Create(model);

            #region Start approval

            WF_StepInfo startApproval = new WF_StepInfo
            {
                ChildStepID = 0,
                Comment = "开始财务审批",
                DelFlag = 0,
                IsEndStep = false,
                IsProcessed = true,
                IsStartStep = true,
                ParentStepID = -1,
                ProcessBy = Request["processBy"].ToInt32(),
                ProcessTime = DateTime.Now,
                Remark = "开始进行财务审批",
                SetpName = "开始节点",
                StepResult = (short)WorkflowState.Initial,
                SubTime = DateTime.Now,
                Title = "开始财务审批",
                WF_InstanceID = model.ID,
            };
            wfStepBLL.Create(startApproval);

            #endregion

            #region Manager approval

            WF_StepInfo managerApproval = new WF_StepInfo
            {
                ChildStepID = 0,
                Comment = "开始财务审批",
                DelFlag = 0,
                IsEndStep = false,
                IsProcessed = false,
                IsStartStep = false,
                ParentStepID = -1,
                ProcessBy = Request["processBy"].ToInt32(),
                ProcessTime = DateTime.Now,
                Remark = "开始进行财务审批",
                SetpName = "开始节点",
                StepResult = (short)WorkflowState.Initial,
                SubTime = DateTime.Now,
                Title = "开始财务审批",
                WF_InstanceID = model.ID,
            };
            wfStepBLL.Create(managerApproval);
            #endregion

            #region CFO approval
            WF_StepInfo CFOApproval = new WF_StepInfo
            {
                ChildStepID = 0,
                Comment = string.Empty,
                DelFlag = 0,
                IsEndStep = false,
                IsProcessed = false,
                IsStartStep = false,
                ParentStepID = managerApproval.ID,
                ProcessBy = int.Parse(Request["userList"]),
                ProcessTime = DateTime.Now,
                Remark = string.Empty,
                SetpName = "总监审批",
                StepResult = (short)WorkflowState.Initial,
                SubTime = DateTime.Now,
                Title = string.Empty,
                WF_InstanceID = model.ID,
            };
            wfStepBLL.Create(CFOApproval);
            #endregion




            bool result = wfInstanceBLL.SaveChanges() > 0;
            return JsonNT(new { result = result, msg = result ? "Success" : "Failed" });
        }

        public ActionResult Approval()
        {
            int userId = userInfo.ID;
            IEnumerable<WF_StepInfo> approvalSteps = wfStepBLL.Retrieve(s => s.ProcessBy == userId && !s.IsProcessed);
            IEnumerable<WF_Instance> instances = approvalSteps.Select(s => s.WF_Instance);
            return View(instances);
        }

        public ActionResult StartApprove(int instanceId)
        {
            WF_Instance instance = wfInstanceBLL.Retrieve(i => i.ID == instanceId).FirstOrDefault();
            ViewBag.startUser = userBLL.Retrieve(u => u.ID == instance.StartedBy).FirstOrDefault().UName;
            ViewBag.userList = userBLL.Retrieve(u => true).Select(u => new SelectListItem { Selected = false, Text = u.UName, Value = u.ID.ToString() });
            return View(instance);
        }
    }
}