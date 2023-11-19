using System;
using System.Collections.Generic;
using System.Linq;
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
                return _node.Name;
            }
        }

        public string Status { get; set; }

        private Image _node;

        private Label _automateLabel;

        private bool _isFixing = false;

        private List<string> _resources;

        public int ResourcesCount { get => _resources.Count; }

        public static List<string> ListStore = new List<string> { "Кола 0.5л", "Кола 1л", "Фанта 1л", "Миринда 1л", "Миринда 0.5л" };

        public Automate(Image node, Label isEmptyLabel, string status)
        {
            _node = node;
            _automateLabel = isEmptyLabel;
            Status = status;
            _resources = new List<string>();
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
        }

        public async void FixAutomate()
        {
            if (_isFixing || Status == AutomateStatuses.Green) return;

            SetImageSource(DefaultValues.YellowMachineSource);
            this.Status = AutomateStatuses.Yellow;

            _isFixing = true;
            await Task.Delay(5000);
            SetImageSource(DefaultValues.GreenMachineSource);
            this.Status = AutomateStatuses.Green;
            _isFixing = false;
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
            Random rnd = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < 20; i++)
            {
                _resources.Add(ListStore.ElementAt(rnd.Next(ListStore.Count)));
            }

            SetLabelFilled();

        }

        public bool SellRandom()
        {
            if (_resources.Count == 0)
            {
                SetLabelEmpty();
            }

            if (_resources.Count == 0 || _isFixing || !(Status == AutomateStatuses.Green))
            {
                return false;
            }

            Random rnd = new Random((int)DateTime.Now.Ticks);

            _resources.Remove(_resources.ElementAt(rnd.Next(_resources.Count)));

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
