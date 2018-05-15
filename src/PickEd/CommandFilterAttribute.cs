using System;

namespace PickEd
{
    
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandFilterAttribute : Attribute
    {
        public string Prefix { get; set; }
    }
}
