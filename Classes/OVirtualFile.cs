/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2018-10-30                        | 
'| Use: General                                         |
' \====================================================/
*/

using System.IO;

namespace K2host.Core.Classes
{

    public class OVirtualFile
    {

        public string FileName { get; set; }
        public string MimeType { get; set; }
        public byte[] Data { get; set; }

        public OVirtualFile()
        {
            FileName = string.Empty;
            MimeType = string.Empty;
        }

        public FileInfo Dump(string fullFilePath)
        {

            File.WriteAllBytes(fullFilePath, Data);
            return new FileInfo(fullFilePath);

        }

        public MemoryStream ToStream()
        {
            var output = new MemoryStream(Data) { Position = 0 };
            output.Seek(0, SeekOrigin.Begin);
            return output;
        }

        public MemoryStream ToExpandableStream()
        {
            var output = new MemoryStream();
            output.Write(Data, 0, Data.Length);
            output.Position = 0;
            output.Seek(0, SeekOrigin.Begin);
            return output;
        }
    }

}
