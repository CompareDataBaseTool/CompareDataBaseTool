using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompareDataBaseTool
{
    public class LogType
    {
        public const string colunmType = "列";
        public const string tableType = "表";
        public const string fkType = "外键约束";
        public const string pkType = "主键约束";
        public const string dfType = "Default约束";
        public const string uqType = "唯一约束";
        public const string ckType = "Check约束";
        public const string viewType = "视图";
        public const string indexType = "索引";
        public const string addType = "新增";
        public const string delType = "删除";
        public const string editType = "修改";
    }
    public class TableNameClass
    {
        /// <summary>
        /// 主键
        /// </summary>
        public const string newPKDT = "newPK";
        public const string oldPKDT = "oldPK";
        /// <summary>
        /// 外键
        /// </summary>
        public const string newFKDT = "newFK";
        public const string oldFKDT = "oldFK";
        /// <summary>
        /// 检查（Check）约束
        /// </summary>
        public const string newCKDT = "newCK";
        public const string oldCKDT = "oldCK";
        /// <summary>
        /// 默认(Default)约束
        /// </summary>
        public const string newDFDT = "newDF";
        public const string oldDFDT = "oldDF";
        /// <summary>
        /// 唯一(UNIQUE)约束
        /// </summary>
        public const string newUQDT = "newUQ";
        public const string oldUQDT = "oldUQ";
        /// <summary>
        /// 索引
        /// </summary>
        public const string newIndexDT = "newIndex";
        public const string oldIndexDT = "oldIndex";
        /// <summary>
        /// 视图
        /// </summary>
        public const string newViewDT = "newView";
        public const string oldViewDT = "oldView";

        /// <summary>
        /// 视图列
        /// </summary>
        public const string newViewColumnDT = "newViewColumn";
        public const string oldViewColumnDT = "oldViewColumn";
        /// <summary>
        /// 列
        /// </summary>
        public const string newColumnsDT = "newColumns";
        public const string oldColumnsDT = "oldColumns";
        /// <summary>
        /// Table
        /// </summary>
        public const string newTablesDT = "newTables";
        public const string oldTablesDT = "oldTables";
        /// <summary>
        /// 存储结果
        /// </summary>
        public const string resultDT = "resultDT";
    }
}
