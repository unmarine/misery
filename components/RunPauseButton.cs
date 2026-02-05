namespace misery.components;

public sealed class RunPauseButton : Button
{
        private readonly System.Windows.Forms.Timer _timer;
        private bool _isOnPause = true;

        public RunPauseButton(System.Windows.Forms.Timer timer)
        {
                _timer = timer;
                Text = @"Run";
        }

        protected override void OnClick(EventArgs e)
        {
                _isOnPause = !_isOnPause;
                if (_isOnPause)
                {
                        Text = @"Run";
                        _timer.Stop();
                }
                else
                {
                        Text = @"Pause";
                        _timer.Start();
                }
        }
}