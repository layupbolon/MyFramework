using Framework.Core.Configuration;
using Framework.Core.Data.Configuration;
using System;
using System.Data;
using System.Data.Common;

namespace Framework.Core.Data
{
    public static class DataCommandManager
    {

        #region GetDataCommand
        public static DataCommand GetDataCommand(string name)
        {
            var config = ConfigManager.GetConfig<DataConfig>();
            if (config != null)
            {
                foreach (var c in config.DataCommands)
                {
                    if (c.Name.ToLower() == name.ToLower())
                        return c;
                }
            }
            return null;
        }
        #endregion
    }
}
