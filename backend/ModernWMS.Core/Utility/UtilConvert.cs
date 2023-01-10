using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ModernWMS.Core.Utility
{
    /// <summary>
    /// convert utility
    /// </summary>
    public static class UtilConvert
    {
        #region object convert function
        /// <summary>
        /// object convert to int
        /// </summary>
        /// <param name="thisValue">value</param>
        /// <returns></returns>
        public static int ObjToInt(this object thisValue)
        {
            int reval = 0;
            if (thisValue == null)
            {
                return 0;
            }
            if (string.IsNullOrEmpty(thisValue.ToString()))
            {
                return 0;
            }
            if (thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return reval;
        }
        /// <summary>
        /// object convert to int
        /// </summary>
        /// <param name="thisValue">value</param>
        /// <param name="errorValue">value when error occured</param>
        /// <returns></returns>
        public static int ObjToInt(this object thisValue, int errorValue)
        {

            if (thisValue == null)
            {
                return errorValue;
            }
            if (string.IsNullOrEmpty(thisValue.ToString()))
            {
                return 0;
            }
            if (thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out int reval))
            {
                return reval;
            }
            return errorValue;
        }
        /// <summary>
        /// object convert to double
        /// </summary>
        /// <param name="thisValue">value</param>
        /// <returns></returns>
        public static double ObjToDouble(this object thisValue)
        {
            if (string.IsNullOrEmpty(thisValue.ToString()))
            {
                return 0;
            }
            if (thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out double reval))
            {
                return reval;
            }
            return 0;
        }
        /// <summary>
        /// object convert to double
        /// </summary>
        /// <param name="thisValue">value</param>
        /// <param name="errorValue">value when error occured</param>
        /// <returns></returns>
        public static double ObjToDouble(this object thisValue, double errorValue)
        {
            if (string.IsNullOrEmpty(thisValue.ToString()))
            {
                return 0;
            }
            if (thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out double reval))
            {
                return reval;
            }
            return errorValue;
        }
        /// <summary>
        /// object convert to string
        /// </summary>
        /// <param name="thisValue">value</param>
        /// <returns></returns>
        public static string ObjToString(this object thisValue)
        {
            if (thisValue != null) return thisValue.ToString().Trim();
            return "";
        }
        /// <summary>
        /// object convert to string
        /// </summary>
        /// <param name="thisValue">value</param>
        /// <param name="errorValue">value when error occured</param>
        /// <returns></returns>
        public static string ObjToString(this object thisValue, string errorValue)
        {
            if (thisValue != null) return thisValue.ToString().Trim();
            return errorValue;
        }
        /// <summary>
        /// object convert to decimal
        /// </summary>
        /// <param name="thisValue">value</param>
        /// <returns></returns>
        public static decimal ObjToDecimal(this object thisValue)
        {
            if (string.IsNullOrEmpty(thisValue.ToString()))
            {
                return 0;
            }
            if (thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out decimal reval))
            {
                return reval;
            }
            return 0;
        }
        /// <summary>
        /// object convert to decimal
        /// </summary>
        /// <param name="thisValue">value</param>
        /// <param name="errorValue">value when error occured</param>
        /// <returns></returns>
        public static decimal ObjToDecimal(this object thisValue, decimal errorValue)
        {
            if (string.IsNullOrEmpty(thisValue.ToString()))
            {
                return 0;
            }
            if (thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out decimal reval))
            {
                return reval;
            }
            return errorValue;
        }
        /// <summary>
        ///  object convert to date
        /// </summary>
        /// <param name="thisValue">value</param>
        /// <returns></returns>
        public static DateTime ObjToDate(this object thisValue)
        {
            DateTime reval = Convert.ToDateTime("1900-01-01");
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out reval))
            {
                reval = Convert.ToDateTime(thisValue);
            }
            return reval;
        }
        /// <summary>
        /// object convert to date
        /// </summary>
        /// <param name="thisValue">value</param>
        /// <param name="errorValue">value when error occured</param>
        /// <returns></returns>
        public static DateTime ObjToDate(this object thisValue, DateTime errorValue)
        {
            DateTime reval;
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return errorValue;
        }

        /// <summary>
        /// object convert to bool
        /// </summary>
        /// <param name="thisValue">value</param>
        /// <returns></returns>
        public static bool ObjToBool(this object thisValue)
        {
            bool reval = false;
            if (thisValue != null && thisValue != DBNull.Value && bool.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return reval;
        }
        #endregion

        #region compare function
        /// <summary>
        /// less than 
        /// </summary>
        /// <param name="thisValue">thisValue</param>
        /// <param name="compareValue">compareValue</param>
        /// <returns></returns>
        public static bool IsLessThan(this object thisValue,double compareValue)
        {
            return Convert.ToDouble(thisValue) < compareValue;
        }
        /// <summary>
        /// less than or equal
        /// </summary>
        /// <param name="thisValue">thisValue</param>
        /// <param name="compareValue">compareValue</param>
        /// <returns></returns>
        public static bool IsLessThanOrEqual(this object thisValue, double compareValue)
        {
            return Convert.ToDouble(thisValue) <= compareValue;
        }
        /// <summary>
        /// greater than
        /// </summary>
        /// <param name="thisValue">当前值</param>
        /// <param name="compareValue">比较值</param>
        /// <returns></returns>
        public static bool IsGreaterThan(this object thisValue, double compareValue)
        {
            return Convert.ToDouble(thisValue) > compareValue;
        }
        /// <summary>
        /// greater than or equal
        /// </summary>
        /// <param name="thisValue">thisValue</param>
        /// <param name="compareValue">compareValue</param>
        /// <returns></returns>
        public static bool IsGreaterThanOrEqual(this object thisValue, double compareValue)
        {
            return Convert.ToDouble(thisValue) >= compareValue;
        }
        /// <summary>
        /// less then 
        /// </summary>
        /// <param name="thisValue">thisValue</param>
        /// <param name="compareValue">compareValue</param>
        /// <returns></returns>
        public static bool IsLessThan(this object thisValue, DateTime compareValue)
        {
            return Convert.ToDateTime(thisValue) < compareValue;
        }
        /// <summary>
        /// less than or equal
        /// </summary>
        /// <param name="thisValue">thisValue</param>
        /// <param name="compareValue">compareValue</param>
        /// <returns></returns>
        public static bool IsLessThanOrEqual(this object thisValue, DateTime compareValue)
        {
            return Convert.ToDateTime(thisValue) <= compareValue;
        }
        /// <summary>
        ///  less then 
        /// </summary>
        /// <param name="thisValue">thisValue</param>
        /// <param name="compareValue">compareValue</param>
        /// <returns></returns>
        public static bool IsGreaterThan(this object thisValue, DateTime compareValue)
        {
            return Convert.ToDateTime(thisValue) > compareValue;
        }
        /// <summary>
        /// ess than or equal
        /// </summary>
        /// <param name="thisValue">thisValue</param>
        /// <param name="compareValue">compareValue</param>
        /// <returns></returns>
        public static bool IsGreaterThanOrEqual(this object thisValue, DateTime compareValue)
        {
            return Convert.ToDateTime(thisValue) >= compareValue;
        }
        #endregion

        /// <summary>
        /// default min date
        /// </summary>
        public static DateTime MinDate => Convert.ToDateTime("1900-01-01");
    }   
}
