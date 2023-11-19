using AutomSys.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace AutomSys
{
    public class SimulateController
    {
        public static bool onSimulate = false;
        private static int _minTimeBeforeNextEvent = 10;
        private static int _maxTimeBeforeNextEvent = 30;

        public async static void Simulate(List<Automate> automates, EventsController eventsController)
        {
            if (onSimulate) return;

            Random rnd = new Random((int)DateTime.Today.Ticks);
            onSimulate = true;

            var count = automates.Count();
            
            Task.Run(async () =>
            {
                while (onSimulate)
                {
                    await Task.Delay(rnd.Next(_minTimeBeforeNextEvent, _maxTimeBeforeNextEvent) * 1000);

                    var automateIndx = rnd.Next(0, count);
                    if(onSimulate)
                        automates[automateIndx].DestroyAutomate();
                }
            });

            Task.Run(async () =>
            {
                while (onSimulate)
                {
                    await Task.Delay(1000);

                    var automateIndx = rnd.Next(0, count);
                    if (onSimulate)
                        automates[automateIndx].SellRandom();
                }
            });
        }
        public static void StopSimulation()
        {
            if (!onSimulate) return;
            onSimulate = false;
        }
    }
}
