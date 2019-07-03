﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace HISMedTypeManage.Rpt {
    using System;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class dsOutPatient : DataSet {
        
        private element1DataTable tableelement1;
        
        public dsOutPatient() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected dsOutPatient(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["element1"] != null)) {
                    this.Tables.Add(new element1DataTable(ds.Tables["element1"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.InitClass();
            }
            this.GetSerializationData(info, context);
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public element1DataTable element1 {
            get {
                return this.tableelement1;
            }
        }
        
        public override DataSet Clone() {
            dsOutPatient cln = ((dsOutPatient)(base.Clone()));
            cln.InitVars();
            return cln;
        }
        
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        protected override void ReadXmlSerializable(XmlReader reader) {
            this.Reset();
            DataSet ds = new DataSet();
            ds.ReadXml(reader);
            if ((ds.Tables["element1"] != null)) {
                this.Tables.Add(new element1DataTable(ds.Tables["element1"]));
            }
            this.DataSetName = ds.DataSetName;
            this.Prefix = ds.Prefix;
            this.Namespace = ds.Namespace;
            this.Locale = ds.Locale;
            this.CaseSensitive = ds.CaseSensitive;
            this.EnforceConstraints = ds.EnforceConstraints;
            this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
            this.InitVars();
        }
        
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new XmlTextReader(stream), null);
        }
        
        internal void InitVars() {
            this.tableelement1 = ((element1DataTable)(this.Tables["element1"]));
            if ((this.tableelement1 != null)) {
                this.tableelement1.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "dsOutPatient";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/dsOutPatient.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableelement1 = new element1DataTable();
            this.Tables.Add(this.tableelement1);
        }
        
        private bool ShouldSerializeelement1() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void element1RowChangeEventHandler(object sender, element1RowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class element1DataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnITEMCATID_CHR;
            
            private DataColumn columnTOTALMONEY;
            
            private DataColumn columnTYPENAME_VCHR;
            
            internal element1DataTable() : 
                    base("element1") {
                this.InitClass();
            }
            
            internal element1DataTable(DataTable table) : 
                    base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn ITEMCATID_CHRColumn {
                get {
                    return this.columnITEMCATID_CHR;
                }
            }
            
            internal DataColumn TOTALMONEYColumn {
                get {
                    return this.columnTOTALMONEY;
                }
            }
            
            internal DataColumn TYPENAME_VCHRColumn {
                get {
                    return this.columnTYPENAME_VCHR;
                }
            }
            
            public element1Row this[int index] {
                get {
                    return ((element1Row)(this.Rows[index]));
                }
            }
            
            public event element1RowChangeEventHandler element1RowChanged;
            
            public event element1RowChangeEventHandler element1RowChanging;
            
            public event element1RowChangeEventHandler element1RowDeleted;
            
            public event element1RowChangeEventHandler element1RowDeleting;
            
            public void Addelement1Row(element1Row row) {
                this.Rows.Add(row);
            }
            
            public element1Row Addelement1Row(string ITEMCATID_CHR, System.Single TOTALMONEY, string TYPENAME_VCHR) {
                element1Row rowelement1Row = ((element1Row)(this.NewRow()));
                rowelement1Row.ItemArray = new object[] {
                        ITEMCATID_CHR,
                        TOTALMONEY,
                        TYPENAME_VCHR};
                this.Rows.Add(rowelement1Row);
                return rowelement1Row;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                element1DataTable cln = ((element1DataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new element1DataTable();
            }
            
            internal void InitVars() {
                this.columnITEMCATID_CHR = this.Columns["ITEMCATID_CHR"];
                this.columnTOTALMONEY = this.Columns["TOTALMONEY"];
                this.columnTYPENAME_VCHR = this.Columns["TYPENAME_VCHR"];
            }
            
            private void InitClass() {
                this.columnITEMCATID_CHR = new DataColumn("ITEMCATID_CHR", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnITEMCATID_CHR);
                this.columnTOTALMONEY = new DataColumn("TOTALMONEY", typeof(System.Single), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnTOTALMONEY);
                this.columnTYPENAME_VCHR = new DataColumn("TYPENAME_VCHR", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnTYPENAME_VCHR);
                this.columnITEMCATID_CHR.AllowDBNull = false;
                this.columnTOTALMONEY.AllowDBNull = false;
            }
            
            public element1Row Newelement1Row() {
                return ((element1Row)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new element1Row(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(element1Row);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.element1RowChanged != null)) {
                    this.element1RowChanged(this, new element1RowChangeEvent(((element1Row)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.element1RowChanging != null)) {
                    this.element1RowChanging(this, new element1RowChangeEvent(((element1Row)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.element1RowDeleted != null)) {
                    this.element1RowDeleted(this, new element1RowChangeEvent(((element1Row)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.element1RowDeleting != null)) {
                    this.element1RowDeleting(this, new element1RowChangeEvent(((element1Row)(e.Row)), e.Action));
                }
            }
            
            public void Removeelement1Row(element1Row row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class element1Row : DataRow {
            
            private element1DataTable tableelement1;
            
            internal element1Row(DataRowBuilder rb) : 
                    base(rb) {
                this.tableelement1 = ((element1DataTable)(this.Table));
            }
            
            public string ITEMCATID_CHR {
                get {
                    return ((string)(this[this.tableelement1.ITEMCATID_CHRColumn]));
                }
                set {
                    this[this.tableelement1.ITEMCATID_CHRColumn] = value;
                }
            }
            
            public System.Single TOTALMONEY {
                get {
                    return ((System.Single)(this[this.tableelement1.TOTALMONEYColumn]));
                }
                set {
                    this[this.tableelement1.TOTALMONEYColumn] = value;
                }
            }
            
            public string TYPENAME_VCHR {
                get {
                    try {
                        return ((string)(this[this.tableelement1.TYPENAME_VCHRColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("无法获取值，因为它是 DBNull。", e);
                    }
                }
                set {
                    this[this.tableelement1.TYPENAME_VCHRColumn] = value;
                }
            }
            
            public bool IsTYPENAME_VCHRNull() {
                return this.IsNull(this.tableelement1.TYPENAME_VCHRColumn);
            }
            
            public void SetTYPENAME_VCHRNull() {
                this[this.tableelement1.TYPENAME_VCHRColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class element1RowChangeEvent : EventArgs {
            
            private element1Row eventRow;
            
            private DataRowAction eventAction;
            
            public element1RowChangeEvent(element1Row row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public element1Row Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}