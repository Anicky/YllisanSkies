namespace RaverSoft.YllisanSkies.Utils
{
    public class DateTimeUtils
    {
        public static string formatTimeFromSeconds(int timeInSeconds)
        {
            int hours = timeInSeconds / (60 * 60);
            int minutes = timeInSeconds / 60;
            int seconds = timeInSeconds % 60;
            string hoursToString = hours.ToString();
            if (hours < 10)
            {
                hoursToString = "0" + hoursToString;
            }
            string minutesToString = minutes.ToString();
            if (minutes < 10)
            {
                minutesToString = "0" + minutesToString;
            }
            string secondsToString = seconds.ToString();
            if (seconds < 10)
            {
                secondsToString = "0" + secondsToString;
            }
            return hoursToString + ":" + minutesToString + ":" + secondsToString;
        }
    }
}