using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Идентификатор")]
        public int Id { get; set; }
        [DisplayName("Источник")]
        public string Source { get; set; }
        [DisplayName("Событие")]
        public string Message { get; set; }
        [DisplayName("Дата")]
        public string Date { get; set; }

        public static int currId { get; set; } = 0;
    }
}
