using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.BaseImpl;
using SpaDemo.Module.BusinessObjects;
using System;
using System.Data;
using System.IO;

namespace SpaDemo.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        string[] noteDescriptions = {
            "works with customers until their problems are resolved and often goes an extra step to help upset customers be completely surprised by how far we will go to satisfy customers",
            "is very good at making team members feel included. The inclusion has improved the team's productivity dramatically",
            "is very good at sharing knowledge and information during a problem to increase the chance it will be resolved quickly",
            "actively elicits feedback from customers and works to resolve their problems",
            "creates an inclusive work environment where everyone feels they are a part of the team",
            "consistently keeps up on new trends in the industry and applies these new practices to every day work",
            "is clearly not a short term thinker - the ability to set short and long term business goals is a great asset to the company",
            "seems to want to achieve all of the goals in the last few weeks before annual performance review time, but does not consistently work towards the goals throughout the year",
            "does not yet delegate effectively and has a tendency to be overloaded with tasks which should be handed off to subordinates",
            "to be discussed with the top management..."
        };

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            string user1 = "Sam", user2 = "John";
            DataTable employeesTable = GetEmployeesDataTable();
            for(int i = 0; i < 10; i++) {
                DataRow employee = employeesTable.Rows[i];
                string employeeFullName = string.Format("{0} {1}", Convert.ToString(employee["FirstName"]), Convert.ToString(employee["LastName"]));
                string owner = i % 2 == 0 ? user1 : user2;
                DemoNote note = ObjectSpace.FindObject<DemoNote>(CriteriaOperator.Parse(string.Format("Contains({0}, '{1}')", nameof(DemoNote.Subject), employeeFullName)));
                if(note == null) {
                    note = ObjectSpace.CreateObject<DemoNote>();
                    string namePart = i % 3 == 0 ? "Quarterly" : "Annual";
                    note.Subject = string.Format("{0} - {1} Performance Review", employeeFullName, namePart);
                    note.Owner = owner;
                    note.Description = noteDescriptions[i];
                    note.CreatedOn = DateTime.Now.AddDays(i * (-2)).AddHours(i * (-2)).AddMinutes(i * (-4));
                    if(i % 3 == 0) {
                        note.LastModified = DateTime.Now.AddDays(i * (-2)).AddHours(i * (-2)).AddMinutes(i * (-4));
                    }
                }
            }
            ObjectSpace.CommitChanges();
        }
        private DataTable GetEmployeesDataTable() {
            string shortName = "DemoData.xml";
            string embeddedResourceName = Array.Find<string>(this.GetType().Assembly.GetManifestResourceNames(), (s) => { return s.Contains(shortName); });
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(embeddedResourceName);
            if(stream == null) {
                throw new Exception(string.Format("Cannot read employees data from the {0} file!", shortName));
            }
            DataSet ds = new DataSet();
            ds.ReadXml(stream);
            return ds.Tables["Employee"];
        }
    }

}
