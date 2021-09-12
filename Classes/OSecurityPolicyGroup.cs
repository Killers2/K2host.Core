/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2018-10-30                        | 
'| Use: General                                         |
' \====================================================/
*/

namespace K2host.Core.Classes
{

    public class OSecurityPolicyGroup : System.ComponentModel.DescriptionAttribute
    {
        private readonly string v;

        public string PolicyGroup { get { return v; } }

        public OSecurityPolicyGroup(string v) : base(v)
        {
            this.v = v;
        }

    }

}
