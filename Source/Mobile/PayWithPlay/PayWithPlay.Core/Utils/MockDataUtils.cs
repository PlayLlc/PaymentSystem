﻿using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.Chart;
using PayWithPlay.Core.Models.Home;
using PayWithPlay.Core.Models.Loyalty;
using System;
using System.Data;
using System.Drawing.Text;
using System.Security.Cryptography;

namespace PayWithPlay.Core.Utils
{
    public static class MockDataUtils
    {
        private static string[] _productsName = new string[] { "Crocks", "Ben Sherman", "Lacoste Boat", "Free people", "Seven For all", "Prodcut name", "Prodcut name", "Prodcut name", "Prodcut name", "Prodcut name", "Prodcut name", "Prodcut name", "Prodcut name" };

        private static string[] _shortMonthsValues = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        private static string[] _weekDaysValues = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

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

        #region Inventory charts

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

        #endregion

        #region Loyalty charts

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

        public static List<ChartEntry> RandomLoyaltyTotalSalesChartData(ChartStepType chartStep)
        {
            var list = new List<ChartEntry>();
            var random = new Random();

            var min = 5000;
            var max = 100000;

            if (chartStep == ChartStepType.Day)
            {
                for (int i = 0; i < 24; i++)
                {
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
                    list.Add(new ChartEntry(i, RandomBool() ? random.Next(min, max) : 0) { Title = _weekDaysValues[i] });
                }
            }
            else if (chartStep == ChartStepType.Month)
            {
                for (int i = 0; i < _shortMonthsValues.Length; i++)
                {
                    list.Add(new ChartEntry(i, RandomBool() ? random.Next(min, max) : 0) { Title = _shortMonthsValues[i] });
                }
            }

            return list;
        }

        public static List<EnrollerModel> RandomEnrollersChartData(ChartStepType chartStep)
        {
            var list = new List<EnrollerModel>()
            {
                new EnrollerModel()
                {
                    EnrollerName = "Mark Molina",
                    StoreName = "Store name 7",
                    Active = RandomBool()
                },
                new EnrollerModel()
                {
                    EnrollerName = "Susan Sparks",
                    StoreName = "Store name 2",
                    Active = RandomBool()
                },
                new EnrollerModel()
                {
                    EnrollerName = "Oliver Rodriguez",
                    StoreName = "Store name 5",
                    Active = RandomBool()
                },
                new EnrollerModel()
                {
                    EnrollerName = "Peter Smith",
                    StoreName = "Store name 1",
                    Active = RandomBool()
                },
                new EnrollerModel()
                {
                    EnrollerName = "Tim Krueger",
                    StoreName = "Store name 8",
                    Active = RandomBool()
                },
                new EnrollerModel()
                {
                    EnrollerName = "Dottie Donaldson",
                    StoreName = "Store name 3",
                    Active = RandomBool()
                },
                new EnrollerModel()
                {
                    EnrollerName = "Maria Carney",
                    StoreName = "Store name 4",
                    Active = RandomBool()
                },
                new EnrollerModel()
                {
                    EnrollerName = "Lorena Trevino",
                    StoreName = "Store name 6",
                    Active = RandomBool()
                },
            };

            list.Shuffle();

            return list;
        }

        #endregion

        #region Home charts

        public static List<ChartEntry> RandomHomeTotalSalesChartData(ChartStepType chartStep)
        {
            var list = new List<ChartEntry>();
            var random = new Random();

            var min = 5000;
            var max = 100000;

            if (chartStep == ChartStepType.Day)
            {
                for (int i = 0; i < 24; i++)
                {
                    var title = string.Empty;
                    if (i == 0)
                    {
                        title = "0am";
                    }
                    else if (i == 5)
                    {
                        title = "6am";
                    }
                    else if (i == 11)
                    {
                        title = "12am";
                    }
                    else if (i == 17)
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
                    list.Add(new ChartEntry(i, RandomBool() ? random.Next(min, max) : 0) { Title = _weekDaysValues[i] });
                }
            }
            else if (chartStep == ChartStepType.Month)
            {
                for (int i = 0; i < _shortMonthsValues.Length; i++)
                {
                    list.Add(new ChartEntry(i, RandomBool() ? random.Next(min, max) : 0) { Title = _shortMonthsValues[i] });
                }
            }

            return list;
        }

        public static List<ChartEntry> RandomAvgTransactionValueChartData(ChartStepType chartStep)
        {
            var list = new List<ChartEntry>();
            var random = new Random();

            var min = 50;
            var max = 1000;

            if (chartStep == ChartStepType.Day)
            {
                for (int i = 0; i < 24; i++)
                {
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
                    list.Add(new ChartEntry(i, RandomBool() ? random.Next(min, max) : 0) { Title = _weekDaysValues[i] });
                }
            }
            else if (chartStep == ChartStepType.Month)
            {
                for (int i = 0; i < _shortMonthsValues.Length; i++)
                {
                    list.Add(new ChartEntry(i, RandomBool() ? random.Next(min, max) : 0) { Title = _shortMonthsValues[i] });
                }
            }

            return list;
        }

        public static List<SellerModel> RandomTopSellersChartData(ChartStepType chartStep)
        {
            var list = new List<SellerModel>()
            {
                new SellerModel()
                {
                    SellerName = "Mark Molina",
                    StoreName = "Store name 7",
                    Active = RandomBool()
                },
                new SellerModel()
                {
                    SellerName = "Susan Sparks",
                    StoreName = "Store name 2",
                    Active = RandomBool()
                },
                new SellerModel()
                {
                    SellerName = "Oliver Rodriguez",
                    StoreName = "Store name 5",
                    Active = RandomBool()
                },
                new SellerModel()
                {
                    SellerName = "Peter Smith",
                    StoreName = "Store name 1",
                    Active = RandomBool()
                },
                new SellerModel()
                {
                    SellerName = "Tim Krueger",
                    StoreName = "Store name 8",
                    Active = RandomBool()
                },
                new SellerModel()
                {
                    SellerName = "Dottie Donaldson",
                    StoreName = "Store name 3",
                    Active = RandomBool()
                },
                new SellerModel()
                {
                    SellerName = "Maria Carney",
                    StoreName = "Store name 4",
                    Active = RandomBool()
                },
                new SellerModel()
                {
                    SellerName = "Lorena Trevino",
                    StoreName = "Store name 6",
                    Active = RandomBool()
                },
            };

            list.Shuffle();

            var amounts = new decimal[] { 489m, 402m, 320m, 289m, 230m, 200m, 195m, 150m };
            var prevAmounts = new decimal[] { -25m, 95m, -13m, -10m, 76m, 11m, -26m, -35m };

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                item.Amount = amounts[i];
                item.PrevAmount = prevAmounts[i];
            }

            return list;
        }

        public static List<ChartEntry> RandomTransactionsChartData()
        {
            var list = new List<ChartEntry>();

            var full = RandomBool();

            var random = new Random();

            var count = full ? 60 : random.Next(12, 60);

            for (int i = 0; i < 60; i++)
            {
                var title = string.Empty;
                if (i == 0)
                {
                    title = "8am";
                }
                else if (i == 5)
                {
                    title = "9am";
                }
                else if (i == 11)
                {
                    title = "10am";
                }
                else if (i == 17)
                {
                    title = "11am";
                }
                else if (i == 23)
                {
                    title = "12am";
                }
                else if (i == 29)
                {
                    title = "1pm";
                }
                else if (i == 35)
                {
                    title = "2pm";
                }
                else if (i == 41)
                {
                    title = "3pm";
                }
                else if (i == 47)
                {
                    title = "4pm";
                }
                else if (i == 53)
                {
                    title = "5pm";
                }
                else if (i == 59)
                {
                    title = "6pm";
                }
                list.Add(new ChartEntry(i, i <= count ? random.Next(0, 60) : 0) { Title = title });
            }

            return list;
        }

        #endregion
    }
}
