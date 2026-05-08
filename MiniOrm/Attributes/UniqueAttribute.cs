using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UniqueAttribute : Attribute { }
