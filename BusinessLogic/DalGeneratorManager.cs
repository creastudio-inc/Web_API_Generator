using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class DalGeneratorManager
    {
        private String _test;
        public String Test { get { return _test; } set { _test = value; } }

        private List<DataField> getDataField(String tableName, String cnxString)
        {
            List<DataField> lstDataField = new List<DataField>();

            //Call Dal : 
            DataAccess da = new DataAccess();
            DataTable dt = da.GetInformationForDataTable(tableName, cnxString);


            int count = dt.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                DataField dataField = new DataField();
                if (dt.Rows[i]["Length"] != null)
                    dataField.Lengh = dt.Rows[i]["Length"].ToString();
                if (dt.Rows[i]["Column_name"] != null)
                    dataField.Name = dt.Rows[i]["Column_name"].ToString();
                //Check nullable 
                if (dt.Rows[i]["Nullable"] != null)
                {
                    if (dt.Rows[i]["Nullable"].ToString() == "no" || dt.Rows[i]["Nullable"].ToString() == "non")
                        dataField.Nullable = false;
                    else
                        dataField.Nullable = true;
                }
                if (dt.Rows[i]["Type"] != null)
                    dataField.Type = dt.Rows[i]["Type"].ToString();

                //Creation Parametre : 
                dataField.ParametersName = CreateSqlParametersName(dataField.Name);

                lstDataField.Add(dataField);
            }

            return lstDataField;
        }

        private String CreateSqlParametersName(String fieldName)
        {
            //Recuperation d'un champ => remplacement de la case sur la premiere lettre + ajouter @Devant
            String firstChar = String.Empty;
            if (!String.IsNullOrEmpty(fieldName))
                firstChar = fieldName[0].ToString();

            String sqlParameterName = String.Empty;
            if (!String.IsNullOrEmpty(firstChar))
                sqlParameterName = String.Format("@{0}{1}", firstChar.ToLower(), fieldName.Substring(1));
            return sqlParameterName;
        }
        public List<String> GetDatabasesOnServer(String cnxString)
        {
            DataAccess da = new DataAccess();
            return da.GetLstDataBases(cnxString);
        }
        public List<String> GetTableOnDataBase(String cnxString)
        {
            DataAccess da = new DataAccess();
            return da.GetTableInDataBase(cnxString);
        }

        public List<String> generateDataAccess(String tableName, String cnxString)
        {
            List<String> lstGeneratedCode = new List<string>();
            //Generate Code 
            StringBuilder sb = new StringBuilder();

            //Create Business Entities : 

            sb.Append("using System;\nusing System.Data;\nusing System.Data.Common;\nusing System.Collections.Generic;\nusing System.Runtime.Serialization;\n\n//using reference Enterprise library \n\n");
            sb.Append("\n\n\n//------------------------------ BUSINESS ENTITIES --------------------\n");

            sb.Append(String.Format("public class BE{0} ", tableName));
            sb.Append("{ \n");
            List<DataField> lstDataField = new List<DataField>();
            lstDataField = getDataField(tableName, cnxString);

            if (lstDataField == null)
                throw new ArgumentNullException("lstDataField");

            //Create Properties : 

            foreach (DataField df in lstDataField)
            {
                if (df.Nullable)
                {
                    if (df.Type != "varchar" && df.Type != "char" && df.Type != "nvarchar")
                    {
                        sb.Append(String.Format("private Nullable<{0}> _{1}; \n", TranslateToDotNetType(df.Type), df.Name));
                        sb.Append("public Nullable<" + TranslateToDotNetType(df.Type) + "> " + df.Name + "{get{return _" + df.Name + ";}set{_" + df.Name + " = value;");
                    }
                    else
                    {
                        sb.Append(String.Format("private {0} _{1}; \n", TranslateToDotNetType(df.Type), df.Name));
                        sb.Append("public " + TranslateToDotNetType(df.Type) + " " + df.Name + "{get{return _" + df.Name + ";}set{_" + df.Name + " = value;");
                    }
                }
                else
                {
                    sb.Append(String.Format("private {0} _{1}; \n", TranslateToDotNetType(df.Type), df.Name));
                    sb.Append("public " + TranslateToDotNetType(df.Type) + " " + df.Name + "{get{return _" + df.Name + ";}set{_" + df.Name + " = value;");

                }
                sb.Append("}} \n\n");
            }
            sb.Append("} \n\n");

            sb.Append("\n\n\n//------------------------------ DATA Contract --------------------\n");

            sb.Append(String.Format("[DataContract(Namespace = \"MyNameSpace\", Name = \"DC{0}\")]\n\n", tableName));
            sb.Append(String.Format("public class DC{0} ", tableName));
            sb.Append("{ \n");

            //Create Properties : 
            int i = 0;
            foreach (DataField df in lstDataField)
            {
                if (df.Nullable)
                {
                    if (df.Type != "varchar" && df.Type != "char" && df.Type != "nvarchar")
                    {
                        sb.Append(String.Format("private Nullable<{0}> _{1}; \n", TranslateToDotNetType(df.Type), df.Name));
                        sb.Append(String.Format("[DataMember(IsRequired = true, Name = \"{0}\", Order = {1})]\n", df.Name, i.ToString()));
                        sb.Append("public Nullable<" + TranslateToDotNetType(df.Type) + "> " + df.Name + "{get{return _" + df.Name + ";}set{_" + df.Name + " = value;");
                    }
                    else
                    {
                        sb.Append(String.Format("private {0} _{1}; \n", TranslateToDotNetType(df.Type), df.Name));
                        sb.Append(String.Format("[DataMember(IsRequired = true, Name = \"{0}\", Order = {1})]\n", df.Name, i.ToString()));
                        sb.Append("public " + TranslateToDotNetType(df.Type) + " " + df.Name + "{get{return _" + df.Name + ";}set{_" + df.Name + " = value;");
                    }
                }
                else
                {
                    sb.Append(String.Format("private {0} _{1}; \n", TranslateToDotNetType(df.Type), df.Name));
                    sb.Append(String.Format("[DataMember(IsRequired = true, Name = \"{0}\", Order = {1})]\n", df.Name, i.ToString()));
                    sb.Append("public " + TranslateToDotNetType(df.Type) + " " + df.Name + "{get{return _" + df.Name + ";}set{_" + df.Name + " = value;");

                }
                sb.Append("}} \n\n");
                i++;
            }
            sb.Append("} \n\n");

            sb.Append("\n\n\n//------------------------------ Mapping Manager --------------------\n");
            sb.Append(String.Format("\n\n\n//------------------------------ Translate DC{0} to BE{0}--------------------\n", tableName));
            sb.Append("public static class MappingManager {\n");
            sb.Append(String.Format("public static BE{0} TranslateDC{0}ToBE{0}(DC{0} from)", tableName));
            sb.Append("{\n");
            sb.Append(String.Format("BE{0} to = null;\n", tableName));
            sb.Append("if(from!=null){\n");
            sb.Append(String.Format("to = new BE{0}();\n", tableName));
            //Mapping
            foreach (DataField df in lstDataField)
                sb.Append(String.Format("to.{0} = from.{0};\n", df.Name));
            sb.Append("}\n");
            //return
            sb.Append("return to;");
            sb.Append("}\n\n");

            sb.Append(String.Format("\n\n\n//------------------------------ Translate BE{0} to DC{0}--------------------\n", tableName));
            sb.Append(String.Format("public static DC{0} TranslateBE{0}ToDC{0}(BE{0} from)", tableName));
            sb.Append("{\n");
            sb.Append(String.Format("DC{0} to = null;\n", tableName));
            sb.Append("if(from!=null){\n");
            sb.Append(String.Format("to = new DC{0}();\n", tableName));
            //Mapping
            foreach (DataField df in lstDataField)
                sb.Append(String.Format("to.{0} = from.{0};\n", df.Name));
            sb.Append("}\n");
            //return
            sb.Append("return to;");
            sb.Append("}\n");
            sb.Append("}\n\n");


            sb.Append("\n\n\n//------------------------------ DATA ACCESS WITH ENTLIB --------------------\n");


            sb.Append("\n\n\n//------------------------------ GET Method --------------------\n");

            //sb.Append("using System; \n using System.Data;\nusing System.Collections.Generic;\n //Using AddReferenceToEnterpriseLibrary; \n\n ");
            sb.Append("public class DataAccess{\n");
            sb.Append("private const string CNX_DATABASE = \"connexionName\";\n");
            sb.Append(String.Format("public List<BE{0}> Get{0}()", tableName));
            sb.Append("{\n");
            sb.Append(String.Format("List<BE{0}> lstBE{0} = new List<BE{0}>();\n", tableName));
            sb.Append("Database bdd = null;\nDbCommand command = null;\nIDataReader dr = null;\ntry{\nbdd = DatabaseFactory.CreateDatabase(CNX_DATABASE);\n");
            sb.Append(String.Format("command = bdd.GetStoredProcCommand(\"[dbo].[Get{0}]\");\n\n", tableName));

            sb.Append("dr = bdd.ExecuteReader(command);\n");

            sb.Append(String.Format("BE{0} result = null;\n", tableName));

            sb.Append("if(dr!=null){\n");
            sb.Append("while(dr.Read()){\n");
            sb.Append(String.Format("result = new BE{0}();\n", tableName));

            foreach (DataField df in lstDataField)
            {
                sb.Append(String.Format("int ord{0} = dr.GetOrdinal(\"{0}\");\n", df.Name));
                sb.Append(String.Format("if (!dr.IsDBNull(ord{0}))\n", df.Name));
                sb.Append(String.Format("result.{0} = ", df.Name));
                sb.Append(String.Format("dr.Get{0}(ord{1});\n\n", TranslateToDotNetType(df.Type), df.Name));
            }
            sb.Append(String.Format("lstBE{0}.Add(result);\n", tableName));
            sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("dr.Close();\n");
            sb.Append("}\n");
            sb.Append("catch(Exception ex){\n //Add code if exception is catched \n }");
            sb.Append("}\n");


            //Generation de la methode d'insert :
            sb.Append("\n\n\n//------------------------------ Insert Method --------------------\n");

            sb.Append(String.Format("public static bool Insert{0}(BE{0} be{0})", tableName));
            sb.Append("{\n");
            sb.Append(String.Format("if (be{0} == null)\n throw new ArgumentNullException(\"be{0}\");\n", tableName));

            sb.Append("Database bdd = null;\nDbCommand command = null;\ntry{\nbdd = DatabaseFactory.CreateDatabase(CNX_DATABASE);\n");
            sb.Append(String.Format("command = bdd.GetStoredProcCommand(\"[dbo].[Insert{0}]\");\n\n", tableName));

            sb.Append("#region add parameters\n");

            //Ajout des parametres @ la prodStock : 
            foreach (DataField df in lstDataField)
            {
                if (df.Nullable)
                {
                    if (df.Type != "char" && df.Type != "varchar" && df.Type != "nvarchar")
                    {
                        sb.Append(String.Format("if (be{0}.{1}.HasValue)\n", tableName, df.Name));
                        sb.Append("bdd.AddInParameter(command, \"" + df.ParametersName + "\", DbType." + TranslateToDotNetType(df.Type) + ",be" + tableName + "." + df.Name + ".Value);\n");
                        sb.Append(String.Format("else\n bdd.AddInParameter(command, \"{0}\", DbType.{1}, DBNull.Value);\n\n", df.ParametersName, TranslateToDotNetType(df.Type)));
                    }
                    else
                    {
                        sb.Append(String.Format("if (be{0}.{1} !=null)\n", tableName, df.Name));
                        sb.Append("bdd.AddInParameter(command, \"" + df.ParametersName + "\", DbType." + TranslateToDotNetType(df.Type) + ",be" + tableName + "." + df.Name + ");\n");
                        sb.Append(String.Format("else\n bdd.AddInParameter(command, \"{0}\", DbType.{1}, DBNull.Value);\n\n", df.ParametersName, TranslateToDotNetType(df.Type)));
                    }
                }
                else
                {
                    sb.Append(String.Format("bdd.AddInParameter(command, \"{0}\", DbType.{1}, be" + tableName + "." + df.Name + ");\n\n", df.ParametersName, TranslateToDotNetType(df.Type)));

                }
            }

            sb.Append("#endregion\n");

            sb.Append("bdd.ExecuteNonQuery(command);\nif(command!=null)\ncommand.Dispose();");

            //sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("catch(Exception ex){\n //Add code if exception is catched \n }");
            sb.Append("}");

            //Generation de la methode d'update :
            sb.Append("\n\n\n//------------------------------ Update Method --------------------\n");

            sb.Append(String.Format("public static bool Update{0}(BE{0} be{0})", tableName));
            sb.Append("{\n");
            sb.Append(String.Format("if (be{0} == null)\n throw new ArgumentNullException(\"be{0}\");\n", tableName));

            sb.Append("Database bdd = null;\nDbCommand command = null;\ntry{\nbdd = DatabaseFactory.CreateDatabase(CNX_DATABASE);\n");
            sb.Append(String.Format("command = bdd.GetStoredProcCommand(\"[dbo].[Update{0}]\");\n\n", tableName));

            sb.Append("#region add parameters\n");

            //Ajout des parametres @ la prodStock : 
            foreach (DataField df in lstDataField)
            {
                if (df.Nullable)
                {
                    if (df.Type != "char" && df.Type != "varchar" && df.Type != "nvarchar")
                    {
                        sb.Append(String.Format("if (be{0}.{1}.HasValue)\n", tableName, df.Name));
                        sb.Append("bdd.AddInParameter(command, \"" + df.ParametersName + "\", DbType." + TranslateToDotNetType(df.Type) + ",be" + tableName + "." + df.Name + ".Value);\n");
                        sb.Append(String.Format("else\n bdd.AddInParameter(command, \"{0}\", DbType.{1}, DBNull.Value);\n\n", df.ParametersName, TranslateToDotNetType(df.Type)));
                    }
                    else
                    {
                        sb.Append(String.Format("if (be{0}.{1} !=null)\n", tableName, df.Name));
                        sb.Append("bdd.AddInParameter(command, \"" + df.ParametersName + "\", DbType." + TranslateToDotNetType(df.Type) + ",be" + tableName + "." + df.Name + ");\n");
                        sb.Append(String.Format("else\n bdd.AddInParameter(command, \"{0}\", DbType.{1}, DBNull.Value);\n\n", df.ParametersName, TranslateToDotNetType(df.Type)));
                    }
                }
                else
                {
                    sb.Append(String.Format("bdd.AddInParameter(command, \"{0}\", DbType.{1}, be" + tableName + "." + df.Name + ");\n\n", df.ParametersName, TranslateToDotNetType(df.Type)));

                }
            }

            sb.Append("#endregion\n");
            sb.Append("bdd.ExecuteNonQuery(command);\nif(command!=null)\ncommand.Dispose();");

            //sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("catch(Exception ex){\n //Add code if exception is catched \n }");
            sb.Append("}\n");
            sb.Append("}");



            lstGeneratedCode.Add(sb.ToString());
            //Add procStock to the list 
            lstGeneratedCode.Add(generateStoredProcedures(tableName, lstDataField));

            return lstGeneratedCode;

        }

        public String generateStoredProcedures(String tableName, List<DataField> lstDataField)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(String.Format("-- Generated Code : stored procedure to Get in {0}\n\n", tableName));
            sb.Append(String.Format("CREATE PROCEDURE Get{0}\n AS \n Begin \n SELECT \n", tableName));

            foreach (DataField df in lstDataField)
            {
                sb.Append(String.Format("{0},\n", df.Name));
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(String.Format("\nFROM \n dbo.{0}\nEnd\nGO", tableName));


            sb.Append(String.Format("-- Generated Code : stored procedure to Insert in {0}\n\n", tableName));
            sb.Append(String.Format("CREATE PROCEDURE Insert{0}\n", tableName));
            //Ajout des parametres : 
            foreach (DataField df in lstDataField)
            {
                sb.Append(String.Format("{0} {1}", df.ParametersName, df.Type));
                if (df.Type == "varchar" || df.Type == "char")
                    sb.Append(String.Format("({0}),\n", df.Lengh));
                else
                    sb.Append(",\n");
            }
            sb.Remove(sb.Length - 2, 2);

            sb.Append(String.Format("\nAS \n Begin \n INSERT INTO {0} (\n", tableName));
            foreach (DataField df in lstDataField)
            {
                sb.Append(String.Format("{0},\n", df.Name));
            }
            sb.Remove(sb.Length - 2, 2); //delete the last coma 

            sb.Append("\n)\nvalues\n(\n");
            foreach (DataField df in lstDataField)
            {
                sb.Append(String.Format("{0},\n", df.ParametersName));
            }
            sb.Remove(sb.Length - 2, 2);

            sb.Append("\n)\nEnd\nGO\n\n");

            //Generation de l'update : 
            sb.Append(String.Format("-- Generated Code : stored procedure to Update in {0}\n\n", tableName));
            sb.Append(String.Format("CREATE PROCEDURE Update{0}\n", tableName));
            //Ajout des parametres : 
            foreach (DataField df in lstDataField)
            {
                sb.Append(String.Format("{0} {1}", df.ParametersName, df.Type));
                if (df.Type == "varchar" || df.Type == "char")
                    sb.Append(String.Format("({0}),\n", df.Lengh));
                else
                    sb.Append(",\n");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(String.Format("\nAS \n Begin \n UPDATE {0} \nset\n", tableName));
            //Update : 
            foreach (DataField df in lstDataField)
            {
                sb.Append(string.Format("{0} = {1} ,\n", df.Name, df.ParametersName));
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append("\n -- WHERE id = @id (Complete condition)\nend\nGO");


            return sb.ToString();
        }
        private string TranslateToDotNetType(string dataFieldType)
        {
            //Translate Sql Type to DotNet
            switch (dataFieldType)
            {
                case "int":
                    return "Int32";
                case "char":
                    return "String";
                case "varchar":
                    return "String";
                case "nvarchar":
                    return "String";
                case "bigint":
                    return "Int64";
                case "smallint":
                    return "Int16";
                case "bit":
                    return "Boolean";
                case "datetime":
                    return "DateTime";
                case "decimal":
                    return "Decimal";
                default:
                    return "object";
            }
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
