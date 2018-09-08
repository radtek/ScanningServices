using Newtonsoft.Json;
using ScanningServicesDataObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanningServicesAdmin
{
    class GeneralTools
    {
        /// <summary>
        /// Builds a CROn Job string from given Json parameter
        /// </summary>
        /// <param name="scheduleJS"></param>
        /// <returns></returns>
        public static string scheduleStringBuilder(string scheduleJS)
        {

            string scheduleCronFormat = "";
            GlobalVars.ScheduleTime schedule = new GlobalVars.ScheduleTime();
            if (!string.IsNullOrEmpty(scheduleJS))
            {
                schedule = JsonConvert.DeserializeObject<GlobalVars.ScheduleTime>(scheduleJS);
                string seconds = "0";
                string minutes = "0";
                string hours = "0";
                string dayOfTheMonth = "?";
                string month = "*"; // evry month
                string daysOfTheWeek = "0";
                string years = "*";

                //-------------------------------------------------------------
                //Cron Job Validation in Quartz Reference : https://www.freeformatter.com/cron-expression-generator-quartz.html
                //-------------------------------------------------------------
                // Setting Days parameter in Cron Job String
                if (schedule.dailyFlag)
                {
                    dayOfTheMonth = "?";
                    if (schedule.recurEveryDays.Length != 0)
                    {
                        daysOfTheWeek = "1/" + schedule.recurEveryDays.Trim();
                    }
                    else
                    {
                        daysOfTheWeek = "*";
                    }
                }

                //-------------------------------------------------------------
                // Setting Minutes parameter in Cron Job string
                if (schedule.startTaskAtFlag)
                {
                    minutes = schedule.startTaskMinute;
                    hours = schedule.startTaskHour;
                }
                //-------------------------------------------------------------
                if (schedule.repeatTaskFlag)
                {
                    if (schedule.repeatEveryHoursFlag)
                    {
                        hours = "";
                        int min;
                        int max;

                        if (schedule.repeatTaskRange)
                        {
                            min = Convert.ToInt32(schedule.taskBeginHour);
                            max = Convert.ToInt32(schedule.taskEndHour);
                        }
                        else
                        {
                            min = 0;
                            max = 23;
                        }
                        int n = min;

                        if (schedule.repeatTaskTimes.Length == 0)
                        {
                            //hours = schedule.taskBeginHour.ToString() + "-" + schedule.taskEndHour.ToString();
                            hours = "*";
                        }
                        else
                        {
                            int increment = Convert.ToInt32(schedule.repeatTaskTimes);
                            while (n <= max)
                            {
                                if ((n + increment) >= max)
                                {
                                    if (n == max)
                                    {
                                        if (hours.Length > 0) hours = hours + "," + n.ToString();
                                    }
                                    else
                                    {
                                        hours = hours + n.ToString();
                                    }
                                }
                                else
                                    hours = hours + n.ToString() + ",";
                                n = n + increment;
                            }
                        }
                    }
                    if (schedule.repeatEveryMinutesFlag)
                    {
                        if (schedule.repeatTaskTimes.Length == 0)
                            minutes = "0";
                        else
                            minutes = "0/" + schedule.repeatTaskTimes;
                        if (schedule.repeatTaskRange)
                        {
                            hours = schedule.taskBeginHour.ToString() + "-" + schedule.taskEndHour.ToString();
                        }
                        else
                        {
                            hours = "*";
                        }
                    }
                }
                //-------------------------------------------------------------
                if (schedule.dayOfTheWeekFlag)
                {
                    dayOfTheMonth = "?";
                    //SUN,MON,TUE,WED,THU,FRI,SAT
                    daysOfTheWeek = "";
                    if (schedule.sunday)
                        daysOfTheWeek = "SUN";
                    if (schedule.monday)
                        if (daysOfTheWeek.Length == 0)
                            daysOfTheWeek = "MON";
                        else
                            daysOfTheWeek = daysOfTheWeek + "," + "MON";
                    if (schedule.tuesday)
                        if (daysOfTheWeek.Length == 0)
                            daysOfTheWeek = "TUE";
                        else
                            daysOfTheWeek = daysOfTheWeek + "," + "TUE";
                    if (schedule.wednesday)
                        if (daysOfTheWeek.Length == 0)
                            daysOfTheWeek = "WED";
                        else
                            daysOfTheWeek = daysOfTheWeek + "," + "WED";
                    if (schedule.thursday)
                        if (daysOfTheWeek.Length == 0)
                            daysOfTheWeek = "THU";
                        else
                            daysOfTheWeek = daysOfTheWeek + "," + "THU";
                    if (schedule.friday)
                        if (daysOfTheWeek.Length == 0)
                            daysOfTheWeek = "FRI";
                        else
                            daysOfTheWeek = daysOfTheWeek + "," + "FRI";
                    if (schedule.saturday)
                        if (daysOfTheWeek.Length == 0)
                            daysOfTheWeek = "SAT";
                        else
                            daysOfTheWeek = daysOfTheWeek + "," + "SAT";
                }
                //-------------------------------------------------------------
                // Building Final Schedule Cron Job content
                scheduleCronFormat = seconds + " " + minutes + " " + hours + " " + dayOfTheMonth + " " + month + " " + daysOfTheWeek + " " + years;
            }

            return scheduleCronFormat;
        }
    }
}
