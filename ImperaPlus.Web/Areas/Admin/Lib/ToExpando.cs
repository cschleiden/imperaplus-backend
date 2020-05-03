using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace ImperaPlus.Backend.Areas.Admin.Lib
{
  public static class Expando
  {
    public static ExpandoObject ToExpando(this object anonymousObject)
    {
      IDictionary<string, object> expando = new ExpandoObject();
      foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(anonymousObject))
      {
        var obj = propertyDescriptor.GetValue(anonymousObject);
        expando.Add(propertyDescriptor.Name, obj);
      }

      return (ExpandoObject)expando;
    }
  }
}