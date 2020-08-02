using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Codesanook.Examples.WinFormApp
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();

        private async void PingButtonClick(object sender, EventArgs e)
        {
            using var ping = new Ping();
            const int timeoutInMilliseconds = 1000;
            var reply = await ping.SendPingAsync(addressTextBox.Text, timeoutInMilliseconds);

            var result = new Dictionary<string, string>
            {
                { nameof(reply.Status), reply.Status.ToString()},
                { "Time", reply.RoundtripTime.ToString()},
                { nameof(reply.Address), reply.Address.ToString()}
            };

            var outputMessage = (
                from r in result 
                select $"{r.Key}: {r.Value}"
            );

            MessageBox.Show(string.Join(Environment.NewLine, outputMessage));
        }
    }
}
