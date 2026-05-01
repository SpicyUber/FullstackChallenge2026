using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Menu<TData> : MenuBase, IDataMenu<TData> where TData : class
{
    public override Type DataType => typeof(TData);

    public override void Load(object data) => Load(data as TData);

    public abstract void Load(TData data);

    protected void CloseSelf() => UIManager.Instance.CloseMenu(DataType);
}

