
namespace misery.components;
public class ChangeFormButton: Button
{
    Form _current, _movedTo;

    public ChangeFormButton(Form current, Form movedTo)
    {
        _current = current;
        _movedTo = movedTo;
        Text = movedTo.Text;
    }

    protected override void OnClick(EventArgs e)
    {
        _movedTo.Show();
        _current.Hide();
        _movedTo.FormClosed += (s, e) => _current.Close();
    }
}
