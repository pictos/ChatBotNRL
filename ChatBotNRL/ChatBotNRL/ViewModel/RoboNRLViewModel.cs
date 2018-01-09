using ChatBotNRL.Model;
using ChatBotNRL.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.TextToSpeech;
using System.Linq;
using Plugin.TextToSpeech.Abstractions;
using System.Threading.Tasks;

namespace ChatBotNRL.ViewModel
{
    public class RoboNRLViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ConversationMessage> _messages;

        private string _textMessage;

        private bool _isRefreshing;

        private ICommand _sendMessageCommand;

        static CrossLocale? locale = null;
        

        private ChatBotService _chatBotService = new ChatBotService();      

        public RoboNRLViewModel()
        {
            _messages = new ObservableCollection<ConversationMessage>
            {
                new ConversationMessage
                {
                    Message = "Inicializando",
                    FromUser = "RoboNRL",
                    UserImageUrl = "travelagent.jpg"
                }
            };

            Local();
        }

        async Task Local()
        {
            var locales = await CrossTextToSpeech.Current.GetInstalledLanguages();
            var items = locales.Select(a => a.ToString()).ToArray();

            locale = locales.Last();
        }

        public ObservableCollection<ConversationMessage> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged("Messages");
            }
        }

        public string TextMessage
        {
            get { return _textMessage; }
            set
            {
                _textMessage = value;
                OnPropertyChanged("TextMessage");
            }
        }

        public ICommand SendMessageCommand
        {
            get
            {
                return _sendMessageCommand = _sendMessageCommand ?? new Command(async () =>
                {


                    Messages.Add(new ConversationMessage
                    {
                        FromUser = "AVID CUSTOMER",
                        Message = TextMessage,
                        UserImageUrl = "winston.png"
                    });

                    IsRefreshing = true;

                    var reply = await _chatBotService.SendMessage(TextMessage);                    

                    TextMessage = "";

                    Messages.Clear();

                    foreach (var msgItem in reply.Messages)
                    {
                        Messages.Add(new ConversationMessage
                        {
                            FromUser = msgItem.From == "RoboNRL" ? "TRAVEL AGENT" : "AVID CUSTOMER",
                            Message = msgItem.Text,
                            Id = msgItem.Id,
                            UserImageUrl = msgItem.From == "RoboNRL" ? "travelagent.png" : "winston.png" //travelagentbotcn IS THE NAME OF THE BOT YOU CREATED
                        });
                    }
                    var fala = Messages.Last();
                    await CrossTextToSpeech.Current.Speak(fala.Message.ToString(),crossLocale: locale); 
                    reply = null;

                    IsRefreshing = false;
                });
            }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged("IsRefreshing");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
