using misery.Components;
using misery.Eng;

namespace misery.components;

public class RandomizeControls
{
        private readonly Grid _grid;
        
        private readonly Button _button;
        private readonly NumericUpDown _lower;
        private readonly NumericUpDown _upper;
        private readonly VisualGrid _visualGrid;
        
        public RandomizeControls(Grid grid, Button button, NumericUpDown lower, NumericUpDown upper, VisualGrid vg)
        {
                _grid = grid;
                _button = button;
                _lower = lower;
                _upper = upper;
                _visualGrid = vg;
                
                _button.Click += OnClick;
                _button.Text = @"Randomize";
        }
        
        private void OnClick(Object? sender, EventArgs e)
        {
                var random = new Random();
                for (var row = 0; row < _grid.Rows; row++)
                for (var column = 0; column < _grid.Columns; column++)
                {
                        var value = random.Next((int)_lower.Value, (int)_upper.Value + 1);
                        _grid.SetState(row, column, new State(value));
                }                
                _visualGrid.Invalidate();
        }
}