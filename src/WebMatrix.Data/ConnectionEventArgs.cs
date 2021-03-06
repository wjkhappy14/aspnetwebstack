﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Data.Common;

namespace WebMatrix.Data
{


    /// <summary>
    /// 定义数据库连接事件参数
    /// </summary>
    public class ConnectionEventArgs : EventArgs
    {
        public ConnectionEventArgs(DbConnection connection)
        {
            Connection = connection;
        }

        public DbConnection Connection { get; private set; }
    }
}
