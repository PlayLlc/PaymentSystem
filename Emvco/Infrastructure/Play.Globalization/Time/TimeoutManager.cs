using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Globalization.Time
{
    internal class TimeoutManager
    {
        private readonly Stopwatch _Stopwatch;

        public TimeoutManager()
        {
            _Stopwatch = new Stopwatch();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Start(Milliseconds timeout)
        {
            if(_Stopwatch.IsRunning)
                throw new InvalidOperationException($"The {nameof(TimeoutManager)} could not complete the {nameof(Start)} method because the {nameof(TimeoutManager)} is currently running");
        }



    }
}
