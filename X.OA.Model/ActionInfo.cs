//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace X.OA.Model
{
    using System;
    using System.Collections.Generic;
    
    
    [Serializable]
    [System.ComponentModel.Description("X.OA.Model.OAEntities")]
    public partial class ActionInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ActionInfo()
        {
            this.R_UserInfo_ActionInfo = new HashSet<R_UserInfo_ActionInfo>();
            this.Departments = new HashSet<Department>();
            this.RoleInfoes = new HashSet<RoleInfo>();
        }
    
        public int ID { get; set; }
        public System.DateTime SubTime { get; set; }
        public short DelFlag { get; set; }
        public string ModifiedOn { get; set; }
        public string Remark { get; set; }
        public string Url { get; set; }
        public string HttpMethod { get; set; }
        public string ActionMethodName { get; set; }
        public string ControllerName { get; set; }
        public string ActionInfoName { get; set; }
        public string Sort { get; set; }
        public short ActionTypeEnum { get; set; }
        public string MenuIcon { get; set; }
        public Nullable<int> IconWidth { get; set; }
        public Nullable<int> IconHeight { get; set; }
    
        [Newtonsoft.Json.JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<R_UserInfo_ActionInfo> R_UserInfo_ActionInfo { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Department> Departments { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleInfo> RoleInfoes { get; set; }
    }
}
