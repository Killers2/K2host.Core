/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2018-10-30                        | 
'| Use: General                                         |
' \====================================================/
*/

namespace K2host.Core.Classes
{

    public class OSecurityPolicyDescription : System.ComponentModel.DescriptionAttribute
    {
        private readonly string v;

        public string PolicyDescription { get { return v; } }

        public OSecurityPolicyDescription(string v) : base(v)
        {
            this.v = v;
        }

    }

}
