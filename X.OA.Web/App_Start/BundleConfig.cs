using System.Web;
using System.Web.Optimization;

namespace X.OA.Web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Single

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                           "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-").Include(
                        "~/Scripts/jquery-1-11-3.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundle/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            //@Scripts.Render("~/bundle/bootstrap")

            bundles.Add(new ScriptBundle("~/bundle/unobtrusive-ajax").Include(
                "~/Scripts/jquery.unobtrusive-ajax.js"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/default/css").Include(
                "~/Content/themes/default/easyui.css"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/easyuicon").Include(
                "~/Content/themes/icon.css"
                ));
            //@Styles.Render("~/Content/themes/default/css", "~/Content/themes/easyuicon")

            bundles.Add(new StyleBundle("~/Content/themes/base/jquery-ui").Include(
                "~/Content/themes/base/jquery-ui.min.css"
                ));
            //@Styles.Render("~/Content/themes/base/jquery-ui")

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            //@Styles.Render("~/Content/css")

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css"
                ));

            #endregion

            #region Combined Styles

            bundles.Add(new StyleBundle("~/Content/bootstrapjquery-ui").Include(
               "~/Content/bootstrap.css",
                "~/Content/themes/base/jquery-ui.min.css"
               ));
            //@Styles.Render("~/Content/bootstrapjquery-ui")
            #endregion

            #region Combined Scripts
            Bundle easyui = new ScriptBundle("~/bundles/easyui").Include(
              "~/Scripts/jquery-easyui-1-4-5.js",
              "~/Scripts/locale/easyui-lang-zh_CN.js"
              );
            easyui.Transforms.Clear();
            bundles.Add(easyui);

            bundles.Add(new ScriptBundle("~/bundles/jqueryAjax").Include(
                        "~/Scripts/jquery-1-11-3.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"
                        ));
            //@Scripts.Render("~/bundles/jqueryAjax")

            bundles.Add(new ScriptBundle("~/bundles/jqueryBootstrap").Include(
                        "~/Scripts/jquery-1-11-3.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"
                        ));
            //@Styles.Render("~/Content/bootstrap")
            //@Scripts.Render("~/bundles/jqueryBootstrap")


            bundles.Add(new ScriptBundle("~/bundles/jqueryBootstrapjq-ui").Include(
                        "~/Scripts/jquery-1-11-3.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/jquery-ui-1.12.1.min.js"
                        ));
            //@Styles.Render("~/Content/bootstrapjquery-ui")
            //@Scripts.Render("~/bundles/jqueryBootstrapjq-ui")

            bundles.Add(new ScriptBundle("~/bundles/jqueryAjaxValidate").Include(
                        "~/Scripts/jquery-1-11-3.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.validate*"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryAjaxValidate").Include(
                        "~/Scripts/jquery-1-11-3.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.validate*"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryAjaxBootstrap").Include(
                        "~/Scripts/jquery-1-11-3.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"
                        ));
            //@Scripts.Render("~/bundles/jqueryAjaxBootstrap")
            bundles.Add(new ScriptBundle("~/bundles/jqueryAjaxBootstrapValidate").Include(
                        "~/Scripts/jquery-1-11-3.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/jquery.validate*"
                        ));
            #endregion

            #region Common

            bundles.Add(new ScriptBundle("~/Scripts/Common/AjaxUpload").Include(
                "~/Scripts/ViewScripts/Common.AjaxUpload.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/Common/CAPTCHA").Include(
                "~/Scripts/ViewScripts/Common.CAPTCHA.js"
                ));

            #endregion

            #region ViewScripts

            // ActionInfo
            bundles.Add(new ScriptBundle("~/Scripts/ActionInfo/Edit").Include(
                "~/Scripts/ViewScripts/ActionInfo.Edit.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/ActionInfo/Index").Include(
                "~/Scripts/ViewScripts/ActionInfo.Index.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/ActionInfo/SetRole").Include(
                "~/Scripts/ViewScripts/ActionInfo.SetRole.js"
                ));

            // Home
            bundles.Add(new ScriptBundle("~/Scripts/Home/Index").Include(
               "~/Scripts/ViewScripts/Home.Index.js"
               ));

            // Password
            bundles.Add(new ScriptBundle("~/Scripts/Password/Index").Include(
               "~/Scripts/ViewScripts/Password.Index.js"
               ));

            bundles.Add(new ScriptBundle("~/Scripts/Password/Verify").Include(
               "~/Scripts/ViewScripts/Password.Verify.js"
               ));

            // RoleInfo
            bundles.Add(new ScriptBundle("~/Scripts/RoleInfo/Index").Include(
               "~/Scripts/ViewScripts/RoleInfo.Index.js"
               ));

            // SignIn
            bundles.Add(new ScriptBundle("~/Scripts/SignIn/Index").Include(
               "~/Scripts/ViewScripts/SignIn.Index.js"
               ));

            bundles.Add(new ScriptBundle("~/Scripts/SignUp/Index").Include(
               "~/Scripts/ViewScripts/SignUp.Index.js"
               ));

            // UserInfo
            bundles.Add(new ScriptBundle("~/Scripts/UserInfo/Index").Include(
               "~/Scripts/ViewScripts/UserInfo.Index.js"
               ));

            bundles.Add(new ScriptBundle("~/Scripts/UserInfo/SetAction").Include(
               "~/Scripts/ViewScripts/UserInfo.SetAction.js"
               ));

            bundles.Add(new ScriptBundle("~/Scripts/UserInfo/SetRole").Include(
               "~/Scripts/ViewScripts/UserInfo.SetRole.js"
               ));

            // Search
            bundles.Add(new ScriptBundle("~/Scripts/Search/Index").Include(
               "~/Scripts/ViewScripts/Search.Index.js"
               ));

            #endregion
        }
    }
}
