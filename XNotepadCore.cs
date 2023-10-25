using System.IO;
using System;
using System.Windows.Forms;
using System.Drawing;

internal class XNotepadCore
{
    private static string _savedFileLocation;
    private static bool _saved = false;
    private static int _defaultZoomValue = 1;

    //FileMenu
    public static void CreateNewWindow(Form form)
    {
        form.ShowDialog();
    }
    public static void CloseWindow(Form form)
    {
        form.Close();
    }
    public static void CreateNewFile(RichTextBox richTextBox, Form form)
    {
        richTextBox.Clear();
        form.Text = "Untitled";
        _saved = false;
    }
    public static void SaveFile(RichTextBox richTextBox, SaveFileDialog saveFileDialog, Form form)
    {
        if (!_saved)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {

                using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
                {
                    _savedFileLocation = saveFileDialog.FileName;
                    streamWriter.Write(richTextBox.Text);
                }
                _saved = true;
                form.Text = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
            }
        }
        else
        {
            using (StreamWriter streamWriter = new StreamWriter(_savedFileLocation))
            {
                streamWriter.Write(richTextBox.Text);
            }
        }
    }
    public static void OpenFile(RichTextBox richTextBox, OpenFileDialog openFileDialog, Form form)
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
            {
                form.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                richTextBox.Text = streamReader.ReadToEnd();
            }
        }
    }
    public static void Exit()
    {
        Application.Exit();
    }
    
    //EditMenu
    public static void _edit_Clear(RichTextBox richTextBox)
    {
        richTextBox.Clear();
    }
    public static void _edit_Undo(RichTextBox richTextBox)
    {
        richTextBox.Undo();
    }
    public static void _edit_Redo(RichTextBox richTextBox)
    {
        richTextBox.Redo();
    }
    public static void _edit_Copy(RichTextBox richTextBox)
    {
        Clipboard.SetText(richTextBox.Text);
        MessageBox.Show("Copied!");
    }
    public static void _edit_Paste(RichTextBox richTextBox)
    {
        richTextBox.Text += "\n" + Clipboard.GetText();
    }
    public static void _edit_Date(RichTextBox richTextBox)
    {
        richTextBox.Text += DateTime.Now.ToString("g") + " ";
    }
    public static void SetRTextBoxZoomMore(RichTextBox richTextBox, int zoomMaxValue)
    {
        if (richTextBox.ZoomFactor < zoomMaxValue)
        {
            richTextBox.ZoomFactor += 1;
        }
    }
    public static void SetRTextBoxZoomLess(RichTextBox richTextBox)
    {
        if (richTextBox.ZoomFactor > 1)
        {
            richTextBox.ZoomFactor -= 1;
        }
    }
    public static void ResetRTextZoom(RichTextBox richTextBox)
    {
        richTextBox.ZoomFactor = _defaultZoomValue;
    }
    public static void ChangeFont(RichTextBox richTextBox, string fontName, int fontSize)
    {
        richTextBox.Font = new Font(fontName, fontSize);
    }
    public static void Search(RichTextBox richTextBox, string searchText)
    {
        richTextBox.SelectionStart = 0;
        richTextBox.SelectionLength = richTextBox.TextLength;
        richTextBox.SelectionBackColor = richTextBox.BackColor;

        if (!string.IsNullOrEmpty(searchText))
        {
            int _startIndex = 0;
            while (_startIndex < richTextBox.TextLength)
            {
                int _index = richTextBox.Find(searchText, _startIndex, RichTextBoxFinds.None);
                if (_index == -1) break;

                richTextBox.SelectionStart = _index;
                richTextBox.SelectionLength = searchText.Length;
                richTextBox.SelectionBackColor = Color.FromArgb(112, 79, 79);

                _startIndex = _index + searchText.Length;
            }
        }
        if (string.IsNullOrEmpty(searchText))
        {
            richTextBox.SelectionLength = 0;
        }

    }

    //Other
    public static string CountLines(RichTextBox richTextBox) 
    {
        return richTextBox.Text.Split('\n').Length.ToString();
    }
}
