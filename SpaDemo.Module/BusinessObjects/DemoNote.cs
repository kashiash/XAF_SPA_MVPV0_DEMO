using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaDemo.Module.BusinessObjects {
    [DefaultClassOptions]
    [ImageName("BO_Note")]
    [XafDisplayName("Note")]
    [XafDefaultProperty(nameof(Subject))]
    [ListViewFilter("Recently created", "IsThisWeek(CreatedOn)", Index = 0)]
    [ListViewFilter("Created this year", "IsThisYear(CreatedOn)", true, Index = 1)]
    [ListViewFilter("All notes", "1=1", Index = 2)]
    public class DemoNote : BaseObject {
        public const string SPA_DemoUser = "SPA Demo User";
        public DemoNote(Session session)
            : base(session) {
        }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Subject {
            get { return GetPropertyValue<string>(nameof(Subject)); }
            set { SetPropertyValue<string>(nameof(Subject), value); }
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            CreatedOn = DateTime.Now;
            Owner = GetCurrentUser();
        }
        private string GetCurrentUser() {
            return SPA_DemoUser;
        }
        protected override void OnSaving() {
            base.OnSaving();
            if(!Session.IsNewObject(this)) {
                LastModified = DateTime.Now;
            }
        }
        [ModelDefault("AllowEdit", "False")]
        [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime CreatedOn {
            get { return GetPropertyValue<DateTime>(nameof(CreatedOn)); }
            set { SetPropertyValue<DateTime>(nameof(CreatedOn), value); }
        }
        [ModelDefault("AllowEdit", "False")]
        [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime? LastModified {
            get { return GetPropertyValue<DateTime?>(nameof(LastModified)); }
            set { SetPropertyValue<DateTime?>(nameof(LastModified), value); }
        }
        [ModelDefault("AllowEdit", "False")]
        public string Owner {
            get { return GetPropertyValue<string>(nameof(Owner)); }
            set { SetPropertyValue<string>(nameof(Owner), value); }
        }
        // Dennis: For MVP V1 or later.
        //private PermissionPolicyUser GetCurrentUser() {
        //    return Session.GetObjectByKey<PermissionPolicyUser>(SecuritySystem.CurrentUserId);
        //}
        //[ModelDefault("AllowEdit", "False")]
        //public PermissionPolicyUser Owner {
        //    get { return GetPropertyValue<PermissionPolicyUser>(nameof(Owner)); }
        //    set { SetPropertyValue<PermissionPolicyUser>(nameof(Owner), value); }
        //}
        //public bool Completed {
        //    get { return GetPropertyValue<bool>(nameof(Completed)); }
        //    set { SetPropertyValue<bool>(nameof(Completed), value); }
        //}
        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get { return GetPropertyValue<string>(nameof(Description)); }
            set { SetPropertyValue<string>(nameof(Description), value); }
        }
    }
}
