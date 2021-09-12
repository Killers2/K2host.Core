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

    public class OVirtualEmailAttachment
    {

        public string FileName { get; set; }
        public byte[] Data { get; set; }

        public OVirtualEmailAttachment()
        {
            FileName = string.Empty;
        }

        public MemoryStream ToStream()
        {
            return new MemoryStream(Data);
        }

    }

}
