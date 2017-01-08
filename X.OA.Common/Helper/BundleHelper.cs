//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Optimization;

//namespace X.OA.Common.Helper
//{
//    public class CombineBundle : Bundle
//    {
//        IList<Bundle> bundles = new List<Bundle>();

//        public CombineBundle(string virtualPath) : this(virtualPath, null)
//        {

//        }

//        public CombineBundle(string virtualPath, string cdnPath) : base(virtualPath, cdnPath)
//        {
//            base.ConcatenationToken = ";" + Environment.NewLine;
//        }


//        public CombineBundle Include(Bundle bundle, bool EnableOptimizations = true)
//        {
//            if (!EnableOptimizations) bundle.Transforms.Clear();
//            bundles.Add(bundle);
//            return this;
//        }

//        //public override BundleResponse GenerateBundleResponse(BundleContext context)
//        //{
//        //    if (context == null)
//        //    {
//        //        throw new ArgumentNullException("context");
//        //    }

//        //    StringBuilder content = new StringBuilder();
//        //    IEnumerable<BundleFile> files = EnumerateFiles(context);

//        //    foreach (Bundle bundle in bundles)
//        //    {
//        //        BundleResponse _response = bundle.GenerateBundleResponse(context);
//        //        content.Append(_response.Content);
//        //        files.Concat(_response.Files);
//        //    }
//        //    return new BundleResponse(content.ToString(), files);
//        //}


//        //public override BundleResponse GenerateBundleResponse(BundleContext context)
//        //{
//        //    List<BundleFile> allFiles = new List<BundleFile>();
//        //    StringBuilder content = new StringBuilder();
//        //    string contentType = null;

//        //    foreach (Bundle b in bundles)
//        //    {
//        //        var r = b.GenerateBundleResponse(context);
//        //        content.Append(r.Content);

//        //        // 考虑到 BundleBundle 可能用于 CSS，所以这里进行一次判断，
//        //        // 只在 ScriptBundle 后面加分号（兼容 ASI 风格脚本）
//        //        // 这里可能会出现在已有分号的代码后面加分号的情况，
//        //        // 考虑到只会浪费 1 个字节，忍了
//        //        if (b is ScriptBundle)
//        //        {
//        //            content.Append(';');
//        //        }
//        //        content.AppendLine();

//        //        allFiles.AddRange(r.Files);
//        //        if (contentType == null)
//        //        {
//        //            contentType = r.ContentType;
//        //        }
//        //    }

//        //    var response = new BundleResponse(content.ToString(), allFiles);
//        //    response.ContentType = contentType;
//        //    return response;
//        //}
//    }
//}
