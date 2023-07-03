using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.Chart;
using System.Data;

namespace PayWithPlay.Core.Utils
{
    public static class MockDataUtils
    {
        private static string[] _productsName = new string[] { "Crocks", "Ben Sherman", "Lacoste Boat", "Free people", "Seven For all", "Prodcut name", "Prodcut name", "Prodcut name", "Prodcut name", "Prodcut name", "Prodcut name", "Prodcut name", "Prodcut name" };

        private static string[] _shortMonthsValues = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        private static string[] _weekDaysValues = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

        public static decimal RandomDecimal(bool onlyPositives = true)
        {
            var randomNumberGenerator = new Random();
            int precision = randomNumberGenerator.Next(2, 5);
            int scale = randomNumberGenerator.Next(2, precision);

            if (randomNumberGenerator == null)
                throw new ArgumentNullException("randomNumberGenerator");
            if (!(precision >= 1 && precision <= 28))
                throw new ArgumentOutOfRangeException("precision", precision, "Precision must be between 1 and 28.");
            if (!(scale >= 0 && scale <= precision))
                throw new ArgumentOutOfRangeException("scale", precision, "Scale must be between 0 and precision.");

            decimal d = 0m;
            for (int i = 0; i < precision; i++)
            {
                int r = randomNumberGenerator.Next(0, 10);
                d = d * 100m + r;
            }
            for (int s = 0; s < scale; s++)
            {
                d /= 10m;
            }
            if (randomNumberGenerator.Next(2) == 1)
                d = decimal.Negate(d);

            if (onlyPositives)
            {
                return Math.Abs(d);
            }

            return d;
        }

        public static bool RandomBool()
        {
            var random = new Random();
            return random.Next() > (int.MaxValue / 2);
        }

        public static string RandomString()
        {
            var random = new Random();
            return RandomString(random.Next(1, 20));
        }

        public static string RandomString(int length)
        {
            var random = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

        public static MiniChartModel RandomDataMiniChart(MiniChartModel? miniChartModel)
        {
            miniChartModel ??= new MiniChartModel();

            var random = new Random();
            miniChartModel.Value = (float)random.NextDouble();
            miniChartModel.IsPositive = RandomBool();

            var entries = new List<ChartEntry>();
            for (int i = 0; i < 12; i++)
            {
                entries.Add(new ChartEntry(i, random.Next(1, 10)));
            }

            if (miniChartModel.IsPositive && entries.Last().Y <= entries.First().Y)
            {
                entries.Last().Y = entries.First().Y + 1;
            }
            else if (!miniChartModel.IsPositive && entries.Last().Y >= entries.First().Y)
            {
                entries.Last().Y = entries.First().Y - 1;
            }

            miniChartModel.Entries = entries;

            return miniChartModel;
        }

        public static List<ChartEntry> RandomTopSellingChartData(ChartStepType chartStep)
        {
            var list = new List<ChartEntry>();
            var random = new Random();

            if (random.Next() < (int.MaxValue / 8))
            {
                return list;
            }

            var min = random.Next(5, 20);
            var max = random.Next(60, 100);

            for (int i = 0; i < 13; i++)
            {
                list.Add(new ChartEntry(i, random.Next(min, max)) { Title = _productsName[i] });
            }

            return list;
        }

        public static List<ChartEntry> RandomShrinkageRateChartData() 
        {
            var list = new List<ChartEntry>();
            var random = new Random();

            if (random.Next() < (int.MaxValue / 8)) 
            {
                return list;
            }

            for (int i = 0; i < 10; i++)
            {
                list.Add(new ChartEntry(i, (float)random.NextDouble()) { Title = _productsName[i] });
            }

            return list;
        }

        public static List<ChartEntry> RandomInventoryOnHandChartData()
        {
            var list = new List<ChartEntry>();
            var random = new Random();

            if (random.Next() < (int.MaxValue / 8))
            {
                return list;
            }

            for (int i = 0; i < _shortMonthsValues.Length; i++)
            {
                list.Add(new ChartEntry(i, random.Next(5, 80)) { Title = _shortMonthsValues[i] });
            }

            return list;
        }

        public static List<ChartEntry> RandomSalesVsShrinkageChartData(int minValue, int maxValue) 
        {
            var list = new List<ChartEntry>();
            var random = new Random();

            for (int i = 0; i < _shortMonthsValues.Length; i++)
            {
                list.Add(new ChartEntry(i, random.Next(minValue, maxValue)) { Title = _shortMonthsValues[i] });
            }

            return list;
        }

        public static List<ChartEntry> RandomLoyaltySalesVsRedeemedChartData(int minValue, int maxValue)
        {
            var list = new List<ChartEntry>();
            var random = new Random();

            for (int i = 0; i < _shortMonthsValues.Length; i++)
            {
                list.Add(new ChartEntry(i, random.Next(minValue, maxValue)) { Title = _shortMonthsValues[i] });
            }

            return list;
        }

        public static List<ChartEntry> RandomNewLoyaltyAccountsChartData(ChartStepType chartStep) 
        {
            var list = new List<ChartEntry>();
            var random = new Random();

            if (random.Next() < (int.MaxValue / 8))
            {
                return list;
            }


            if (chartStep == ChartStepType.Day)
            {
                for (int i = 0; i < 24; i++)
                {
                    var min = 1;
                    var max = 5;
                    var title = string.Empty;
                    if (i == 0)
                    {
                        title = "0am";
                    }
                    else if (i == 6)
                    {
                        title = "6am";
                    }
                    else if (i == 12)
                    {
                        title = "12am";
                    }
                    else if (i == 18) 
                    {
                        title = "6pm";
                    }
                    list.Add(new ChartEntry(i, RandomBool() ? random.Next(min, max) : 0) { Title = title });
                }
            }
            else if (chartStep == ChartStepType.Week)
            {
                for (int i = 0; i < _weekDaysValues.Length; i++)
                {
                    var min = 1;
                    var max = 20;
                    list.Add(new ChartEntry(i, RandomBool() ? random.Next(min, max) : 0) { Title = _weekDaysValues[i] });
                }
            }
            else if (chartStep == ChartStepType.Month) 
            {
                for (int i = 0; i < _shortMonthsValues.Length; i++)
                {
                    var min = 1;
                    var max = 100;
                    list.Add(new ChartEntry(i, RandomBool() ? random.Next(min, max) : 0) { Title = _shortMonthsValues[i] });
                }
            }

            return list;
        }
    }
}
