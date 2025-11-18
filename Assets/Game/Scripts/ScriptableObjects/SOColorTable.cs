using System.Collections.Generic;
using UnityEngine;
using ZeroX.DataTableSystem.SoTableSystem;

[System.Serializable] public class ColorRow
{
	public int      colorID;
	public Color    color_Code;
	public Color    color_Particle;
	public Gradient gradient_Trail;
	public Material material;
}

[CreateAssetMenu(menuName = "Database/ColorTable/Table")]
public class SOColorTable : SoTableOneId<SOColorTable, ColorRow, int>
{
	protected override string ShortMainPath => "SOColorTable";
	
	protected override int GetRowId(ColorRow row)
	{
		return row.colorID;
	}

	public static List<int> GetRandomColors()
	{
		List<int> colors = new List<int>();
		int amount = Random.Range(0, 4);
		for (int i = 0; i < amount; i++)
		{
			int randomColor = Random.Range(0, 7);
			colors.Add(i);
		}
		return colors;
	}
}