using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    class ShareNotesViewModel : BaseViewModel
    {
        private string message;
        private List<string> recipientlist;
        private string recipient;
        public Command AddRecipient;
        public Command SendNotesCommand { get; }

        public string Recipient
        {
            get => recipient; set => SetProperty(ref recipient, value);
        }

        public List<string> RecipientList
        {
            get => recipientlist; set => SetProperty(ref recipientlist, value);
        }

        public string Message
        {
            get => message; set => SetProperty(ref message, value);
        }

        public ShareNotesViewModel()
        {
            AddRecipient = new Command(AddNewRecipient);
            SendNotesCommand = new Command(async () => await SendSms());
        }

        private void AddNewRecipient()
        {
            RecipientList.Add(Recipient);
        }

        private async Task SendSms()
        {
            try
            {
                var message = new SmsMessage(Message, Recipient);
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
    }
}
