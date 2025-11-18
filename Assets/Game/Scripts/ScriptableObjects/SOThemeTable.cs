using System.Collections.Generic;
using UnityEngine;
using ZeroX.DataTableSystem.SoTableSystem;

public enum ThemeType
{
	Normal,
	Hard,
}

[System.Serializable]
public class ColorTheme
{
	public ThemeType themeType;
	public Material  pathMaterial;
	public Material  backgroundMaterial;
	public Material  tunnelMaterial;
	public Material  holeMaterial;
}

[CreateAssetMenu(menuName = "Database/ThemeTable/Table")]
public class SOThemeTable : SoTableOneId<SOThemeTable, ColorTheme, ThemeType>
{
	protected override string ShortMainPath => "Database/ThemeTable";
	
	protected override ThemeType GetRowId(ColorTheme row)
	{
		return row.themeType;
	}
}