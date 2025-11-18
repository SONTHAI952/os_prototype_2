using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using ZeroX.CsvTools;
using ZeroX.DataTableSystem.SoTableSystem;

namespace ZeroX.DataTableSystem.Editors
{
    public partial class TableToolDrawer
    {
        //Export
        private bool convertHeaderCamelCaseToSnakeCase = true;
        private bool convertEnumCamelCaseToSnakeCase = true;
        private bool includeRowType = false;
        
        
        
        private void DrawTab_Export()
        {
            DrawOption_ConvertHeaderCamelCaseToSnakeCase();
            DrawOption_ConvertEnumCamelCaseToSnakeCase();
            DrawOption_IncludeRowType();
            GUILayout.Space(15);

            
            
            
            
            //Export To Clipboard
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Export To Clipboard", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("With Header", GUILayout.Height(35)))
            {
                ExportToClipboard_WithHeader();
            }
            
            if (GUILayout.Button("No Header", GUILayout.Height(35)))
            {
                ExportToClipboard_NoHeader();
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            
            
            
            
            
            GUILayout.Space(30);
            
            
            
            
            
            //Export Only Row Type To Clipboard
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Export Only Row Type To Clipboard");
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("With Header", GUILayout.Height(20)))
            {
                ExportToClipboard_OnlyRowType_WithHeader();
            }
            
            if (GUILayout.Button("No Header", GUILayout.Height(20)))
            {
                ExportToClipboard_OnlyRowType_NoHeader();
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawOption_ConvertHeaderCamelCaseToSnakeCase()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Convert Header: camelCase to snake_case", GUILayout.Width(250));
            GUILayout.Space(30);
            convertHeaderCamelCaseToSnakeCase = EditorGUILayout.Toggle(convertHeaderCamelCaseToSnakeCase);
            GUILayout.EndHorizontal();
        }
        
        private void DrawOption_ConvertEnumCamelCaseToSnakeCase()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Convert Enum: camelCase to snake_case", GUILayout.Width(250));
            GUILayout.Space(30);
            convertEnumCamelCaseToSnakeCase = EditorGUILayout.Toggle(convertEnumCamelCaseToSnakeCase);
            GUILayout.EndHorizontal();
        }
        private void DrawOption_IncludeRowType()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Include Row Type", GUILayout.Width(250));
            GUILayout.Space(30);
            includeRowType = EditorGUILayout.Toggle(includeRowType);
            bool isClickRecommendOption = GUILayout.Button("R", GUILayout.Width(20));
            GUILayout.EndHorizontal();
            
            if (isClickRecommendOption)
            {
                includeRowType = CalculateRecommendOption_IncludeRowType();
            }
        }

        private bool CalculateRecommendOption_IncludeRowType()
        {
            Type declaredRowType = SOTableReflection.GetDeclaredRowType(tableAsset);
            if (declaredRowType.IsAbstract || declaredRowType.IsInterface)
                return true;


            var listRow = SOTableReflection.GetListRow(tableAsset);
            HashSet<Type> hashSetRowType = new HashSet<Type>();
            foreach (var row in listRow)
            {
                if(row == null)
                    continue;

                hashSetRowType.Add(row.GetType());
                if (hashSetRowType.Count > 1)
                    return true;
            }

            return false;
        }

        private List<string> CreateListHeader()
        {
            var fieldInfos = SOTableReflection.GetListFieldInfoOfRow(tableAsset);

            List<string> list = new List<string>();
            foreach (var fieldInfo in fieldInfos)
            {
                if(convertHeaderCamelCaseToSnakeCase)
                    list.Add(Convert_CamelCase_To_SnakeCase(fieldInfo.Name));
                else
                    list.Add(fieldInfo.Name);
            }

            return list;
        }

        

        private List<List<string>> ConvertTableRows_To_ListRowString()
        {
            var listRow = SOTableReflection.GetListRow(tableAsset);
            if (listRow == null)
            {
                return new List<List<string>>();
            }


            var listFieldInfos = SOTableReflection.GetListFieldInfoOfRow(tableAsset); //Danh sách các fieldInfo của row
            List<List<string>> listRowResult = new List<List<string>>();
            foreach (var rowObj in listRow)
            {
                var rowResult = new List<string>();
                listRowResult.Add(rowResult);
                
                foreach (var fieldInfo in listFieldInfos)
                {
                    if (fieldInfo.DeclaringType.IsInstanceOfType(rowObj) == false)
                    {
                        rowResult.Add("");
                        continue;
                    }
                    
                    var value = fieldInfo.GetValue(rowObj);
                    
                    //Nếu value null thì thêm chuỗi rỗng
                    if (value == null)
                    {
                        rowResult.Add("");
                        continue;
                    }
                    
                    
                    //Kiểm tra xem có thể convert sang json không
                    string json = "";

                    try
                    {
                        json = JsonConvert.SerializeObject(value);
                    }
                    catch (Exception e)
                    {
                        json = JsonUtility.ToJson(value);
                    }
                    
                    
                    if (string.IsNullOrEmpty(json) == false && (json[0] == '{' || json[0] == '['))
                    {
                        rowResult.Add(json);
                        continue;
                    }

                    if (fieldInfo.FieldType.IsEnum)
                    {
                        if(convertEnumCamelCaseToSnakeCase)
                            rowResult.Add(Convert_CamelCase_To_SnakeCase(value.ToString()));
                        else
                            rowResult.Add(value.ToString());
                        
                        continue;
                    }
                    
                    
                    rowResult.Add(string.Format(CultureInfo.InvariantCulture, "{0}", value));
                }
            }

            return listRowResult;
        }

        private List<string> ConvertTableRows_To_ListRowType()
        {
            var listRow = SOTableReflection.GetListRow(tableAsset);
            if (listRow == null || listRow.Count == 0)
            {
                return new List<string>();
            }
            
            List<string> listRowType = new List<string>();
            foreach (object rowObj in listRow)
            {
                var rowType = rowObj.GetType();
                listRowType.Add(rowType.FullName);
            }
            
            return listRowType;
        }
        

        private void ExportToClipboard_WithHeader()
        {
            List<List<string>> listRow = ConvertTableRows_To_ListRowString();
            List<string> listHeader = CreateListHeader();
            
            
            if (includeRowType)
            {
                //Thêm row type
                listHeader.Add(SOTableConst.FIELD_NAME_ROW_TYPE);
                var listRowType = ConvertTableRows_To_ListRowType();
                
                for (int rowIndex = 0; rowIndex < listRow.Count; rowIndex++)
                {
                    var row = listRow[rowIndex];
                    row.Add(listRowType[rowIndex]);
                }
            }
            
            
            //Thêm row header
            listRow.Insert(0, listHeader);
            
            CsvUtility.ExportToClipboard(listRow);
        }

        private void ExportToClipboard_NoHeader()
        {
            List<List<string>> listRow = ConvertTableRows_To_ListRowString();
            
            
            if (includeRowType)
            {
                //Thêm row type
                var listRowType = ConvertTableRows_To_ListRowType();
                
                for (int i = 0; i < listRow.Count; i++)
                {
                    var row = listRow[i];
                    row.Add(listRowType[i]);
                }
            }
            
            
            CsvUtility.ExportToClipboard(listRow);
        }

        private void ExportToClipboard_OnlyRowType_WithHeader()
        {
            var listRowType = ConvertTableRows_To_ListRowType();
            listRowType.Insert(0, SOTableConst.FIELD_NAME_ROW_TYPE);
            
            CsvUtility.ExportOneColumnToClipboard(listRowType);
        }
        
        private void ExportToClipboard_OnlyRowType_NoHeader()
        {
            var listRowType = ConvertTableRows_To_ListRowType();
            CsvUtility.ExportOneColumnToClipboard(listRowType);
        }
        
        

        private string Convert_CamelCase_To_SnakeCase(string s)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                
                
                //Nếu char hiện tại là lower và char tiếp theo là upper thì thêm _ sau khi đã add char hiện tại
                //Nếu char hiện tại là upper, char trước đó cũng là upper và char tiếp theo là lower thì thêm _ trước khi add char hiện tại
                //listReward -> list_reward
                //testFIELDNay -> test_field_nay


                if (i >= s.Length - 1) //Ký tự cuối thì add thôi
                {
                    sb.Append(char.ToLower(c));
                    continue;
                }
                
                if (char.IsLower(c) && char.IsUpper(s[i + 1]))
                {
                    sb.Append(char.ToLower(c));
                    sb.Append('_');
                    continue;
                }

                if (char.IsUpper(c) && char.IsLower(s[i + 1]) && i > 0 && char.IsUpper(s[i - 1]))
                {
                    sb.Append('_');
                    sb.Append(char.ToLower(c));
                    continue;
                }

                sb.Append(char.ToLower(c));
            }

            return sb.ToString();
        }

        private void Export_OnTableAssetChanged()
        {
            includeRowType = CalculateRecommendOption_IncludeRowType();
        }
    }
}