using misery.eng;

namespace misery.components.combiners;

public class ColorPairsControls
{
        private readonly Button _colorPicker;
        private readonly ListBox _pairsListBox;
        private readonly NumericUpDown _statePicker;
        private readonly Button _submitButton;
        private Color _selectedColor = Color.White;

        public ColorPairsControls(NumericUpDown statePicker, Button colorPicker, Button submitButton,
                ListBox pairsListBox)
        {
                _statePicker = statePicker;
                _colorPicker = colorPicker;
                _submitButton = submitButton;
                _pairsListBox = pairsListBox;

                InitializeComponents();
                ReloadDisplay();
        }

        private void InitializeComponents()
        {
                _submitButton.Text = @"Add";
                _submitButton.Click += OnSubmitButtonClick;

                _pairsListBox.IntegralHeight = false;

                _colorPicker.Text = @"Select Color";
                _colorPicker.Click += OnColorPickerClick;
        }

        private void OnColorPickerClick(object? sender, EventArgs e)
        {
                if (sender == null) return;

                using var colorDialog = new ColorDialog();
                if (colorDialog.ShowDialog() == DialogResult.OK) _selectedColor = colorDialog.Color;
        }

        private void ReloadDisplay()
        {
                _pairsListBox.Items.Clear();

                foreach (var pair in Settings.ColorByStateValue)
                        _pairsListBox.Items.Add($"{pair.Key} {pair.Value.Name}");
        }

        private void OnSubmitButtonClick(object? sender, EventArgs e)
        {
                if (sender == null) return;

                Settings.SetColorForState((int)_statePicker.Value, _selectedColor);
                ReloadDisplay();
                if (Settings.DisplayedGrid != null)
                        Settings.DisplayedGrid.Invalidate();
        }
}