using System;

namespace Nebulas
{
    public class Neb
    {
        public HttpRequest Request { get; set; }

        public Admin Admin { get; set; }
        public API Api { get; set; }

        public Neb(HttpRequest request)
        {
            Request = request;
            Admin = new Admin(this);
            Api = new API(this);
        }

        public void SetRequest (HttpRequest request)
        {
            this.Request = request;
            this.Api.SetRequest(request);
            this.Admin.SetRequest(request);

        }
    }
}
