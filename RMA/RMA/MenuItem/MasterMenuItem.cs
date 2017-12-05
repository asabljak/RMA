using System;
using System.Collections.Generic;
using System.Text;

namespace RMA.MenuItem
{
    class MasterMenuItem
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Type TargetType { get; set; }

        public MasterMenuItem(string Title, string MenuItem, Type targetType)
        {
            this.Title = Title;
            this.IconSource = IconSource;
            this.TargetType = targetType;
        }
    }
}
