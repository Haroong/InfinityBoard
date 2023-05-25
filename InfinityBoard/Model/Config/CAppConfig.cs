namespace InfinityBoard.Model.Config
{
    public class CAppConfig
    {
        private static AppSetting APP_SETTING;
        private static CConnectionConfig cConnectionConfig;
        private static CDBInfoConfig cDBInfoConfig;

        #region 앱 모드(TEST/REAL)

        public static AppSetting GetAPP_SETTING()
        {
            return APP_SETTING;
        }

        private static void SetAPP_SETTING(AppSetting value)
        {
            APP_SETTING = value;
        }

        #endregion

        #region DB 연결 설정
        public static CDBInfoConfig GetDBConnectionMode()
        {
            return cDBInfoConfig;
        }

        public static void SetDBConnectionMode(CConnectionConfig value)
        {
            if (APP_SETTING.APP_MODE.Equals("TEST"))
            {
                cDBInfoConfig = value.TEST;
            }
            else
            {
                cDBInfoConfig = value.REAL;
            }
        }

        #endregion

        public CAppConfig(AppSetting appSetting, CConnectionConfig connectionConfig)
        {
            SetAPP_SETTING(appSetting);
            SetDBConnectionMode(connectionConfig);
        }
    }
}
