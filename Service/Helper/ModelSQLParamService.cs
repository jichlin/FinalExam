using FinalExamService.Helper;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace IAI.Membership.Service.Helper
{
    public enum ParameterType { IncludeNull = 0, ExcludeNull = 1 }
    public class ModelSQLParamService<T> where T : class
    {

        public ParameterType Parameter { get; set; }
        public ModelSQLParamService()
        {

        }
        public virtual StoredProcedure SelectedParam(T Source, PropertyInfo[] Property)
        {
            StoredProcedure ReturnData = new StoredProcedure();
            ReturnData.SQLParamString = "";
            var Properties = Source.GetType().GetProperties();
            SqlParameter[] SqlParam = new SqlParameter[Property.Count() + 1];

            int i = 0;

            foreach (var Item in Properties)
            {
                if (Property.Where(X => X == Item).Count() > 0)
                {
                    SqlParam[i] = new SqlParameter("@" + Item.Name, Item.GetValue(Source, null));
                    ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Item.Name : ", @" + Item.Name);
                    i++;
                }

            }
            ReturnData.SQLParam = SqlParam;
            return ReturnData;
        }
        public virtual StoredProcedure Convert(T Source)
        {
            StoredProcedure ReturnData = new StoredProcedure();
            ReturnData.SQLParamString = "";
            var Properties = Source.GetType().GetProperties();
            SqlParameter[] SqlParam = new SqlParameter[Properties.Count() + 1];
            //SqlParameter[] SqlParamClean;
            int i = 0;

            foreach (var Property in Source.GetType().GetProperties())
            {
                SqlParam[i] = new SqlParameter("@" + Property.Name, Property.GetValue(Source, null));
                ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name : ", @" + Property.Name);
                i++;
            }

            ReturnData.SQLParam = SqlParam;
            return ReturnData;
        }
        public virtual StoredProcedure ConvertWithInCondition(T Source, ParameterType Type, string[] OnlyProperty)
        {
            StoredProcedure ReturnData = new StoredProcedure();
            ReturnData.SQLParamString = "";
            var Properties = Source.GetType().GetProperties();
            SqlParameter[] SqlParam = new SqlParameter[Properties.Count() + 1];
            //SqlParameter[] SqlParamClean;
            int i = 0;
            DateTime? DateItem;
            char CharItem;
            if (Type == ParameterType.ExcludeNull)
            {
                SqlParam = new SqlParameter[Properties.Where(item => item.GetValue(Source, null) != null).Count() + 1];
                foreach (var Property in Source.GetType().GetProperties())
                {
                    if (Property.GetValue(Source, null) != null)
                    {
                        if (OnlyProperty.Where(Item => Item.ToString() == Property.Name).Count() > 0)
                        {
                            if (Property.PropertyType == typeof(DateTime))
                            {
                                DateItem = (DateTime)Property.GetValue(Source, null);
                                if (DateItem != DateTime.MinValue)
                                {
                                    SqlParam[i] = new SqlParameter("@" + Property.Name, Property.GetValue(Source, null));
                                    ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name : ", @" + Property.Name);
                                    i++;
                                }
                            }
                            else if (Property.PropertyType == typeof(char))
                            {
                                CharItem = (char)Property.GetValue(Source, null);
                                if (CharItem != '\0')
                                {
                                    SqlParam[i] = new SqlParameter("@" + Property.Name, Property.GetValue(Source, null));
                                    ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name : ", @" + Property.Name);
                                    i++;
                                }
                            }
                            else
                            {
                                SqlParam[i] = new SqlParameter("@" + Property.Name, Property.GetValue(Source, null));
                                ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name : ", @" + Property.Name);
                                i++;
                            }
                        }
                    }

                }
                SqlParam = SqlParam.Where(Item => Item != null).ToArray();
            }
            else if (Type == ParameterType.IncludeNull)
            {
                foreach (var Property in Source.GetType().GetProperties())
                {
                    SqlParam[i] = new SqlParameter("@" + Property.Name, Property.GetValue(Source, null));
                    ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name : ", @" + Property.Name);
                    i++;
                }
            }
            ReturnData.SQLParam = SqlParam;
            return ReturnData;

        }
        public virtual StoredProcedure ConvertWithOutCondition(T Source, ParameterType Type, string[] ExcludeProperty)
        {
            StoredProcedure ReturnData = new StoredProcedure();
            ReturnData.SQLParamString = "";

            var Properties = Source.GetType().GetProperties();
            SqlParameter[] SqlParam = new SqlParameter[Properties.Count() + 1];

            int i = 0;
            DateTime? DateItem;
            char CharItem;
            if (Type == ParameterType.ExcludeNull)
            {
                SqlParam = new SqlParameter[Properties.Where(item => item.GetValue(Source, null) != null).Count() + 1];
                foreach (var Property in Source.GetType().GetProperties())
                {
                    if (Property.GetValue(Source, null) != null)
                    {
                        if (ExcludeProperty.Where(Item => Item.ToString() == Property.Name).Count() == 0)
                        {
                            if (Property.PropertyType == typeof(DateTime))
                            {
                                DateItem = (DateTime)Property.GetValue(Source, null);
                                if (DateItem != DateTime.MinValue)
                                {
                                    SqlParam[i] = new SqlParameter("@" + Property.Name, Property.GetValue(Source, null));
                                    ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name : ", @" + Property.Name);
                                    i++;
                                }
                            }
                            else if (Property.PropertyType == typeof(char))
                            {
                                CharItem = (char)Property.GetValue(Source, null);
                                if (CharItem != '\0')
                                {
                                    SqlParam[i] = new SqlParameter("@" + Property.Name, Property.GetValue(Source, null));
                                    ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name : ", @" + Property.Name);
                                    i++;
                                }
                            }
                            else
                            {
                                SqlParam[i] = new SqlParameter("@" + Property.Name, Property.GetValue(Source, null));
                                ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name : ", @" + Property.Name);
                                i++;
                            }
                        }
                    }

                }
                SqlParam = SqlParam.Where(Item => Item != null).ToArray();
            }
            else if (Type == ParameterType.IncludeNull)
            {
                foreach (var Property in Source.GetType().GetProperties())
                {
                    SqlParam[i] = new SqlParameter("@" + Property.Name, Property.GetValue(Source, null));
                    i++;
                }
            }

            ReturnData.SQLParam = SqlParam;
            return ReturnData;
        }

        public virtual StoredProcedure ConvertInSingleLineParam(T Source, ParameterType Type, string[] ExcludeProperty)
        {
            StoredProcedure ReturnData = new StoredProcedure();
            ReturnData.SQLParamString = "";
            var Binding = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
            var Options = PropertyReflectionOptions.IgnoreEnumerable | PropertyReflectionOptions.IgnoreIndexer;

            var Properties = ReflectionExtensions.GetProperties<T>(Binding, Options).ToList();


            int i = 0;
            DateTime? DateItem;
            char CharItem;

            if (Type == ParameterType.ExcludeNull)
            {
                foreach (var Property in Source.GetType().GetProperties())
                {


                    if (Property.GetValue(Source, null) != null)
                    {
                        var ItemData = Property.GetValue(Source, null);
                        if (ItemData.GetType() == typeof(string))
                        {
                            ItemData = ItemData.ToString().Replace("'", "''");
                        }
                        if (ExcludeProperty.Where(Item => Item.ToString() == Property.Name).Count() == 0)
                        {
                            if (Property.PropertyType == typeof(DateTime))
                            {
                                DateItem = (DateTime)ItemData;
                                if (DateItem != DateTime.MinValue)
                                {
                                    ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name + "='" + ItemData + "'" : ", @" + Property.Name + "='" + ItemData + "'");
                                    i++;
                                }
                            }
                            else if (Property.PropertyType == typeof(char))
                            {
                                CharItem = (char)ItemData;
                                if (CharItem != '\0')
                                {
                                    //SqlParam[i] = new SqlParameter("@" + Property.Name, ItemData);
                                    ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name + "='" + ItemData + "'" : ", @" + Property.Name + "='" + ItemData + "'");
                                    i++;
                                }
                            }
                            else if ((Property.PropertyType == typeof(int)) || (Property.PropertyType == typeof(Int16)) || (Property.PropertyType == typeof(Int32)) || (Property.PropertyType == typeof(Int64)))
                            {

                                if (ItemData != null)
                                {
                                    ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name + "=" + ItemData : ", @" + Property.Name + "=" + ItemData);
                                }
                            }
                            else
                            {

                                ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name + "='" + ItemData + "'" : ", @" + Property.Name + "='" + ItemData + "'");
                                i++;
                            }
                        }
                    }

                }
            }
            else if (Type == ParameterType.IncludeNull)
            {
                foreach (var Property in Source.GetType().GetProperties())
                {
                    var ItemData = Property.GetValue(Source, null);
                    if (ItemData.GetType() == typeof(string))
                    {
                        ItemData = ItemData.ToString().Replace("'", "''");
                    }
                    if ((Property.PropertyType == typeof(int)) || (Property.PropertyType == typeof(Int16)) || (Property.PropertyType == typeof(Int32)) || (Property.PropertyType == typeof(Int64)))
                    {
                        ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name + "=" + ItemData : ", @" + Property.Name + "=" + ItemData);
                    }
                    else
                    {
                        ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name + "=N'" + ItemData + "'" : ", @" + Property.Name + "='" + ItemData + "'");
                    }
                    i++;
                }
            }

            return ReturnData;
        }

        public virtual StoredProcedure ConvertInSingleLineParam(T Source, ParameterType Type)
        {
            StoredProcedure ReturnData = new StoredProcedure();
            ReturnData.SQLParamString = "";
            var Binding = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
            var Options = PropertyReflectionOptions.IgnoreEnumerable | PropertyReflectionOptions.IgnoreIndexer;

            var Properties = ReflectionExtensions.GetProperties<T>(Binding, Options).ToList();
            //var Properties = Source.GetType().GetProperties();

            int i = 0;
            DateTime? DateItem;
            char CharItem;

            if (Type == ParameterType.ExcludeNull)
            {
                foreach (var Property in Source.GetType().GetProperties())
                {
                    if (Property.GetValue(Source, null) != null)
                    {
                        var ItemData = Property.GetValue(Source, null);
                        if (ItemData.GetType() == typeof(string))
                        {
                            ItemData = ItemData.ToString().Replace("'", "''");
                        }
                        if (Property.PropertyType == typeof(DateTime))
                        {
                            DateItem = (DateTime)ItemData;
                            if (DateItem != DateTime.MinValue)
                            {
                                ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name + "='" + ItemData + "'" : ", @" + Property.Name + "='" + ItemData + "'");
                                i++;
                            }
                        }
                        else if (Property.PropertyType == typeof(char))
                        {
                            CharItem = (char)ItemData;
                            if (CharItem != '\0')
                            {
                                //SqlParam[i] = new SqlParameter("@" + Property.Name, ItemData);
                                ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name + "='" + ItemData + "'" : ", @" + Property.Name + "='" + ItemData + "'");
                                i++;
                            }
                        }
                        else if ((Property.PropertyType == typeof(int)) || (Property.PropertyType == typeof(Int16)) || (Property.PropertyType == typeof(Int32)) || (Property.PropertyType == typeof(Int64)))
                        {

                            if (ItemData != null)
                            {
                                ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name + "=" + ItemData : ", @" + Property.Name + "=" + ItemData);
                            }
                        }
                        else
                        {

                            ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name + "='" + ItemData + "'" : ", @" + Property.Name + "='" + ItemData + "'");
                            i++;
                        }
                    }

                }
            }
            else if (Type == ParameterType.IncludeNull)
            {
                foreach (var Property in Source.GetType().GetProperties())
                {
                    var ItemData = Property.GetValue(Source, null);
                    if (ItemData.GetType() == typeof(string))
                    {
                        ItemData = ItemData.ToString().Replace("'", "''");
                    }
                    if ((Property.PropertyType == typeof(int)) || (Property.PropertyType == typeof(Int16)) || (Property.PropertyType == typeof(Int32)) || (Property.PropertyType == typeof(Int64)))
                    {
                        ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name + "=" + ItemData : ", @" + Property.Name + "=" + ItemData);
                    }
                    else
                    {
                        ReturnData.SQLParamString += (ReturnData.SQLParamString == "" ? "@" + Property.Name + "=N'" + ItemData + "'" : ", @" + Property.Name + "='" + ItemData + "'");
                    }
                    i++;
                }
            }

            return ReturnData;
        }
    }
}

