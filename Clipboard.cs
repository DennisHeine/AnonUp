using System.Threading;

public static class Clipboard
{
    public static void SetText(string p_Text)
    {
        Thread STAThread = new Thread(
            delegate ()
            {
                System.Windows.Forms.Clipboard.SetText(p_Text);
            });
        STAThread.SetApartmentState(ApartmentState.STA);
        STAThread.Start();
        STAThread.Join();
    }
    public static string GetText()
    {
        string ReturnValue = string.Empty;
        Thread STAThread = new Thread(
            delegate ()
            {
                ReturnValue = System.Windows.Forms.Clipboard.GetText();
            });
        STAThread.SetApartmentState(ApartmentState.STA);
        STAThread.Start();
        STAThread.Join();

        return ReturnValue;
    }
}