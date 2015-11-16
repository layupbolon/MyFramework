using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Data
{
    public interface IEditable
    {
        DateTime InDate { get; set; }
        int InUser { get; set; }
        DateTime? EditDate { get; set; }
        int? EditUser { get; set; }
    }
}
