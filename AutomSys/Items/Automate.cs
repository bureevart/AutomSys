using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace AutomSys.Items
{
    public class Automate
    {

        public string Name
        {
            get
            {
                return _node.Dispatcher.Invoke(() =>
                {
                    return _node.Name;
                });
            }
        }

        public string Status { get; set; }

        public Image _node;

        private Label _automateLabel;

        private bool _isFixing = false;

        private List<string> _resources;

        private EventsController _eventsController;

        private int _maxItemsInAutomate = 20;

        public int ResourcesCount { get => _resources.Count; }

        public static List<string> ListStore = new List<string> { "Кола 0.5л", "Кола 1л", "Фанта 1л", "Миринда 1л", "Миринда 0.5л" };

        public Automate(Image node, Label isEmptyLabel, string status, EventsController controller)
        {
            _node = node;
            _automateLabel = isEmptyLabel;
            Status = status;
            _resources = new List<string>();
            _eventsController = controller;
        }

        public Automate()
        {
        }

        public async void SetImageSource(ImageSource source)
        {
            await _node.Dispatcher.InvokeAsync(new Action(() => { _node.Source = source; }));
        }

        public void DestroyAutomate()
        {
            if (_isFixing) return;

            SetImageSource(DefaultValues.RedMachineSource);
            this.Status = AutomateStatuses.Red;
            _eventsController.AddEvent(Name, "Автомат сломался!");
        }

        public async void FixAutomate()
        {
            if (_isFixing || Status == AutomateStatuses.Green) return;

            SetImageSource(DefaultValues.YellowMachineSource);
            this.Status = AutomateStatuses.Yellow;

            _isFixing = true;
            _eventsController.AddEvent(Name,"Починка началась!");
            await Task.Delay(5000);
            SetImageSource(DefaultValues.GreenMachineSource);
            this.Status = AutomateStatuses.Green;
            _isFixing = false;
            _eventsController.AddEvent(Name, "Починка завершена!");

        }

        public bool CompareImageWithNode(Image img) => _node == img;

        public static Automate FindImageBySource(DataGrid grid, Image image)
        {
            foreach (var item in grid.ItemsSource)
            {
                if(item is Automate automate && automate.CompareImageWithNode(image))
                {
                    return automate;
                }
            }

            return new Automate();
        }

        public void FillItems()
        {
            if(_resources.Count > _maxItemsInAutomate / 2) 
            {
                _eventsController.AddEvent(Name, "В автомате достаточно товара!");
                return; 
            }

            Random rnd = new Random((int)DateTime.Now.Ticks);
            var itemsToRestore = _maxItemsInAutomate - ResourcesCount;
            for (int i = 0; i < itemsToRestore; i++)
            {
                _resources.Add(ListStore.ElementAt(rnd.Next(ListStore.Count)));
            }

            SetLabelFilled();

            _eventsController.AddEvent(Name, "Восполнили до максимума!");
        }

        public bool SellRandom()
        {
            var isLastItem = false;
            if (_resources.Count == 0)
            {
                SetLabelEmpty();
            }

            if (_resources.Count == 0 || _isFixing || !(Status == AutomateStatuses.Green))
            {
                return false;
            }

            if (_resources.Count == 1)
            {
                isLastItem = true;
            }

            Random rnd = new Random((int)DateTime.Now.Ticks);

            var elToSell = _resources.ElementAt(rnd.Next(_resources.Count));

            _resources.Remove(elToSell);

            _eventsController.AddEvent(Name, $"Продано: {elToSell}");

            if (isLastItem)
            {
                _eventsController.AddEvent(Name, "Нехватка товара!");
            }

            return true;
        }

        private void SetLabelEmpty()
        {
            this._automateLabel.Dispatcher.Invoke(new Action(() =>
            {
                this._automateLabel.Content = "Пусто!";
                this._automateLabel.Foreground = Brushes.Red;
            }));
        }

        private void SetLabelFilled()
        {
            this._automateLabel.Dispatcher.Invoke((Action)(() =>
            {
                this._automateLabel.Content = "В наличии..";
                this._automateLabel.Foreground = Brushes.Green;
            }));
        }

    }
}
