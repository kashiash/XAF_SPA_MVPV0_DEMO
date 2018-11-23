using DevExpress.ExpressApp.Spa.AspNetCore;
using DevExpress.ExpressApp.Spa.AspNetCore.Mvc;
using System;
using System.Linq;

namespace SpaDemo.Spa.Controllers {
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class SpaDemoApplicationController : SpaApplicationController {
        public SpaDemoApplicationController(ISpaApplicationProvider applicationProvider) : base(applicationProvider) { }
    }

    //public class SpaDemoAuthenticationMvcController : XafAuthenticationMvcController {
    //    public SpaDemoAuthenticationMvcController(ISpaApplicationProvider applicationProvider, IXafSecurityAdapter xafSecurityAdapter) : base(applicationProvider, xafSecurityAdapter) { }
    //}
}
