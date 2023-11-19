using AutomSys.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutomSys
{
    public class SimulateController
    {
        public static bool onSimulate = false;
        private static int _minTimeBeforeNextEvent = 10;
        private static int _maxTimeBeforeNextEvent = 30;

        public async static void Simulate(DataGrid grid)
        {
            if (onSimulate) return;

            Random rnd = new Random((int)DateTime.Today.Ticks);
            onSimulate = true;

            var items = (grid.ItemsSource as IEnumerable<Automate>).ToArray();

            if (items == null) return;

            var count = items.Count();
            
            Task.Run(async () =>
            {
                while (onSimulate)
                {
                    await Task.Delay(rnd.Next(_minTimeBeforeNextEvent, _maxTimeBeforeNextEvent) * 1000);

                    var automateIndx = rnd.Next(0, count);

                    items[automateIndx].DestroyAutomate();
                }
            });

            Task.Run(async () =>
            {
                while (onSimulate)
                {
                    await Task.Delay(1000);

                    var automateIndx = rnd.Next(0, count);

                    var isSold = items[automateIndx].SellRandom();

                    if (!isSold)
                    {
                    }
                }
            });
        }
    }
}
