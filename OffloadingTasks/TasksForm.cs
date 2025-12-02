namespace OffloadingTasks
{
    public partial class TasksForm : Form
    {
        // Thready affinity example without using Tasks
        public TasksForm()
        {
            InitializeComponent();
        }

        // When you click the buttons, the UI will freeze until the tasks are complete.
        // commented out that scenario and provided an alternative using Thread class.

        private void message1_Clicked(object sender, EventArgs e)
        {
            // ShowMessage("First Message", 3000);
            Thread thread = new Thread(() => ShowMessage("First Message", 3000));
            thread.Start();
        }

        private void message2_Clicked(object sender, EventArgs e)
        {
            // ShowMessage("Second Message", 5000);
            Thread thread = new Thread(() => ShowMessage("Second Message", 5000));
            thread.Start();
        }

        private void ShowMessage(string message, int delay)
        {
            Thread.Sleep(delay);

            if (lblMessage.InvokeRequired)
            {
                lblMessage.Invoke(() => lblMessage.Text = message); // Synchronize with the UI thread
            }
            else
            {
                lblMessage.Text = message;
            }
        }
    }
}
