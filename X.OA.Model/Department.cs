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
    public partial class Department
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Department()
        {
            this.ActionInfoes = new HashSet<ActionInfo>();
            this.UserInfoes = new HashSet<UserInfo>();
        }
    
        public int ID { get; set; }
        public string DepName { get; set; }
        public int ParentId { get; set; }
        public string TreePath { get; set; }
        public int Level { get; set; }
        public bool IsLeaf { get; set; }
    
        [Newtonsoft.Json.JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActionInfo> ActionInfoes { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserInfo> UserInfoes { get; set; }
    }
}
