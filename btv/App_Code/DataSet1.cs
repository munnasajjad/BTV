using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

[Serializable, ToolboxItem(true), XmlRoot("DataSet1"), DesignerCategory("code"), HelpKeyword("vs.data.DataSet"), GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0"), XmlSchemaProvider("GetTypedDataSetSchema")]
public class DataSet1 : DataSet
{
    private System.Data.SchemaSerializationMode _schemaSerializationMode;
    private DataTable1DataTable tableDataTable1;

    [DebuggerNonUserCode]
    public DataSet1()
    {
        _schemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        base.BeginInit();
        InitClass();
        CollectionChangeEventHandler handler = new CollectionChangeEventHandler(SchemaChanged);
        base.Tables.CollectionChanged += handler;
        base.Relations.CollectionChanged += handler;
        base.EndInit();
    }

    [DebuggerNonUserCode]
    protected DataSet1(SerializationInfo info, StreamingContext context) : base(info, context, false)
    {
        _schemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        if (base.IsBinarySerialized(info, context))
        {
            InitVars(false);
            CollectionChangeEventHandler handler = new CollectionChangeEventHandler(SchemaChanged);
            Tables.CollectionChanged += handler;
            Relations.CollectionChanged += handler;
        }
        else
        {
            string s = (string) info.GetValue("XmlSchema", typeof(string));
            if (base.DetermineSchemaSerializationMode(info, context) == System.Data.SchemaSerializationMode.IncludeSchema)
            {
                DataSet dataSet = new DataSet();
                dataSet.ReadXmlSchema(new XmlTextReader(new StringReader(s)));
                if (dataSet.Tables["DataTable1"] != null)
                {
                    base.Tables.Add(new DataTable1DataTable(dataSet.Tables["DataTable1"]));
                }
                base.DataSetName = dataSet.DataSetName;
                base.Prefix = dataSet.Prefix;
                base.Namespace = dataSet.Namespace;
                base.Locale = dataSet.Locale;
                base.CaseSensitive = dataSet.CaseSensitive;
                base.EnforceConstraints = dataSet.EnforceConstraints;
                base.Merge(dataSet, false, MissingSchemaAction.Add);
                InitVars();
            }
            else
            {
                base.ReadXmlSchema(new XmlTextReader(new StringReader(s)));
            }
            base.GetSerializationData(info, context);
            CollectionChangeEventHandler handler2 = new CollectionChangeEventHandler(SchemaChanged);
            base.Tables.CollectionChanged += handler2;
            Relations.CollectionChanged += handler2;
        }
    }

    [DebuggerNonUserCode]
    public override DataSet Clone()
    {
        DataSet1 set = (DataSet1) base.Clone();
        set.InitVars();
        set.SchemaSerializationMode = SchemaSerializationMode;
        return set;
    }

    [DebuggerNonUserCode]
    protected override XmlSchema GetSchemaSerializable()
    {
        MemoryStream w = new MemoryStream();
        base.WriteXmlSchema(new XmlTextWriter(w, null));
        w.Position = 0L;
        return XmlSchema.Read(new XmlTextReader(w), null);
    }

    [DebuggerNonUserCode]
    public static XmlSchemaComplexType GetTypedDataSetSchema(XmlSchemaSet xs)
    {
        DataSet1 set = new DataSet1();
        XmlSchemaComplexType type = new XmlSchemaComplexType();
        XmlSchemaSequence sequence = new XmlSchemaSequence();
        XmlSchemaAny item = new XmlSchemaAny {
            Namespace = set.Namespace
        };
        sequence.Items.Add(item);
        type.Particle = sequence;
        XmlSchema schemaSerializable = set.GetSchemaSerializable();
        if (xs.Contains(schemaSerializable.TargetNamespace))
        {
            MemoryStream stream = new MemoryStream();
            MemoryStream stream2 = new MemoryStream();
            try
            {
                XmlSchema current = null;
                schemaSerializable.Write(stream);
                IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    current = (XmlSchema) enumerator.Current;
                    stream2.SetLength(0L);
                    current.Write(stream2);
                    if (stream.Length == stream2.Length)
                    {
                        stream.Position = 0L;
                        stream2.Position = 0L;
                        while ((stream.Position != stream.Length) && (stream.ReadByte() == stream2.ReadByte()))
                        {
                        }
                        if (stream.Position == stream.Length)
                        {
                            return type;
                        }
                    }
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (stream2 != null)
                {
                    stream2.Close();
                }
            }
        }
        xs.Add(schemaSerializable);
        return type;
    }

    [DebuggerNonUserCode]
    private void InitClass()
    {
        base.DataSetName = "DataSet1";
        base.Prefix = "";
        base.Namespace = "http://tempuri.org/DataSet1.xsd";
        base.EnforceConstraints = true;
        SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        tableDataTable1 = new DataTable1DataTable();
        base.Tables.Add(tableDataTable1);
    }

    [DebuggerNonUserCode]
    protected override void InitializeDerivedDataSet()
    {
        base.BeginInit();
        InitClass();
        base.EndInit();
    }

    [DebuggerNonUserCode]
    internal void InitVars()
    {
        InitVars(true);
    }

    [DebuggerNonUserCode]
    internal void InitVars(bool initTable)
    {
        tableDataTable1 = (DataTable1DataTable) base.Tables["DataTable1"];
        if (initTable && (tableDataTable1 != null))
        {
            tableDataTable1.InitVars();
        }
    }

    [DebuggerNonUserCode]
    protected override void ReadXmlSerializable(XmlReader reader)
    {
        if (base.DetermineSchemaSerializationMode(reader) == System.Data.SchemaSerializationMode.IncludeSchema)
        {
            Reset();
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(reader);
            if (dataSet.Tables["DataTable1"] != null)
            {
                base.Tables.Add(new DataTable1DataTable(dataSet.Tables["DataTable1"]));
            }
            base.DataSetName = dataSet.DataSetName;
            base.Prefix = dataSet.Prefix;
            base.Namespace = dataSet.Namespace;
            base.Locale = dataSet.Locale;
            base.CaseSensitive = dataSet.CaseSensitive;
            base.EnforceConstraints = dataSet.EnforceConstraints;
            base.Merge(dataSet, false, MissingSchemaAction.Add);
            InitVars();
        }
        else
        {
            base.ReadXml(reader);
            InitVars();
        }
    }

    [DebuggerNonUserCode]
    private void SchemaChanged(object sender, CollectionChangeEventArgs e)
    {
        if (e.Action == CollectionChangeAction.Remove)
        {
            InitVars();
        }
    }

    [DebuggerNonUserCode]
    private bool ShouldSerializeDataTable1()
    {
        return false;
    }

    [DebuggerNonUserCode]
    protected override bool ShouldSerializeRelations()
    {
        return false;
    }

    [DebuggerNonUserCode]
    protected override bool ShouldSerializeTables()
    {
        return false;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false), DebuggerNonUserCode]
    public DataTable1DataTable DataTable1
    {
        get
        {
            return tableDataTable1;
        }
    }

    [DebuggerNonUserCode, DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DataRelationCollection Relations
    {
        get
        {
            return base.Relations;
        }
    }

    [DebuggerNonUserCode, DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true)]
    public override System.Data.SchemaSerializationMode SchemaSerializationMode
    {
        get
        {
            return _schemaSerializationMode;
        }
        set
        {
            _schemaSerializationMode = value;
        }
    }

    [DebuggerNonUserCode, DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DataTableCollection Tables
    {
        get
        {
            return base.Tables;
        }
    }

    [Serializable, XmlSchemaProvider("GetTypedTableSchema"), GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    public class DataTable1DataTable : TypedTableBase<DataSet1.DataTable1Row>
    {
        private DataColumn columnDataColumn1;

        public event DataSet1.DataTable1RowChangeEventHandler DataTable1RowChanged;

        public event DataSet1.DataTable1RowChangeEventHandler DataTable1RowChanging;

        public event DataSet1.DataTable1RowChangeEventHandler DataTable1RowDeleted;

        public event DataSet1.DataTable1RowChangeEventHandler DataTable1RowDeleting;

        [DebuggerNonUserCode]
        public DataTable1DataTable()
        {
            base.TableName = "DataTable1";
            BeginInit();
            InitClass();
            EndInit();
        }

        [DebuggerNonUserCode]
        internal DataTable1DataTable(DataTable table)
        {
            base.TableName = table.TableName;
            if (table.CaseSensitive != table.DataSet.CaseSensitive)
            {
                base.CaseSensitive = table.CaseSensitive;
            }
            if (table.Locale.ToString() != table.DataSet.Locale.ToString())
            {
                base.Locale = table.Locale;
            }
            if (table.Namespace != table.DataSet.Namespace)
            {
                base.Namespace = table.Namespace;
            }
            base.Prefix = table.Prefix;
            base.MinimumCapacity = table.MinimumCapacity;
        }

        [DebuggerNonUserCode]
        protected DataTable1DataTable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            InitVars();
        }

        [DebuggerNonUserCode]
        public void AddDataTable1Row(DataSet1.DataTable1Row row)
        {
            base.Rows.Add(row);
        }

        [DebuggerNonUserCode]
        public DataSet1.DataTable1Row AddDataTable1Row(string DataColumn1)
        {
            DataSet1.DataTable1Row row = (DataSet1.DataTable1Row) base.NewRow();
            row.ItemArray = new object[] { DataColumn1 };
            base.Rows.Add(row);
            return row;
        }

        [DebuggerNonUserCode]
        public override DataTable Clone()
        {
            DataSet1.DataTable1DataTable table = (DataSet1.DataTable1DataTable) base.Clone();
            table.InitVars();
            return table;
        }

        [DebuggerNonUserCode]
        protected override DataTable CreateInstance()
        {
            return new DataSet1.DataTable1DataTable();
        }

        [DebuggerNonUserCode]
        protected override Type GetRowType()
        {
            return typeof(DataSet1.DataTable1Row);
        }

        [DebuggerNonUserCode]
        public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
        {
            XmlSchemaComplexType type = new XmlSchemaComplexType();
            XmlSchemaSequence sequence = new XmlSchemaSequence();
            DataSet1 set = new DataSet1();
            XmlSchemaAny item = new XmlSchemaAny {
                Namespace = "http://www.w3.org/2001/XMLSchema",
                MinOccurs = 0M,
                MaxOccurs = 79228162514264337593543950335M,
                ProcessContents = XmlSchemaContentProcessing.Lax
            };
            sequence.Items.Add(item);
            XmlSchemaAny any2 = new XmlSchemaAny {
                Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1",
                MinOccurs = 1M,
                ProcessContents = XmlSchemaContentProcessing.Lax
            };
            sequence.Items.Add(any2);
            XmlSchemaAttribute attribute = new XmlSchemaAttribute {
                Name = "namespace",
                FixedValue = set.Namespace
            };
            type.Attributes.Add(attribute);
            XmlSchemaAttribute attribute2 = new XmlSchemaAttribute {
                Name = "tableTypeName",
                FixedValue = "DataTable1DataTable"
            };
            type.Attributes.Add(attribute2);
            type.Particle = sequence;
            XmlSchema schemaSerializable = set.GetSchemaSerializable();
            if (xs.Contains(schemaSerializable.TargetNamespace))
            {
                MemoryStream stream = new MemoryStream();
                MemoryStream stream2 = new MemoryStream();
                try
                {
                    XmlSchema current = null;
                    schemaSerializable.Write(stream);
                    IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        current = (XmlSchema) enumerator.Current;
                        stream2.SetLength(0L);
                        current.Write(stream2);
                        if (stream.Length == stream2.Length)
                        {
                            stream.Position = 0L;
                            stream2.Position = 0L;
                            while ((stream.Position != stream.Length) && (stream.ReadByte() == stream2.ReadByte()))
                            {
                            }
                            if (stream.Position == stream.Length)
                            {
                                return type;
                            }
                        }
                    }
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                    if (stream2 != null)
                    {
                        stream2.Close();
                    }
                }
            }
            xs.Add(schemaSerializable);
            return type;
        }

        [DebuggerNonUserCode]
        private void InitClass()
        {
            columnDataColumn1 = new DataColumn("DataColumn1", typeof(string), null, MappingType.Element);
            base.Columns.Add(columnDataColumn1);
        }

        [DebuggerNonUserCode]
        internal void InitVars()
        {
            columnDataColumn1 = base.Columns["DataColumn1"];
        }

        [DebuggerNonUserCode]
        public DataSet1.DataTable1Row NewDataTable1Row()
        {
            return (DataSet1.DataTable1Row) base.NewRow();
        }

        [DebuggerNonUserCode]
        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new DataSet1.DataTable1Row(builder);
        }

        [DebuggerNonUserCode]
        protected override void OnRowChanged(DataRowChangeEventArgs e)
        {
            base.OnRowChanged(e);
            if (DataTable1RowChanged != null)
            {
                DataTable1RowChanged(this, new DataSet1.DataTable1RowChangeEvent((DataSet1.DataTable1Row) e.Row, e.Action));
            }
        }

        [DebuggerNonUserCode]
        protected override void OnRowChanging(DataRowChangeEventArgs e)
        {
            base.OnRowChanging(e);
            if (DataTable1RowChanging != null)
            {
                DataTable1RowChanging(this, new DataSet1.DataTable1RowChangeEvent((DataSet1.DataTable1Row) e.Row, e.Action));
            }
        }

        [DebuggerNonUserCode]
        protected override void OnRowDeleted(DataRowChangeEventArgs e)
        {
            base.OnRowDeleted(e);
            if (DataTable1RowDeleted != null)
            {
                DataTable1RowDeleted(this, new DataSet1.DataTable1RowChangeEvent((DataSet1.DataTable1Row) e.Row, e.Action));
            }
        }

        [DebuggerNonUserCode]
        protected override void OnRowDeleting(DataRowChangeEventArgs e)
        {
            base.OnRowDeleting(e);
            if (DataTable1RowDeleting != null)
            {
                DataTable1RowDeleting(this, new DataSet1.DataTable1RowChangeEvent((DataSet1.DataTable1Row) e.Row, e.Action));
            }
        }

        [DebuggerNonUserCode]
        public void RemoveDataTable1Row(DataSet1.DataTable1Row row)
        {
            base.Rows.Remove(row);
        }

        [DebuggerNonUserCode, Browsable(false)]
        public int Count
        {
            get
            {
                return base.Rows.Count;
            }
        }

        [DebuggerNonUserCode]
        public DataColumn DataColumn1Column
        {
            get
            {
                return columnDataColumn1;
            }
        }

        [DebuggerNonUserCode]
        public DataSet1.DataTable1Row this[int index]
        {
            get
            {
                return (DataSet1.DataTable1Row) base.Rows[index];
            }
        }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    public class DataTable1Row : DataRow
    {
        private DataSet1.DataTable1DataTable tableDataTable1;

        [DebuggerNonUserCode]
        internal DataTable1Row(DataRowBuilder rb) : base(rb)
        {
            tableDataTable1 = (DataSet1.DataTable1DataTable) base.Table;
        }

        [DebuggerNonUserCode]
        public bool IsDataColumn1Null()
        {
            return base.IsNull(tableDataTable1.DataColumn1Column);
        }

        [DebuggerNonUserCode]
        public void SetDataColumn1Null()
        {
            base[tableDataTable1.DataColumn1Column] = Convert.DBNull;
        }

        [DebuggerNonUserCode]
        public string DataColumn1
        {
            get
            {
                string str;
                try
                {
                    str = (string) base[tableDataTable1.DataColumn1Column];
                }
                catch (InvalidCastException exception)
                {
                    throw new StrongTypingException("The value for column 'DataColumn1' in table 'DataTable1' is DBNull.", exception);
                }
                return str;
            }
            set
            {
                base[tableDataTable1.DataColumn1Column] = value;
            }
        }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    public class DataTable1RowChangeEvent : EventArgs
    {
        private DataRowAction eventAction;
        private DataSet1.DataTable1Row eventRow;

        [DebuggerNonUserCode]
        public DataTable1RowChangeEvent(DataSet1.DataTable1Row row, DataRowAction action)
        {
            eventRow = row;
            eventAction = action;
        }

        [DebuggerNonUserCode]
        public DataRowAction Action
        {
            get
            {
                return eventAction;
            }
        }

        [DebuggerNonUserCode]
        public DataSet1.DataTable1Row Row
        {
            get
            {
                return eventRow;
            }
        }
    }

    public delegate void DataTable1RowChangeEventHandler(object sender, DataSet1.DataTable1RowChangeEvent e);
}

