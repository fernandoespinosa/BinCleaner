using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BinCleaner
{
    public partial class Foo : Form
    {
        public Foo()
        {
            InitializeComponent();
            this.checkedListBox.CheckOnClick = true;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public DialogResult ShowDialog(params DirectoryOption[] directoryOptions)
        {
            foreach (var option in directoryOptions)
            {
                this.checkedListBox.SetItemChecked(this.checkedListBox.Items.Add(option.Name), option.Checked);
            }

            return base.ShowDialog();
        }

        public IEnumerable<string> SelectedDirectoryNames
        {
            get { return this.checkedListBox.CheckedItems.Cast<string>(); }
        }
    }

    public class DirectoryOption
    {
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}
