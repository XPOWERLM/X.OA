using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace X.OA.Workflow
{

    public sealed class InputBookMark<T> : NativeActivity
    {
        // 定义一个字符串类型的活动输入参数
        public InArgument<string> BookMarkName { get; set; }
        public OutArgument<int> StepId { get; set; }
        public OutArgument<T> Result { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            string bookMarkName = context.GetValue(BookMarkName);
            context.CreateBookmark(bookMarkName, (con, mark, obj) =>
            {
                ResumeBookMarkModel<T> objValue = (ResumeBookMarkModel<T>)obj;
                con.SetValue(BookMarkName, objValue.BookMarkName);
                con.SetValue(StepId, objValue.StepId);
                con.SetValue(Result, objValue.Result);
                
            });
        }

        protected override bool CanInduceIdle
        {
            get
            {
                return true;
            }
        }
    }
}
