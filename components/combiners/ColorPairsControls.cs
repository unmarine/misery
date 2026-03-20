using misery.Eng;

namespace misery.components.combiners
{
    public class ColorPairsControls
    {
        private NumericUpDown statePicker;
        private Button colorPicker;
        private Button submitButton;
        private ListBox pairsListBox;
        private Color selectedColor = Color.White;

        public ColorPairsControls(NumericUpDown statePicker, Button colorPicker, Button submitButton, ListBox pairsListBox)
        {
            this.statePicker = statePicker;
            this.colorPicker = colorPicker;
            this.submitButton = submitButton;
            this.pairsListBox = pairsListBox;

            InitializeComponents();
            ReloadDisplay();
        }

        private void InitializeComponents()
        {
            submitButton.Text = "Add";
            submitButton.Click += OnSubmitButtonClick;

            pairsListBox.IntegralHeight = false;

            colorPicker.Text = "Select Color";
            colorPicker.Click += OnColorPickerClick;
        }

        private void OnColorPickerClick(object? sender, EventArgs e)
        {
            if (sender == null) return;

            using ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedColor = colorDialog.Color;
            }
        }

        private void ReloadDisplay()
        {
            pairsListBox.Items.Clear();

            foreach (var pair in Settings.ColorByStateValue)
            {
                pairsListBox.Items.Add($"{pair.Key} {pair.Value.Name}");
            }
        }

        private void OnSubmitButtonClick(object? sender, EventArgs e)
        {
            if (sender == null) return;

            Settings.SetColorForState((int)statePicker.Value, selectedColor);
            ReloadDisplay();
            if (Settings.DisplayedGrid != null)
                Settings.DisplayedGrid.Invalidate();
        }
    }
}
