using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutomSys.Items
{
    public class Event
    {
        public Event(object sender) : this()
        {
            switch (sender)
            {
                case Button button:
                    Source = "Button: " + button.Content;
                    break;
                case string str:
                    Source = str;
                    break;
                default:
                    Source = "Неизвестный источник";
                    break;
            }
        }

        public Event()
        {
            currId++;
            Id = currId;
            Date = DateTime.Now.ToString();
            Source = "Неизвестный источник";
        }

        public int Id { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string Date { get; set; }

        public static int currId { get; set; } = 0;
    }
}
