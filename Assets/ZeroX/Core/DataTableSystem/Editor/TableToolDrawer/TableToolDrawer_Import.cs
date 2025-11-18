using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ZeroX.CsvTools;

namespace ZeroX.DataTableSystem.Editors
{
    public partial class TableToolDrawer
    {
        //Import
        private string csvFilePath = "";
        private readonly string[] listFieldSeparator = new string[]
        {
            ",", ";", "|"
        };
        
        private readonly string[] listQuoteCharacter = new string[]
        {
            "\"", "'"
        };

        private static int fieldSeparatorIndexSelected = 0;
        private static int quoteCharacterIndexSelected;
        private static bool trimField = true;
        private static SORowNameStyle soRowNameStyle = SORowNameStyle.Id;
        

        
        //Data Analyzed
        private List<string> listCsvColumnName = new List<string>();
        private List<string> listField = new List<string>();
        private List<List<string>> listCsvRow = new List<List<string>>();
        
        
        //Convert Setting
        private bool RemoveWordSeparator
        {
            get => EditorPrefs.GetBool("ZeroX.DataTableSystem.Editors.RemoveWordSeparator", true);
            set => EditorPrefs.SetBool("ZeroX.DataTableSystem.Editors.RemoveWordSeparator", value);
        }

        private bool UpperCaseAfterWordSeparator
        {
            get => EditorPrefs.GetBool("ZeroX.DataTableSystem.Editors.UpperCaseAfterWordSeparator", true);
            set => EditorPrefs.SetBool("ZeroX.DataTableSystem.Editors.UpperCaseAfterWordSeparator", value);
        }
        
        
        
        
        
        
        
        
        
        private void DrawTab_Import()
        {
            DrawChooseFileCsv();
            GUILayout.Space(15);
            DrawSetting();
            
            if (string.IsNullOrEmpty(csvFilePath) == false)
            {
                GUILayout.Space(15);
                
                DrawButtonAnalytics();
                DrawButtonImport();
                
                if (listCsvColumnName.Count > 0)
                {
                    GUILayout.Space(15);
                    DrawColumnAndField();
                }
            }
        }
        
        void DrawChooseFileCsv()
        {
            GUILayout.BeginHorizontal();
            csvFilePath = EditorGUILayout.TextField("File CSV", csvFilePath);

            bool clicked = GUILayout.Button("Choose CSV File", GUILayout.Width(120));
            GUILayout.EndHorizontal();

            if (clicked)
            {
                string lastFolder = "";
                if (string.IsNullOrWhiteSpace(csvFilePath) == false)
                {
                    string directory = Path.GetDirectoryName(csvFilePath);
                    if (string.IsNullOrWhiteSpace(directory) == false)
                    {
                        if (Directory.Exists(directory))
                            lastFolder = directory;
                    }
                }
                
                string newFilePath = EditorUtility.OpenFilePanel("Choose file csv", lastFolder, "csv");
                if (string.IsNullOrEmpty(newFilePath) == false && newFilePath != csvFilePath)
                {
                    csvFilePath = newFilePath;
                    ClearDataAnalyzed();
                }
                    
            }
        }

        void ClearDataAnalyzed()
        {
            listCsvColumnName.Clear();
            listCsvRow.Clear();
            listField.Clear();
        }

        void DrawSetting()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            
            GUILayout.Label("Import Settings", EditorStyles.boldLabel);
            fieldSeparatorIndexSelected = EditorGUILayout.Popup("Field Separator", fieldSeparatorIndexSelected, listFieldSeparator);
            quoteCharacterIndexSelected = EditorGUILayout.Popup("Quote Character", quoteCharacterIndexSelected, listQuoteCharacter);
            trimField = EditorGUILayout.Toggle("Trim Field", trimField);


            if (SOTableReflection.GetDeclaredRowType(tableAsset).IsSubclassOf(typeof(ScriptableObject)))
            {
                GUILayout.Space(15);
                soRowNameStyle = (SORowNameStyle) EditorGUILayout.EnumPopup("SO Row Name Style", soRowNameStyle);
            }
            
            GUILayout.EndVertical();
        }

        void DrawButtonAnalytics()
        {
            if (GUILayout.Button("Analytics", GUILayout.Height(25)))
            {
                ClearDataAnalyzed();
                
                if (File.Exists(csvFilePath) == false)
                {
                    Debug.LogError("File csv not exist!");
                    return;
                }

                char fieldSeparator = listFieldSeparator[fieldSeparatorIndexSelected][0];
                char quoteCharacter = listQuoteCharacter[quoteCharacterIndexSelected][0];
                var listRow = CsvUtility.ConvertCsvToListCsvRow(File.ReadAllText(csvFilePath), fieldSeparator, quoteCharacter, trimField);
                if (listRow.Count == 0)
                {
                    Debug.Log("Csv file empty");
                }
                else
                {
                    listCsvColumnName = listRow[0];
                    
                    listRow.RemoveAt(0);
                    listCsvRow = listRow;
                    
                    ConvertListCsvColumnToListField();
                }
            }
        }

        void ConvertListCsvColumnToListField()
        {
            listField = CsvUtility.ConvertListColumnNameToListFieldName(listCsvColumnName, UpperCaseAfterWordSeparator, RemoveWordSeparator);
        }

        void DrawButtonImport()
        {
            bool oldEnable = GUI.enabled;
            GUI.enabled = listCsvRow.Count > 0 && listField.Count > 0;
            if (GUILayout.Button("Import", GUILayout.Height(25)) == false)
            {
                GUI.enabled = oldEnable;
                return;
            }
            GUI.enabled = oldEnable;

            SOTableReflection.Invoke_Editor_ImportFromCsv(tableAsset, listField, listCsvRow, soRowNameStyle);
            Debug.Log("Import process completed");
        }

        
        void DrawColumnAndField()
        {
            GUILayout.BeginHorizontal();
            
            //Column
            GUILayout.BeginVertical();
            GUILayout.Label("Columns", EditorStyles.boldLabel);
            foreach (var col in listCsvColumnName)
            {
                GUILayout.Label(col);
            }
            GUILayout.EndVertical();
            
            
            //Convert Setting
            GUILayout.BeginVertical();
            GUILayout.Label("Convert Setting", EditorStyles.boldLabel);
            RemoveWordSeparator = GUILayout.Toggle(RemoveWordSeparator, "Remove word separator");
            UpperCaseAfterWordSeparator = GUILayout.Toggle(UpperCaseAfterWordSeparator, "Upper case after word separator");
            if (GUILayout.Button("Convert", GUILayout.Width(70)))
            {
                ConvertListCsvColumnToListField();
            }
            
            GUILayout.EndVertical();
            
            
            //Field
            GUILayout.BeginVertical();
            GUILayout.Label("Fields", EditorStyles.boldLabel);
            for (int i = 0; i < listField.Count; i++)
            {
                listField[i] = GUILayout.TextField(listField[i]);
            }
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();
        }
        
        
        private void Import_OnTableAssetChanged()
        {
            ClearDataAnalyzed();
        }
    }
}