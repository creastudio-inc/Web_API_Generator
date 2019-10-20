using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class DataField
    {
        private String _name;
        private String _type;
        private String _lengh;
        private bool _nullable;
        private String _parametersName;
               
        public String Name { get { return _name; } set { _name = value; } }

        public String Type { get { return _type; } set { _type = value; } }

        public String Lengh { get { return _lengh; } set { _lengh = value; } }

        public bool Nullable { get { return _nullable; } set { _nullable = value; } }

        public String ParametersName { get { return _parametersName; } set { _parametersName = value; } }




    }
}
