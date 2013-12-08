using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProgram
{
    public class TwitterSettings
    {
        private string _accsToken;
        private string _accsSec;

        public string AccsToken
        {
            get {return _accsToken;}
            //set {_accsToken = value;}
        }
        public string AccsSec
        {
            get {return _accsSec;}
            //set {_accsSec = value;}
        }

        public TwitterSettings()
        {
            _accsToken = null;
            _accsSec = null;
        }
        public TwitterSettings(string accsToken,string accsSec)
        {
            _accsToken = accsToken;
            _accsSec = accsSec;
        }
    }
}
