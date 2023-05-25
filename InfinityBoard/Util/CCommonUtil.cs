using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace BigglzPetJson.Dll.Util
{
    public static class CCommonUtil
    {
        /// <summary>
        /// 문자열 숫자를 int 형으로 변환, 널이면 0을 리턴 
        /// </summary>
        /// <param name="self">문자열 숫자</param>
        /// <returns>숫자</returns>
        public static int ToInt(this string self)
        {
            if (string.IsNullOrEmpty(self))
            {
                return 0;
            }

            if (!chkOnlyNum(self))
            {
                return 0;
            }

            return Convert.ToInt32(self);
        }

        public static UInt32 ToUInt(this string self)
        {
            if (string.IsNullOrEmpty(self))
            {
                return 0;
            }

            if (!chkOnlyNum(self))
            {
                return 0;
            }

            return Convert.ToUInt32(self);
        }


        public static UInt16 ToUShortInt(this string self)
        {
            if (string.IsNullOrEmpty(self))
            {
                return 0;
            }

            if (!chkOnlyNum(self))
            {
                return 0;
            }

            return Convert.ToUInt16(self);
        }

        public static UInt64 ToUBigInt(this string self)
        {
            if (string.IsNullOrEmpty(self))
            {
                return 0;
            }

            if (!chkOnlyNum(self))
            {
                return 0;
            }

            return Convert.ToUInt64(self);
        }

        /// <summary>
        /// 파라미터를 받아 허용된 값(숫자)이면 true를 반환
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static bool chkOnlyNum(string param)
        {
            bool retValue = false;
            if (!string.IsNullOrEmpty(param))
            {
                if (Regex.IsMatch(param, @"^[+-]?\d*(\.?\d*)$"))
                {
                    retValue = true;
                }
            }
            return retValue;
        }

        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        /// <summary>
        /// Web network error
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Tuple<bool, int> ToTuple(this WebException self)
        {
            bool result = false;
            int statusCode = 0;

            if (self.Status == WebExceptionStatus.ProtocolError)
            {
                var response = self.Response as HttpWebResponse;
                if (response != null)
                {
                    statusCode = (int)response.StatusCode;
                }
            }

            return Tuple.Create(result, statusCode);
        }


        /// <summary>
        /// GetTimeStamp
        /// </summary>
        /// <returns></returns>
        public static Int64 GetTimeStamp()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (Int64)timeSpan.TotalSeconds;
        }

        /// <summary>
        /// date로 WeekNo 가져오기
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetWeekNo(string date)
        {
            int result = 0;

            try
            {
                if (isDate(date))
                {
                    DateTime dt = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    CultureInfo ciCurr = CultureInfo.CurrentCulture;
                    result = ciCurr.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return result;
        }


        /// <summary>
        /// 날짜 문자열이 YYYYMMDD 형식인지 검사한다.
        /// </summary>
        /// <param date="YYYYMMDD or YYYY-MM-DD"></param>
        /// <returns></returns>
        private static bool isDate(string date)
        {
            bool result = false;
            result = Regex.IsMatch(date, @"^(19|20)\d{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[0-1])$");
            if (!result) result = Regex.IsMatch(date, @"^(19|20)\d{2}-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[0-1])$");

            return result;
        }

        public static UInt16 ToSInt(this string self)
        {
            if (string.IsNullOrEmpty(self))
            {
                return 0;
            }

            if (!chkOnlyNum(self))
            {
                return 0;
            }

            return Convert.ToUInt16(self);
        }

        /// <summary>
        /// 서버 루트 주소
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            return Path.Combine((string)AppDomain.CurrentDomain.GetData("WebRootPath"), path);
        }
    }
}
