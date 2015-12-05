using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace System.Web.Http.SelfHost.Streams
{
    /// <summary>
    /// 从服务器端读取视频
    /// </summary>
    public class VideoStream
    {
        private readonly string _filename;
        public VideoStream(string filename, string ext)
        {
            _filename = string.Format("{0}{1}", filename, ext);
        }
        public async void WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            try
            {

              var channelBinding=   context.GetChannelBinding(ChannelBindingKind.Endpoint);

                var buffer = new byte[65536];
                using (var video = File.Open(_filename, FileMode.Open, FileAccess.Read))
                {
                    var length = (int)video.Length;
                    var bytesRead = 1;
                    while (length > 0 && bytesRead > 0)
                    {
                        bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));
                        await outputStream.WriteAsync(buffer, 0, bytesRead);
                        length -= bytesRead;
                    }
                }
            }
            catch (HttpResponseException ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
            finally
            {
                outputStream.Close();
            }
        }

        private byte[] ReadFile(string fileName)
        {

            FileStream pFileStream = null;

            byte[] pReadByte = new byte[0];

            try
            {

                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                BinaryReader r = new BinaryReader(pFileStream);

                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开

                pReadByte = r.ReadBytes((int)r.BaseStream.Length);

                return pReadByte;

            }

            catch
            {

                return pReadByte;

            }

            finally
            {

                if (pFileStream != null)

                    pFileStream.Close();

            }

        }
    }
}
