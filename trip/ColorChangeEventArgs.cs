using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace trip
{
    public class ColorChangeEventArgs : EventArgs
    {
        private Color _color;

        public ColorChangeEventArgs()
        { }
        public ColorChangeEventArgs(Color color)
        {
            this._color = color;
        }

        public Color Color
        {
            get{return _color;}
            set{this._color = value;}
        }
    }

}
