/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2018-10-30                        | 
'| Use: General                                         |
' \====================================================/
*/
namespace K2host.Core.Classes
{

    public class OAsyncReturn
    {

        public string ReturnUrl { get; set; }

        public OAsyncReturn() { }

        public override string ToString()
        {
            return ReturnUrl;
        }

    }

}
