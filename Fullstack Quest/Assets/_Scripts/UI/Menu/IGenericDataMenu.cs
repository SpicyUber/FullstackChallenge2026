using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public interface IGenericDataMenu
{
    public Type DataType { get; }

    public void Load(object data);
}

