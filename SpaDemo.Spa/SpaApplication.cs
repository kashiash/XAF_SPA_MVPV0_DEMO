using System;
using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using System.ComponentModel;
using DevExpress.ExpressApp.Mobile;
using System.Collections.Generic;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Spa;
using DevExpress.ExpressApp.Spa.AspNetCore;

namespace SpaDemo.Spa {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWebWebApplicationMembersTopicAll.aspx
    public partial class SpaDemoSpaApplication : SpaApplication {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Spa.SystemModule.SystemSpaModule module2;
        private Module.Spa.SpaDemoSpaModule spaDemoSpaModule1;
        private SpaDemo.Module.SpaDemoModule module3;

        #region Default XAF configuration options (https://www.devexpress.com/kb=T501418)
        static SpaDemoSpaApplication() {
            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
            DevExpress.ExpressApp.Mobile.MobileApplication.EnableExtendedDetailViewLayout = true;
        }
        private void InitializeDefaults() {
            LinkNewObjectToParentImmediately = false;
        }
        #endregion
        // public SpaDemoSpaApplication() {
        public SpaDemoSpaApplication(ISpaApplicationConfigProvider spaApplicationConfigProvider) : base(spaApplicationConfigProvider)
        {
            Tracing.Initialize();
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
            InitializeComponent();
            InitializeDefaults();
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached && CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProvider = new XPObjectSpaceProvider(GetDataStoreProvider(args.ConnectionString, args.Connection), true);
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }
        private IXpoDataStoreProvider GetDataStoreProvider(string connectionString, System.Data.IDbConnection connection) {
            //System.Web.HttpApplicationState application = (System.Web.HttpContext.Current != null) ? System.Web.HttpContext.Current.Application : null;
            //IXpoDataStoreProvider dataStoreProvider = null;
            //if(application != null && application["DataStoreProvider"] != null) {
            //    dataStoreProvider = application["DataStoreProvider"] as IXpoDataStoreProvider;
            //}
            //else {
            //    dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, connection, true);
            //    if(application != null) {
            //        application["DataStoreProvider"] = dataStoreProvider;
            //    }
            //}
            return XPObjectSpaceProvider.GetDataStoreProvider(connectionString, connection, true); ;
        }
        private void SpaDemoMobileApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
#if EASYTEST
            e.Updater.Update();
            e.Handled = true;
#else
            if (System.Diagnostics.Debugger.IsAttached) {
                e.Updater.Update();
                e.Handled = true;
            }
            else {
                string message = "The application cannot connect to the specified database, " +
                    "because the database doesn't exist, its version is older " +
                    "than that of the application or its schema does not match " +
                    "the ORM data model structure. To avoid this error, use one " +
                    "of the solutions from the https://www.devexpress.com/kb=T367835 KB Article.";

                if (e.CompatibilityError != null && e.CompatibilityError.Exception != null) {
                    message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
                }
                throw new InvalidOperationException(message);
            }
#endif
        }
        private void InitializeComponent() {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Spa.SystemModule.SystemSpaModule();
            this.module3 = new SpaDemo.Module.SpaDemoModule();
            this.spaDemoSpaModule1 = new SpaDemo.Module.Spa.SpaDemoSpaModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // SpaDemoSpaApplication
            // 
            this.ApplicationName = "SpaDemo";
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.spaDemoSpaModule1);
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.SpaDemoMobileApplication_DatabaseVersionMismatch);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}
