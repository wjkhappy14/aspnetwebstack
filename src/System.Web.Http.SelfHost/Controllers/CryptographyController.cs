using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections.Cryptographys;

namespace System.Web.Http.SelfHost.Controllers
{
    /*
        加密用于达到以下目的：
       •保密性：帮助保护用户的标识或数据不被读取。
       •数据完整性：帮助保护数据不被更改。
       •身份验证：确保数据发自特定的一方。
       •不可否认性：防止特定的一方否认发送过消息。
   */


    /// <summary>
    /// Cryptography  密码使用法  https://msdn.microsoft.com/zh-cn/library/92f9ye3s(v=vs.100).aspx
    /// </summary>
    public class CryptographyController : ApiController
    {

        public CryptographyController()
        {
            //加密随机数生成器创建加密型强随机值。
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        }


        /// <summary>
        /// 对称算法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Symmetric()
        {
            var symmetric = SymmetricAlgorithm.Create();
            symmetric.Mode = CipherMode.CBC;//指定用于加密的块密码模式。
            return Json(symmetric);
        }





        /// <summary>
        /// 不对称算法
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IHttpActionResult Asymmetric()
        {
            var asymmetric = AsymmetricAlgorithm.Create();
            return Json(asymmetric);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Des()
        {
            DES des = DES.Create();
            des.Key = Encoding.UTF8.GetBytes("Hello Angkor");
            des.IV = Encoding.UTF8.GetBytes("Nice to meet you ");
            return Json(des);

        }
        [HttpGet]
        public IHttpActionResult AES(string content)
        {
            using (AesManaged aes = new AesManaged())
            {
                aes.CreateEncryptor();
                byte[] encrypted = CommonCrypto.EncryptStringToBytes_Aes(content, aes.Key, aes.IV);
                content = CommonCrypto.DecryptStringFromBytes_Aes(encrypted, aes.Key, aes.IV);
            }
            return Json(content);

        }


        /// <summary>
        /// RSA 算法
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IHttpActionResult Rsa()
        {
            var rsa = RSA.Create();
            var keyExchange = rsa.KeyExchangeAlgorithm;
            var signature = rsa.SignatureAlgorithm;
            return Json(rsa);
        }
    }
}
