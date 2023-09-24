using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Energy2.AutoGen;
using System.Collections.Generic;
using System.Linq;
using TestTask;

namespace sob
{

    public class Сalculation : SettlementCenter
    {

        static void Main(string[] args)
        {

            // Вывод поставщика базы данных 
            Console.WriteLine($"Испльзуется поставщих данных {ProjectConstants.DatabaseProvider}. \n");

            using (SettlementCenter db = new())
            {
                // Извлекаем все значения TariffPrice и Standart из базы данных
                List<decimal?> tariffPrices = db.Tariffs.Select(t => t.TariffPrice).ToList();
                List<decimal?> standards = db.Tariffs.Select(t => t.Standard).ToList();

                // Пример Id для TariffPrice и Standard
                long tariffId1 = 0;
                long tariffId2 = 1;
                long tariffId3 = 2;
                long tariffId4 = 3;
                long tariffId5 = 4;
                long tariffId6 = 5;

                // Получаем значения TariffPrice по Id
                decimal? tariffPrice1 = GetTariffPriceById(tariffPrices, tariffId1); // Тариф за Холодное водоснабжение
                decimal? tariffPrice2 = GetTariffPriceById(tariffPrices, tariffId2); // Тариф за Электроэнергию
                decimal? tariffPrice3 = GetTariffPriceById(tariffPrices, tariffId3); // Тариф за Электроэнергия день 
                decimal? tariffPrice4 = GetTariffPriceById(tariffPrices, tariffId4); // Тариф за Электроэнергия ночь
                decimal? tariffPrice5 = GetTariffPriceById(tariffPrices, tariffId5); // Тариф за Теплоноситель
                decimal? tariffPrice6 = GetTariffPriceById(tariffPrices, tariffId6); // Тариф за Тепловая энергия

                // Получаем значения Standard по Id
                decimal? standard1 = GetStandardById(standards, tariffId1); // Норматив для Холодное водоснабжение
                decimal? standard2 = GetStandardById(standards, tariffId2); // Норматив для Электроэнергия 
                decimal? standard5 = GetStandardById(standards, tariffId5); // Норматив для Теплоноситель
                decimal? standard6 = GetStandardById(standards, tariffId6); // Норматив для Тепловая энергия

                // Теперь у вас есть значения TariffPrice и Standard для использования в других методах

                // Проверка допустимости значения ID
                static decimal? GetTariffPriceById(List<decimal?> tariffPrices, long tariffId)
                {
                    if (tariffId >= 0 && tariffId < tariffPrices.Count)
                    {
                        return tariffPrices[(int)tariffId];
                    }
                    else
                    {
                        return null;
                    }
                }
                // Проверка допустимости значения ID
                static decimal? GetStandardById(List<decimal?> standards, long tariffId)
                {
                    if (tariffId >= 0 && tariffId < standards.Count)
                    {
                        return standards[(int)tariffId];
                    }
                    else
                    {
                        return null;
                    }
                }

                
                DateTime currentDate = DateTime.Today;

            start: // метка для вывода в меню после выполнения case 3.

                while (true)
                {    // Бесконечный цикл


                    Console.WriteLine("Выберите действие:");
                    Console.WriteLine("1) Рассчитать по показаниям прибора");
                    Console.WriteLine("2) Рассчитать без показаний прибора");
                    Console.WriteLine("3) Показать тарифы");

                    int choice;
                    if (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2 && choice != 3))
                    {
                        Console.WriteLine("Ошибка: Введите 1 или 2 для выбора действия.");
                        continue; // Продолжить цикл, чтобы пользователь мог выбрать правильное действие.
                    
                    }
                    switch (choice)
                    {
                        case 1:
                            CalculateWithMeterReadings(tariffPrice1.GetValueOrDefault(), standard6.GetValueOrDefault(), tariffPrice6.GetValueOrDefault(), tariffPrice3.GetValueOrDefault(), tariffPrice4.GetValueOrDefault());
                            break;
                        case 2:
                            decimal countOfPepole2 = CountOfPeople();
                            CalculateWithoutMeterReadings(standard1.GetValueOrDefault(), tariffPrice1.GetValueOrDefault(), standard5.GetValueOrDefault(), tariffPrice5.GetValueOrDefault(), standard6.GetValueOrDefault(), tariffPrice6.GetValueOrDefault(), standard2.GetValueOrDefault(), tariffPrice2.GetValueOrDefault(), countOfPepole2);
                            break;
                        case 3:
                            ShowService(); //Вывод информации о услугах и тарифах
                            goto start; // Возврат к началу бесконечного цикла, после вывода информации о тарифах 
                        default:
                            Console.WriteLine("Некорректный выбор. Пожалуйста, выберите 1 или 2.");
                            break;
                    }
                    
                    currentDate = DataOfCal(currentDate);

                    Console.WriteLine($"Хотите выполнить расчет за {currentDate}? \n 1)Да / 2)Нет");
                    string answer = Console.ReadLine();

                    if (answer.ToLower() != "1")
                        break;
                }
            }
        }
        // CalculateWithMeterReadings выполняет расчет начислений за каждую коммунальную услугу по показаниям счетчиков
        private static void CalculateWithMeterReadings(decimal tariffPrice1, decimal standard6, decimal tariffPrice6, decimal tariffPrice3, decimal tariffPrice4)
        {


            // Холодное водоснабжение
            Console.WriteLine("\nРасчет за холодное водоснабжение");
            Console.WriteLine("Введите текущие показание");
            decimal mNow = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Введите предидущее показание ");
            decimal mPast = Convert.ToDecimal(Console.ReadLine());
            decimal resultColdWater = ColdWaterWithEquipment(mNow, mPast, tariffPrice1);

            // Горячиее водоснабжение
            Console.WriteLine("\nРасчет за горячее водоснабжение");
            Console.WriteLine("Введите текущие показание");
            decimal mNow1 = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Введите предидущее показание");
            decimal mPast1 = Convert.ToDecimal(Console.ReadLine());
            decimal resultHotWater = HotWaterWithEquipment(mNow1, mPast1, tariffPrice1, standard6, tariffPrice6);

            // Электроэнергия 
            Console.WriteLine("\nРасчет за Электроэнергию");
            Console.WriteLine("Введите дневное показание счетчика");

            // дневное показание счетчика
            decimal daytime = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Введите ночное показание счетчика");

            // ночное показание счетчика
            decimal nighttime = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine(" ");
            decimal resultElectricity = ElectricityWithEquipment(daytime, nighttime, tariffPrice3, tariffPrice4);

            DisplayResult(resultColdWater, resultHotWater, resultElectricity);
        }

        // CalculateWithoutMeterReadings  выполняет расчет начислений за каждую коммунальную услугу без показаний счетчика, использую нормативный объем.
        private static void CalculateWithoutMeterReadings(decimal standard1, decimal tariffPrice1, decimal standard5, decimal tariffPrice5, decimal standard6, decimal tariffPrice6, decimal standard2, decimal tariffPrice2, decimal countOfPepole2)
        {
            decimal resultColdWater = ColdWaterWithoutEquipment(countOfPepole2, standard1, tariffPrice1);
            decimal resultHotWater = HotWaterWithoutEquipment(countOfPepole2, standard5, tariffPrice5, standard6, tariffPrice6);
            decimal resultElectricity = ElectricityWithoutEquipment(countOfPepole2, standard2, tariffPrice2);
            Console.WriteLine(" ");

            DisplayResult(resultColdWater, resultHotWater, resultElectricity);

        }

        // Рассчет ХВС по показаниям счетчика 
        static decimal ColdWaterWithEquipment(decimal mTek, decimal mPred, decimal t)
        {
            decimal v = mTek - mPred;
            return (v * t);
        }

        // Рассчет ХВС без показания счечика 
        static decimal ColdWaterWithoutEquipment(decimal n, decimal standard1, decimal tariffPrice1)
        {
            decimal v = n * standard1;
            return (v * tariffPrice1);
        }

        // Расчет ГВС по показаниям счетчика
        static decimal HotWaterWithEquipment(decimal mTek, decimal mPred, decimal tariffPrice1, decimal standard6, decimal tariffPrice6)
        {
            // Oбъем потреблнеия услуги "ГВС Теплоноситель"
            decimal vTN = mTek - mPred;
            // ГВС теплоностиель 
            decimal pTN = vTN * tariffPrice1;
            // Oбъем потреблнеия услуги "ГВС тепловая энергия"
            decimal vTE = vTN * standard6;
            //начислени за "ГВС тепловая энергия"
            decimal pTE = vTE * tariffPrice6;
            // Итоговое начисление за "ГВС тепловая энергия" и "ГВС тепловая энергия"
            return (pTE + pTN);
        }

        // Расчет ГВС без показаний счетчика 
        static decimal HotWaterWithoutEquipment(decimal n, decimal standard5, decimal tariffPrice5, decimal standard6, decimal tariffPrice6)
        {
            // Oбъем потреблнеия услуги "ГВС Теплоноситель"
            decimal vTN = n * standard5;
            // ГВС теплоностиель
            decimal pTN = vTN * tariffPrice5;
            //Oбъем потреблнеия услуги "ГВС тепловая энергия"
            decimal vTE = standard6 * n;
            //начислени за "ГВС тепловая энергия"
            decimal pTE = vTE * tariffPrice6;
            // Итоговое начисление за "ГВС тепловая энергия" и "ГВС тепловая энергия"
            return (pTE + pTN);
        }
        // Расчет электроэнергии по показаниям счетчика
        static decimal ElectricityWithEquipment(decimal vD, decimal vN, decimal tariffPrice3, decimal tariffPrice4)
        {
            // Расчет стоимости по дневному тарифу
            decimal pD = vD * tariffPrice3;
            // Расчет стоимости по ночному тарифу 
            decimal pN = vN * tariffPrice4;
            return (pD + pN);
        }
        // Расчет электроэнергии без показаний счетчика
        static decimal ElectricityWithoutEquipment(decimal n, decimal standard2, decimal tariffPrice2)
        {
            decimal v = n * standard2;
            return (v * tariffPrice2);
        }
        // метод для ввода и проверки данных о количесве прживающих жильцов 
        static decimal CountOfPeople()
        {
            while (true)
            {
                Console.WriteLine("\nВведите количество проживающих в помещении");

                try
                {
                    string input = Console.ReadLine();
                    decimal countOfPeople = decimal.Parse(input);

                    if (countOfPeople > 0)
                    {
                        return countOfPeople;
                    }
                    else
                    {
                        Console.WriteLine("Введите положительное число.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Некорректный формат числа. Пожалуйста, введите число.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Введенное число слишком большое.");
                }
            }
        }
        // Вывод рассчетов в консоль
        static void DisplayResult(decimal resultColdWater, decimal resultHotWater, decimal resultElectricity)
        {
            Console.WriteLine(DateTime.Today);
            Console.WriteLine(new string('-', 43)); // Верхняя горизонтальная линия таблицы
            Console.WriteLine("| Услуга                 | Сумма (рублей) |");
            Console.WriteLine(new string('-', 43)); // Горизонтальная линия после заголовка
            Console.WriteLine("| Холодное водоснабжение | {0,-15:F2}|", resultColdWater);
            Console.WriteLine("| Горячее водоснабжение  | {0,-15:F2}|", resultHotWater);
            Console.WriteLine("| Электроэнергия         | {0,-15:F2}|", resultElectricity);
            Console.WriteLine(new string('-', 43)); // Горизонтальная линия после данных
            Console.WriteLine("| Итог                   | {0,-15:F2}|", resultColdWater + resultHotWater + resultElectricity);
            Console.WriteLine(new string('-', 43)); // Нижняя горизонтальная линия таблицы
        }
        static DateTime DataOfCal(DateTime dateTime)
        {
            // Добавляем один месяц к текущей дате
            dateTime = dateTime.AddMonths(1);

            return dateTime;
        }
        // метод вывода информации о тарифах в консоль
        static void ShowService()
        {
            using (SettlementCenter db = new())
            {
                Console.WriteLine("Перечень предоставленных услуг.");
                var services = db.Tariffs.Select(t => new { t.ServiceName, t.TariffPrice, t.Unit });

                if (!services.Any())
                {
                    Console.WriteLine("Услуги не были найдены");
                    return;
                }

                foreach (var serviceName in services)
                {
                    Console.WriteLine($"Услуга: {serviceName.ServiceName}. Тариф услуги: {serviceName.TariffPrice} рублей за {serviceName.Unit} ");
                }
            }
        }
        /////////////////////////////////////////////
    }

}

