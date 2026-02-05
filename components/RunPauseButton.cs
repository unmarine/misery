using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace misery.components;

public class RunPauseButton: Button
{
    private System.Windows.Forms.Timer _timer;
    private bool _isOnPause = true;

    public RunPauseButton(System.Windows.Forms.Timer timer)
    {
        _timer = timer;
        this.Text = "Run";
    }

    protected override void OnClick(EventArgs e)
    {
        if (_isOnPause)
        {
            _isOnPause = false;
            _timer.Start();
        } else
        {
            _isOnPause = true;
            _timer.Stop();
        }
    }
}
