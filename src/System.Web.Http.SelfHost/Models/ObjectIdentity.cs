using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace System.Web.Http.SelfHost.Models
{
    public abstract class ObjectIdentity
    {
        public ObjectIdentity()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] data = Encoding.Unicode.GetBytes(DateTime.Now.ToString());
            rng.GetBytes(data);
            this.Id = BitConverter.ToInt32(data,4);
            this.UniqieId = Guid.NewGuid();
        }
        public long Id { get; private set; }

        public Guid UniqieId { get; private set; }

    }
}
