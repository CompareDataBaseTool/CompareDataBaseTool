using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace CompareDataBaseTool
{
    public delegate void CheckBoxClickedHandler(bool state);
    public class DataGridViewCheckBoxColumnCellEventArgs : EventArgs
    {
        bool _bChecked;
        public DataGridViewCheckBoxColumnCellEventArgs(bool bChecked)
        {
            _bChecked = bChecked;
        }
        public bool Checked
        {
            get { return _bChecked; }
        }
    }
    class DatagridViewCheckBoxColumnCell:DataGridViewCheckBoxCell
    { 
        Point checkBoxLocation;
        Size checkBoxSize;
        bool _checked = false;
        Point _cellLocation = new Point();
        System.Windows.Forms.VisualStyles.CheckBoxState _cbState = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
        public event CheckBoxClickedHandler OnCheckBoxClicked;

        public DatagridViewCheckBoxColumnCell()
        {

        }
        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds,  int rowIndex,  DataGridViewElementStates dataGridViewElementState, object value, object formattedValue,  string errorText,
            DataGridViewCellStyle cellStyle,DataGridViewAdvancedBorderStyle advancedBorderStyle,  DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            Point p = new Point();
            Size s = CheckBoxRenderer.GetGlyphSize(graphics, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            p.X = cellBounds.Location.X + (cellBounds.Width / 2) - (s.Width / 2)-1;
            p.Y = cellBounds.Location.Y + (cellBounds.Height / 2) - (s.Height / 2)-1;
            _cellLocation = cellBounds.Location;
            checkBoxLocation = p;
            checkBoxSize = s;
            if (_checked)
                _cbState = System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal;
            else
                _cbState = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
            CheckBoxRenderer.DrawCheckBox(graphics, checkBoxLocation,_cbState);
        }
        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            Point p = new Point(e.X + _cellLocation.X, e.Y + _cellLocation.Y);
            if (p.X >= checkBoxLocation.X && p.X <= checkBoxLocation.X + checkBoxSize.Width
            && p.Y >= checkBoxLocation.Y && p.Y <= checkBoxLocation.Y + checkBoxSize.Height)
            {
                _checked = !_checked;
                if (OnCheckBoxClicked != null)
                {
                    OnCheckBoxClicked(_checked);
                    this.DataGridView.InvalidateCell(this);
                }
            }
            base.OnMouseClick(e);
        }

    }
}
