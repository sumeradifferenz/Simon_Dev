using System;
using Xamarin.Forms;

namespace Simon.Controls
{
    public class EditorWithAutoSize : Editor
    {
        public EditorWithAutoSize()
        {
            this.TextChanged += (sender, e) =>
            {
                this.InvalidateMeasure();
            };
        }
    }
}
