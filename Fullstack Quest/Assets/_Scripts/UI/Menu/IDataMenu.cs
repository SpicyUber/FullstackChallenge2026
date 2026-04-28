using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IDataMenu<TData> : IGenericDataMenu where TData : class
{
    public void Load(TData data);
}

