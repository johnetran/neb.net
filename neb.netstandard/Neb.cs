using System;

namespace Nebulas
{
    public class Neb
    {
        public NebRequest Request { get; set; }

        public Admin Admin { get; set; }
        public API Api { get; set; }

        public Neb(string host) : this(new NebRequest(host))
        {
        }

        public Neb(NebRequest request)
        {
            Request = request;
            Admin = new Admin(this);
            Api = new API(this);
        }

        public void SetRequest (NebRequest request)
        {
            this.Request = request;
            this.Api.SetRequest(request);
            this.Admin.SetRequest(request);

        }
    }
}
