using DevExpress.ExpressApp.Spa.AspNetCore;
using DevExpress.ExpressApp.Spa.AspNetCore.Mvc;

namespace SpaDemo.Spa.Controllers {
    public class SpaDemoSpaMetaDataController : DevExpress.ExpressApp.Spa.AspNetCore.Mvc.MetaDataController {
        public SpaDemoSpaMetaDataController(ISpaApplicationProvider applicationProvider) :
            base(applicationProvider) {
        }
    }
}
