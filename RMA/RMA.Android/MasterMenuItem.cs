using System;

namespace RMA
{
    public class MasterMenuItem
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Type TargetType { get; set; }

        public MasterMenuItem(string Title, string IconSource, Type TargetType)
        {
            this.Title = Title;
            this.IconSource = IconSource;
            this.TargetType = TargetType;
        }
    }
}