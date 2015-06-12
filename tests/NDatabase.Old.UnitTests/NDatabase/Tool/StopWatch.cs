using System;
using System.Text;

namespace Test.NDatabase.Tool
{
    /// <summary>
    ///   A simple timer to get task duration. you make start. End when ends your task.
    ///   Then you can get te duration by using getDurationinMseconds
    ///   getDurationInSeconds
    /// </summary>
    /// <author>olivier smadja</author>
    /// <version>03/09/2001 - creation</version>
    public class StopWatch
    {
        /// <summary>
        ///   The end date time in ms
        /// </summary>
        private long end_Renamed_Field;

        /// <summary>
        ///   The start date time in ms
        /// </summary>
        private long start_Renamed_Field;

        /// <summary>
        ///   Constructor
        /// </summary>
        public StopWatch()
        {
            start_Renamed_Field = 0;
            end_Renamed_Field = 0;
        }

        /// <summary>
        ///   gets the duration in mili seconds
        /// </summary>
        /// <returns> long The duration in ms </returns>
        public virtual long DurationInMiliseconds
        {
            get { return end_Renamed_Field - start_Renamed_Field; }
        }

        /// <summary>
        ///   gets the duration in seconds
        /// </summary>
        /// <returns> long The duration in seconds </returns>
        public virtual long DurationInSeconds
        {
            get { return (end_Renamed_Field - start_Renamed_Field) / 1000; }
        }

        public long GetDurationInMiliseconds()
        {
            return (end_Renamed_Field - start_Renamed_Field) / 1000;
        }

        /// <summary>
        ///   Mark the start time
        /// </summary>
        public virtual void Start()
        {
            //UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
            start_Renamed_Field = DateTime.Now.Ticks;
        }

        /// <summary>
        ///   Mark the end time
        /// </summary>
        public virtual void End()
        {
            //UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
            end_Renamed_Field = DateTime.Now.Ticks;
        }

        /// <summary>
        ///   string description of the object
        /// </summary>
        /// <returns> String </returns>
        public override String ToString()
        {
            var sResult = new StringBuilder();
            sResult.Append("Start = ").Append(start_Renamed_Field).Append(" / End = ").Append(end_Renamed_Field).Append(
                " / Duration(ms) = ").Append(DurationInMiliseconds);
            return sResult.ToString();
        }
    }
}
